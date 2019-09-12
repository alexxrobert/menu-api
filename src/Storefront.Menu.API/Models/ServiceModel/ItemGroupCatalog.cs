using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.API.Models.ServiceModel
{
    public sealed class ItemGroupCatalog
    {
        private readonly ApiDbContext _dbContext;

        public ItemGroupCatalog(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ItemGroup ItemGroup { get; private set; }
        public bool GroupNotExists { get; private set; }
        public bool GroupHasItems { get; private set; }

        public async Task Add(ItemGroup itemGroup)
        {
            ItemGroup = itemGroup;

            _dbContext.Add(ItemGroup);

            await _dbContext.SaveChangesAsync();
        }

        public async Task Find(long tenantId, long itemGroupId)
        {
            ItemGroup = await _dbContext.ItemGroups
                .WhereId(tenantId, itemGroupId)
                .SingleOrDefaultAsync();

            GroupNotExists = ItemGroup == null;
        }

        public async Task Update()
        {
            if (GroupNotExists) return;

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete()
        {
            if (GroupNotExists) return;

            GroupHasItems = await _dbContext.Items
                .WhereItemGroupId(ItemGroup.TenantId, ItemGroup.Id)
                .AnyAsync();

            if (GroupHasItems) return;

            _dbContext.ItemGroups.Remove(ItemGroup);

            await _dbContext.SaveChangesAsync();
        }
    }
}
