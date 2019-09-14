using System;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;

namespace Storefront.Menu.Tests.Factories.Items
{
    public static class ItemFactory
    {
        public static Item To(this Item item, ItemGroup itemGroup, bool available = true)
        {
            item.TenantId = itemGroup.TenantId;
            item.Name = ConstantFactory.Text();
            item.Description = ConstantFactory.Text(length: 50, wordCount: 6);
            item.PictureFileId = Guid.NewGuid().ToString();
            item.Price = 5;
            item.IsAvailable = available;
            item.ItemGroup = itemGroup;

            return item;
        }
    }
}
