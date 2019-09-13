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
    [Route("item-groups"), Authorize]
    public sealed class ItemGroupsController : Controller
    {
        private readonly ApiDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public ItemGroupsController(ApiDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        [HttpGet, Route("{id:long}")]
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

        [HttpGet, Route("")]
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

            return new ItemGroupListJson(itemGroups);
        }

        [HttpPost, Route("")]
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

        [HttpPut, Route("{id:long}")]
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

        [HttpDelete, Route("{id:long}")]
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
