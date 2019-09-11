using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.Tests.Factories.ItemGroups
{
    public static class ItemGroupFactory
    {
        public static ItemGroup Of(this ItemGroup itemGroup, long tenantId)
        {
            itemGroup.TenantId = tenantId;
            itemGroup.Title = ConstantFactory.Text();

            return itemGroup;
        }
    }
}
