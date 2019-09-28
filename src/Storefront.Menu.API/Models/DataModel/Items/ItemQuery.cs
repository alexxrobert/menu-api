using System.Linq;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Extensions;

namespace Storefront.Menu.API.Models.DataModel.Items
{
    public static class ItemQuery
    {
        public static IQueryable<Item> OrderByName(this IQueryable<Item> items)
        {
            return items.OrderBy(item => item.Name);
        }

        public static IQueryable<Item> WhereTenantId(this IQueryable<Item> items, long tenantId)
        {
            return items.Where(item => item.TenantId == tenantId);
        }

        public static IQueryable<Item> WhereKey(this IQueryable<Item> items, long tenantId, long itemId)
        {
            return items.Where(item => item.Id == itemId);
        }

        public static IQueryable<Item> WhereItemGroupId(this IQueryable<Item> items, long tenantId, long itemId)
        {
            return items.Where(item => item.ItemGroupId == itemId);
        }

        public static IQueryable<Item> WhereAvailability(this IQueryable<Item> items, bool? isAvailable)
        {
            if (isAvailable == null)
            {
                return items;
            }

            return items.Where(item => item.IsAvailable == isAvailable);
        }

        public static IQueryable<Item> WhereNameContains(this IQueryable<Item> items, string name)
        {
            var words = name.Words();

            if (!words.Any())
            {
                return items;
            }

            return items.Where(item => EF.Functions.Like(
                ApiDbContext.Normalize(item.Name),
                ApiDbContext.Normalize($"%{string.Join('%', words)}%")
            ));
        }
    }
}
