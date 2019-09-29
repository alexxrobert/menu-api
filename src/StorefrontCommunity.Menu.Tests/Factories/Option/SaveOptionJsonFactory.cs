using StorefrontCommunity.Menu.API.Models.TransferModel.Options;

namespace StorefrontCommunity.Menu.Tests.Factories.Options
{
    public static class SaveOptionJsonFactory
    {
        public static SaveOptionJson Build(this SaveOptionJson json, long groupId)
        {
            json.GroupId = groupId;
            json.Name = ConstantFactory.Text();
            json.Description = ConstantFactory.Text();
            json.Price = 5;
            json.IsAvailable = true;

            return json;
        }
    }
}
