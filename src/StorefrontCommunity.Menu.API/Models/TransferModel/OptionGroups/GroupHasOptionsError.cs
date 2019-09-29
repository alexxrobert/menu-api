using StorefrontCommunity.Menu.API.Models.TransferModel;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups
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
