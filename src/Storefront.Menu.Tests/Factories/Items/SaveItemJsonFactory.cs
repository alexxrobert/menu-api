using Storefront.Menu.API.Models.TransferModel.Items;

namespace Storefront.Menu.Tests.Factories.Items
{
    public static class SaveItemJsonFactory
    {
        public static SaveItemJson Build(this SaveItemJson json, long groupId)
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
