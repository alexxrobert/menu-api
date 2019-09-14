using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Models.TransferModel.Items
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
