using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;
using Storefront.Menu.API.Models.EventModel.Published.OptionGroups;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.ServiceModel
{
    public sealed class OptionGroupCatalog
    {
        private readonly ApiDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public OptionGroupCatalog(ApiDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public OptionGroup OptionGroup { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool GroupHasOptions { get; private set; }

        public async Task Add(OptionGroup optionGroup)
        {
            OptionGroup = optionGroup;

            _dbContext.Add(OptionGroup);

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new OptionGroupCreatedEvent(OptionGroup));
        }

        public async Task Find(long tenantId, long itemGroupId, long optionGroupId)
        {
            OptionGroup = await _dbContext.OptionGroups
                .WhereKey(tenantId, optionGroupId)
                .WhereItemGroupId(itemGroupId)
                .SingleOrDefaultAsync();

            GroupNotExists = OptionGroup == null;
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new OptionGroupUpdatedEvent(OptionGroup));
        }

        public async Task Delete()
        {
            GroupHasOptions = await _dbContext.Options
                .WhereOptionGroupId(OptionGroup.TenantId, OptionGroup.Id)
                .AnyAsync();

            if (GroupHasOptions) return;

            _dbContext.OptionGroups.Remove(OptionGroup);

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new OptionGroupDeletedEvent(OptionGroup));
        }
    }
}
