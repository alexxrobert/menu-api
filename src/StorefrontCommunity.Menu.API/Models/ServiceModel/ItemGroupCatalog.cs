using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorefrontCommunity.Menu.API.Models.DataModel;
using StorefrontCommunity.Menu.API.Models.DataModel.ItemGroups;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.EventModel.Published.ItemGroups;
using StorefrontCommunity.Menu.API.Models.IntegrationModel.EventBus;

namespace StorefrontCommunity.Menu.API.Models.ServiceModel
{
    public sealed class ItemGroupCatalog
    {
        private readonly ApiDbContext _dbContext;
        private readonly IMessageBroker _messageBroker;

        public ItemGroupCatalog(ApiDbContext dbContext, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _messageBroker = messageBroker;
        }

        public ItemGroup ItemGroup { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool GroupHasItems { get; private set; }

        public async Task Add(ItemGroup itemGroup)
        {
            ItemGroup = itemGroup;

            _dbContext.Add(ItemGroup);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new ItemGroupCreatedEvent(ItemGroup));
        }

        public async Task Find(long tenantId, long itemGroupId)
        {
            ItemGroup = await _dbContext.ItemGroups
                .WhereKey(tenantId, itemGroupId)
                .SingleOrDefaultAsync();

            GroupNotExists = ItemGroup == null;
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new ItemGroupUpdatedEvent(ItemGroup));
        }

        public async Task Delete()
        {
            GroupHasItems = await _dbContext.Items
                .WhereTenantId(ItemGroup.TenantId)
                .WhereItemGroupId(ItemGroup.Id)
                .AnyAsync();

            if (GroupHasItems) return;

            _dbContext.ItemGroups.Remove(ItemGroup);

            await _dbContext.SaveChangesAsync();

            _messageBroker.Publish(new ItemGroupDeletedEvent(ItemGroup));
        }
    }
}
