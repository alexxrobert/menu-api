using StorefrontCommunity.Menu.API.Models.TransferModel;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Items
{
    public sealed class ItemNotFoundError : UnprocessableEntityError
    {
        public const string ErrorName = "ITEM_NOT_FOUND";

        public ItemNotFoundError()
        {
            Error = ErrorName;
        }
    }
}
