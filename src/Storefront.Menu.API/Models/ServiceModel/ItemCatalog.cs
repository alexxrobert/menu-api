using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.EventModel.Published.Items;
using Storefront.Menu.API.Models.IntegrationModel.EventBus;

namespace Storefront.Menu.API.Models.ServiceModel
{
    public sealed class ItemCatalog
    {
        private readonly ApiDbContext _dbContext;
        private readonly IEventBus _eventBus;

        public ItemCatalog(ApiDbContext dbContext, IEventBus eventBus)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
        }

        public Item Item { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool ItemNotExists { get; private set; }

        public async Task Add(Item item)
        {
            Item = item;

            await CheckIfGroupExists();

            if (GroupNotExists) return;

            _dbContext.Add(Item);

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new ItemCreatedEvent(Item));
        }

        public async Task Find(long tenantId, long itemId)
        {
            Item = await _dbContext.Items
                .WhereKey(tenantId, itemId)
                .SingleOrDefaultAsync();

            ItemNotExists = Item == null;
        }

        public async Task Update()
        {
            await CheckIfGroupExists();

            if (GroupNotExists) return;

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new ItemUpdatedEvent(Item));
        }

        public async Task Delete()
        {
            _dbContext.Items.Remove(Item);

            await _dbContext.SaveChangesAsync();

            _eventBus.Publish(new ItemDeletedEvent(Item));
        }

        private async Task CheckIfGroupExists()
        {
            var itemGroupCatalog = new ItemGroupCatalog(_dbContext, _eventBus);

            await itemGroupCatalog.Find(Item.TenantId, Item.ItemGroupId);

            GroupNotExists = itemGroupCatalog.GroupNotExists;
        }
    }
}
