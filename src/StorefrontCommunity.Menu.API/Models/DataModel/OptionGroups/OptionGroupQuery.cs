using System.Linq;
using Microsoft.EntityFrameworkCore;
using StorefrontCommunity.Menu.API.Extensions;

namespace StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups
{
    public static class OptionGroupQuery
    {
        public static IQueryable<OptionGroup> OrderByTitle(this IQueryable<OptionGroup> optionGroups)
        {
            return optionGroups.OrderBy(optionGroup => optionGroup.Title);
        }

        public static IQueryable<OptionGroup> WhereTenantId(this IQueryable<OptionGroup> optionGroups, long tenantId)
        {
            return optionGroups.Where(optionGroup => optionGroup.TenantId == tenantId);
        }

        public static IQueryable<OptionGroup> WhereKey(this IQueryable<OptionGroup> optionGroups,
            long tenantId, long id)
        {
            return optionGroups.WhereTenantId(tenantId).Where(optionGroup => optionGroup.Id == id);
        }

        public static IQueryable<OptionGroup> WhereTitleContains(this IQueryable<OptionGroup> optionGroups,
            string title)
        {
            var words = title.Words();

            if (!words.Any())
            {
                return optionGroups;
            }

            return optionGroups.Where(optionGroup => EF.Functions.Like(
                ApiDbContext.Normalize(optionGroup.Title),
                ApiDbContext.Normalize($"%{string.Join('%', words)}%")
            ));
        }
    }
}
