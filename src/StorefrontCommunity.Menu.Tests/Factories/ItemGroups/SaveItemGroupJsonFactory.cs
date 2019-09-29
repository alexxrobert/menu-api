using StorefrontCommunity.Menu.API.Models.TransferModel.ItemGroups;

namespace StorefrontCommunity.Menu.Tests.Factories.ItemGroups
{
    public static class SaveItemGroupJsonFactory
    {
        public static SaveItemGroupJson Build(this SaveItemGroupJson json)
        {
            json.Title = ConstantFactory.Text();

            return json;
        }
    }
}
