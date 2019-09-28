using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Authorization;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;
using Storefront.Menu.API.Models.ServiceModel;
using Storefront.Menu.API.Models.TransferModel;
using Storefront.Menu.API.Models.TransferModel.OptionGroups;
using Storefront.Menu.API.Models.TransferModel.Options;

namespace Storefront.Menu.API.Controllers
{
    /// <summary>
    /// Options that belongs to an option group.
    /// </summary>
    [Route("options"), Authorize]
    [ApiExplorerSettings(GroupName = "Options")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public sealed class OptionsController : Controller
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public OptionsController(ApiDbContext dbContext, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
        }

        /// <summary>
        /// Find an option by ID.
        /// </summary>
        /// <param name="id">Option ID.</param>
        /// <returns>Returns option data.</returns>
        /// <response code="200">Option data</response>
        /// <response code="422">Errors: ITEM_NOT_FOUND</response>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Find([FromRoute] long id)
        {
            var catalog = new OptionCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.OptionNotExists)
            {
                return new OptionNotFoundError();
            }

            return new OptionJson(catalog.Option);
        }

        /// <summary>
        /// List options ordered by name. Search by: name, availability.
        /// </summary>
        /// <param name="query">URL query sring parameters.</param>
        /// <returns>List of options</returns>
        /// <response code="200">Search result</response>
        [HttpGet, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionListJson))]
        public async Task<IActionResult> List([FromQuery] OptionListQuery query)
        {
            var optionQuery = _dbContext.Options
                .WhereTenantId(User.Claims.TenantId())
                .WhereNameContains(query.Name)
                .WhereAvailability(query.Available);

            var options = await optionQuery
                .OrderByName()
                .Skip(query.Index.Value)
                .Take(query.Length.Value)
                .ToListAsync();

            var count = await optionQuery.CountAsync();

            return new OptionListJson(options, count);
        }

        /// <summary>
        /// Create an option.
        /// </summary>
        /// <param name="json">Option data.</param>
        /// <returns>Created option.</returns>
        /// <response code="200">Option data</response>
        [HttpPost, Route("")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionListJson))]
        public async Task<IActionResult> Create([FromBody] SaveOptionJson json)
        {
            var catalog = new OptionCatalog(_dbContext, _messageBroker);

            var option = json.MapTo(new Option
            {
                TenantId = User.Claims.TenantId()
            });

            await catalog.Add(option);

            if (catalog.GroupNotExists)
            {
                return new OptionGroupNotFoundError();
            }

            return new OptionJson(catalog.Option);
        }

        /// <summary>
        /// Update an option.
        /// </summary>
        /// <param name="id">Option ID.</param>
        /// <param name="json">Option data.</param>
        /// <returns>Updated option</returns>
        /// <response code="200">Option data</response>
        /// <response code="422">Errors: ITEM_NOT_FOUND, ITEM_GROUP_NOT_FOUND</response>
        [HttpPut, Route("{id:long}")]
        [ProducesResponseType(statusCode: 200, type: typeof(OptionJson))]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Update([FromRoute] long id,
            [FromBody] SaveOptionJson json)
        {
            var catalog = new OptionCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.OptionNotExists)
            {
                return new OptionNotFoundError();
            }

            json.MapTo(catalog.Option);

            await catalog.Update();

            if (catalog.GroupNotExists)
            {
                return new OptionGroupNotFoundError();
            }

            return new OptionJson(catalog.Option);
        }

        /// <summary>
        /// Delete an option.
        /// </summary>
        /// <param name="id">Option ID.</param>
        /// <returns>No content</returns>
        /// <response code="200">Option data</response>
        /// <response code="422">Errors: ITEM_NOT_FOUND, ITEM_GROUP_NOT_FOUND</response>
        [HttpDelete, Route("{id:long}")]
        [ProducesResponseType(statusCode: 204)]
        [ProducesResponseType(statusCode: 422, type: typeof(UnprocessableEntityError))]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            var catalog = new OptionCatalog(_dbContext, _messageBroker);
            var tenantId = User.Claims.TenantId();

            await catalog.Find(tenantId, id);

            if (catalog.OptionNotExists)
            {
                return new OptionNotFoundError();
            }

            await catalog.Delete();

            return NoContent();
        }
    }
}
