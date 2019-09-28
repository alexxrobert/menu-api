using System.Linq;
using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Extensions;

namespace Storefront.Menu.API.Models.DataModel.Options
{
    public static class OptionQuery
    {
        public static IQueryable<Option> OrderByName(this IQueryable<Option> options)
        {
            return options.OrderBy(option => option.Name);
        }

        public static IQueryable<Option> WhereTenantId(this IQueryable<Option> options, long tenantId)
        {
            return options.Where(option => option.TenantId == tenantId);
        }

        public static IQueryable<Option> WhereKey(this IQueryable<Option> options, long tenantId, long optionId)
        {
            return options.Where(option => option.Id == optionId);
        }

        public static IQueryable<Option> WhereOptionGroupId(this IQueryable<Option> options, long tenantId, long optionId)
        {
            return options.Where(option => option.OptionGroupId == optionId);
        }

        public static IQueryable<Option> WhereAvailability(this IQueryable<Option> options, bool? isAvailable)
        {
            if (isAvailable == null)
            {
                return options;
            }

            return options.Where(option => option.IsAvailable == isAvailable);
        }

        public static IQueryable<Option> WhereNameContains(this IQueryable<Option> options, string name)
        {
            var words = name.Words();

            if (!words.Any())
            {
                return options;
            }

            return options.Where(option => EF.Functions.Like(
                ApiDbContext.Normalize(option.Name),
                ApiDbContext.Normalize($"%{string.Join('%', words)}%")
            ));
        }
    }
}
