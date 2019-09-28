using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;
using Storefront.Menu.API.Models.ServiceModel;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.OptionGroups;

namespace Storefront.Menu.API.Controllers
{
    /// <summary>
    /// Group of options allowed for the user to choose
    /// </summary>
    [ApiExplorerSettings(GroupName = "Option groups")]
    [Route("option-groups"), Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class OptionGroupsController : Controller
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public OptionGroupsController(ApiDbContext dbContext, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Find an option group by ID.
        /// </summary>
        /// <param name="id">Option group ID.</param>
        /// <returns>Returns option group data.</returns>
        /// <response code="200">Option group data</response>
        /// <response code="422">Errors: ITEM_GROUP_NOT_FOUND</response>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Find([FromRoute] long id)
        {
            var catalog = new OptionGroupCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new OptionGroupNotFoundError();
            }

            return new OptionGroupJson(catalog.OptionGroup);
        }

        /// <summary>
        /// List option groups ordered by title. Search by: title.
        /// </summary>
        /// <param name="query">URL query sring parameters.</param>
        /// <returns>List of option groups</returns>
        /// <response code="200">Search result</response>
        [HttpGet, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionGroupListJson))]
        public async Task<IActionResult> List([FromQuery] OptionGroupListQuery query)
        {
            var optionGroupQuery = _dbContext.OptionGroups
                .WhereTenantId(User.Claims.TenantId())
                .WhereTitleContains(query.Title);

            var optionGroups = await optionGroupQuery
                .OrderByTitle()
                .Skip(query.Index.Value)
                .Take(query.Length.Value)
                .ToListAsync();

            var count = await optionGroupQuery.CountAsync();

            return new OptionGroupListJson(optionGroups, count);
        }

        /// <summary>
        /// Create an option group.
        /// </summary>
        /// <param name="json">Option group data.</param>
        /// <returns>Created option group.</returns>
        /// <response code="200">Option group data</response>
        [HttpPost, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionGroupListJson))]
        public async Task<IActionResult> Create([FromBody] SaveOptionGroupJson json)
        {
            var catalog = new OptionGroupCatalog(_dbContext, _messageBroker);

            var optionGroup = json.MapTo(new OptionGroup
            {
                TenantId = User.Claims.TenantId()
            });

            await catalog.Add(optionGroup);

            return new OptionGroupJson(catalog.OptionGroup);
        }

        /// <summary>
        /// Update an option group.
        /// </summary>
        /// <param name="id">Option group ID.</param>
        /// <param name="json">Option group data.</param>
        /// <returns>Updated option group</returns>
        /// <response code="200">Option group data</response>
        /// <response code="422">Errors: ITEM_GROUP_NOT_FOUND</response>
        [HttpPut, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Update([FromRoute] long id, [FromBody] SaveOptionGroupJson json)
        {
            var catalog = new OptionGroupCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new OptionGroupNotFoundError();
            }

            json.MapTo(catalog.OptionGroup);

            await catalog.Update();

            return new OptionGroupJson(catalog.OptionGroup);
        }

        /// <summary>
        /// Delete an option group.
        /// </summary>
        /// <param name="id">Option group ID.</param>
        /// <returns>No content</returns>
        /// <response code="200">Option group data</response>
        /// <response code="422">Errors: ITEM_GROUP_NOT_FOUND</response>
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(statusCode: 204, type: typeof(OptionGroupJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var catalog = new OptionGroupCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.GroupNotExists)
            {
                return new OptionGroupNotFoundError();
            }

            await catalog.Delete();

            if (catalog.GroupHasOptions)
            {
                return new GroupHasOptionsError();
            }

            return NoContent();
        }
    }
}
