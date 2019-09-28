using Storefront.Menu.API.Models.TransferModel.OptionGroups;

namespace Storefront.Menu.Tests.Factories.OptionGroups
{
    public static class SaveOptionGroupJsonFactory
    {
        public static SaveOptionGroupJson Build(this SaveOptionGroupJson json)
        {
            json.Title = ConstantFactory.Text();

            return json;
        }
    }
}
