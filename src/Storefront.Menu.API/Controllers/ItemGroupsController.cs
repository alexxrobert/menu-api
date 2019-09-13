using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;
using Storefront.Menu.API.Models.ServiceModel;
using Storefront.Menu.API.Models.TransferModel.ItemGroups;

namespace Storefront.Menu.API.Controllers
{
    /// <summary>
    /// Group of items having particular shared characteristics.
    /// </summary>
    [ApiExplorerSettings(GroupName = "Item groups")]
    [Route("item-groups"), Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class ItemGroupsController : Controller
    {
        private readonly ApiDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public ItemGroupsController(ApiDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        /// <summary>
        /// Find an item group by ID.
        /// </summary>
        /// <param name="id">Item group ID.</param>
        /// <returns>Returns item group data.</returns>
        /// <response code="200">Item group data</response>
        /// <response code="422">Error: ITEM_GROUP_NOT_FOUND</response>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(ItemGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(ItemGroupNotFoundError))]
        public async Task<IActionResult> Find([FromRoute] long id)
        {
            var catalog = new ItemGroupCatalog(_dbContext, _eventBus);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new ItemGroupNotFoundError();
            }

            return new ItemGroupJson(catalog.ItemGroup);
        }

        /// <summary>
        /// List item groups ordered by title. Search by: title.
        /// </summary>
        /// <param name="query">URL query sring parameters.</param>
        /// <returns>List of item groups</returns>
        /// <response code="200">Search result</response>
        [HttpGet, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(ItemGroupListJson))]
        public async Task<IActionResult> List([FromQuery] ItemGroupListQuery query)
        {
            var itemGroupQuery = _dbContext.ItemGroups
                .WhereTenantId(User.Claims.TenantId())
                .WhereTitleContains(query.Title);

            var itemGroups = await itemGroupQuery
                .Skip(query.Index.Value)
                .Take(query.Length.Value)
                .ToListAsync();

            var count = await itemGroupQuery.CountAsync();

            return new ItemGroupListJson(itemGroups, count);
        }

        /// <summary>
        /// Create an item group.
        /// </summary>
        /// <param name="json">Item group data.</param>
        /// <returns>Created item group.</returns>
        /// <response code="200">Item group data</response>
        [HttpPost, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(ItemGroupListJson))]
        public async Task<IActionResult> Create([FromBody] SaveItemGroupJson json)
        {
            var catalog = new ItemGroupCatalog(_dbContext, _eventBus);

            var itemGroup = json.MapTo(new ItemGroup
            {
                TenantId = User.Claims.TenantId()
            });

            await catalog.Add(itemGroup);

            return new ItemGroupJson(catalog.ItemGroup);
        }

        /// <summary>
        /// Update an item group.
        /// </summary>
        /// <param name="id">Item group ID.</param>
        /// <param name="json">Item group data.</param>
        /// <returns>Updated item group</returns>
        /// <response code="200">Item group data</response>
        /// <response code="422">Error: ITEM_GROUP_NOT_FOUND</response>
        [HttpPut, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(ItemGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(ItemGroupNotFoundError))]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] SaveItemGroupJson json)
        {
            var catalog = new ItemGroupCatalog(_dbContext, _eventBus);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new ItemGroupNotFoundError();
            }

            json.MapTo(catalog.ItemGroup);

            await catalog.Update();

            return new ItemGroupJson(catalog.ItemGroup);
        }

        /// <summary>
        /// Delete an item group.
        /// </summary>
        /// <param name="id">Item group ID.</param>
        /// <returns>No content</returns>
        /// <response code="200">Item group data</response>
        /// <response code="422">Error: ITEM_GROUP_NOT_FOUND</response>
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(ItemGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(ItemGroupNotFoundError))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var catalog = new ItemGroupCatalog(_dbContext, _eventBus);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new ItemGroupNotFoundError();
            }

            await catalog.Delete();

            if (catalog.GroupHasItems)
            {
                return new GroupHasItemsError();
            }

            return NoContent();
        }
    }
}
