using Storefront.Menu.API.Models.TransferModel;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class GroupHasItemsError : UnprocessableEntityError
    {
        public const string ErrorName = "ITEM_GROUP_HAS_ITEMS";

        public GroupHasItemsError()
        {
            Error = ErrorName;
        }
    }
}
