using System.Linq;

namespace Storefront.Menu.API.Models.DataModel.Items
{
    public static class ItemQuery
    {
        public static IQueryable<Item> WhereItemGroupId(this IQueryable<Item> items, long itemGroupId)
        {
            return items.Where(item => item.Id == itemGroupId);
        }
    }
}
