using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.Tests.Factories.OptionGroups
{
    public static class OptionGroupFactory
    {
        public static OptionGroup To(this OptionGroup optionGroup, ItemGroup itemGroup)
        {
            optionGroup.TenantId = itemGroup.TenantId;
            optionGroup.Title = ConstantFactory.Text();
            optionGroup.ItemGroup = itemGroup;

            return optionGroup;
        }
    }
}
