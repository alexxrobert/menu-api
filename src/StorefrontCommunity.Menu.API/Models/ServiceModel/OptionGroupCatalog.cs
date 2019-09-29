using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorefrontCommunity.Menu.API.Models.DataModel;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.EventModel.Published.OptionGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.ServiceModel
{
    public sealed class OptionGroupCatalog
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public OptionGroupCatalog(ApiDbContext dbContext, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
        }

        public OptionGroup OptionGroup { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool GroupHasOptions { get; private set; }

        public async Task Add(OptionGroup optionGroup)
        {
            OptionGroup = optionGroup;

            _dbContext.Add(OptionGroup);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionGroupCreatedEvent(OptionGroup));
        }

        public async Task Find(long tenantId, long optionGroupId)
        {
            OptionGroup = await _dbContext.OptionGroups
                .WhereKey(tenantId, optionGroupId)
                .SingleOrDefaultAsync();

            GroupNotExists = OptionGroup == null;
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionGroupUpdatedEvent(OptionGroup));
        }

        public async Task Delete()
        {
            GroupHasOptions = await _dbContext.Options
                .WhereOptionGroupId(OptionGroup.TenantId, OptionGroup.Id)
                .AnyAsync();

            if (GroupHasOptions) return;

            _dbContext.OptionGroups.Remove(OptionGroup);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionGroupDeletedEvent(OptionGroup));
        }
    }
}
