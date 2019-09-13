using System.Linq;

namespace Storefront.Menu.API.Models.DataModel.Items
{
    public static class ItemQuery
    {
        public static IQueryable<Item> WhereTenantId(this IQueryable<Item> items, long tenantId)
        {
            return items.Where(item => item.TenantId == tenantId);
        }

        public static IQueryable<Item> WhereItemGroupId(this IQueryable<Item> items, long tenantId, long itemGroupId)
        {
            return items.Where(item => item.Id == itemGroupId);
        }
    }
}
