using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupNotFoundError : UnprocessableEntityError
    {
        public const string ErrorName = "ITEM_GROUP_NOT_FOUND";

        public ItemGroupNotFoundError()
        {
            Error = ErrorName;
        }
    }
}
