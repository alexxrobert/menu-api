using System;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;

namespace Storefront.Menu.Tests.Factories.Options
{
    public static class OptionFactory
    {
        public static Option To(this Option option, OptionGroup optionGroup, bool available = true)
        {
            option.TenantId = optionGroup.TenantId;
            option.Name = ConstantFactory.Text();
            option.Description = ConstantFactory.Text(length: 50, wordCount: 6);
            option.Price = 5;
            option.IsAvailable = available;
            option.OptionGroup = optionGroup;

            return option;
        }
    }
}
