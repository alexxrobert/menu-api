using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorefrontCommunity.Menu.API.Models.DataModel;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.EventModel.Published.Options;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.ServiceModel
{
    public sealed class OptionCatalog
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public OptionCatalog(ApiDbContext dbContext, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
        }

        public Option Option { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool OptionNotExists { get; private set; }

        public async Task Add(Option option)
        {
            Option = option;

            await CheckIfGroupExists();

            if (GroupNotExists) return;

            _dbContext.Add(Option);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionCreatedEvent(Option));
        }

        public async Task Find(long tenantId, long optionId)
        {
            Option = await _dbContext.Options
                .WhereKey(tenantId, optionId)
                .SingleOrDefaultAsync();

            OptionNotExists = Option == null;
        }

        public async Task Update()
        {
            await CheckIfGroupExists();

            if (GroupNotExists) return;

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionUpdatedEvent(Option));
        }

        public async Task Delete()
        {
            _dbContext.Options.Remove(Option);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new OptionDeletedEvent(Option));
        }

        private async Task CheckIfGroupExists()
        {
            var optionGroupCatalog = new OptionGroupCatalog(_dbContext, _messageBroker);

            await optionGroupCatalog.Find(Option.TenantId, Option.OptionGroupId);

            GroupNotExists = optionGroupCatalog.GroupNotExists;
        }
    }
}
