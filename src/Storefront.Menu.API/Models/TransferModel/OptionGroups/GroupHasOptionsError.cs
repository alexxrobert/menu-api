using Storefront.Menu.API.Models.TransferModel.Errors;

namespace Storefront.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class GroupHasOptionsError : UnprocessableEntityError
    {
        public const string ErrorName = "OPTION_GROUP_HAS_OPTIONS";

        public GroupHasOptionsError()
        {
            Error = ErrorName;
        }
    }
}
