using StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups;

namespace StorefrontCommunity.Menu.Tests.Factories.OptionGroups
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
