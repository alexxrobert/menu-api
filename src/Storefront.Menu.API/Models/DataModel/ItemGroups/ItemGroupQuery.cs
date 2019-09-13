using System.Linq;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Extensions;

namespace Storefront.Menu.API.Models.DataModel.ItemGroups
{
    public static class ItemGroupQuery
    {
        public static IQueryable<ItemGroup> WhereTenantId(this IQueryable<ItemGroup> itemGroups, long tenantId)
        {
            return itemGroups.Where(itemGroup => itemGroup.TenantId == tenantId);
        }

        public static IQueryable<ItemGroup> WhereId(this IQueryable<ItemGroup> itemGroups, long tenantId, long id)
        {
            return itemGroups.WhereTenantId(tenantId).Where(itemGroup => itemGroup.Id == id);
        }

        public static IQueryable<ItemGroup> WhereTitleContains(this IQueryable<ItemGroup> itemGroups, string title)
        {
            var words = title.Words();

            if (!words.Any())
            {
                return itemGroups;
            }

            return itemGroups.Where(itemGroup => EF.Functions.Like(
                ApiDbContext.Normalize(itemGroup.Title),
                ApiDbContext.Normalize($"%{string.Join('%', words)}%")
            ));
        }
    }
}
