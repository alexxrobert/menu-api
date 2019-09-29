using StorefrontCommunity.Menu.API.Models.TransferModel;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class OptionGroupNotFoundError : UnprocessableEntityError
    {
        public const string ErrorName = "OPTION_GROUP_NOT_FOUND";

        public OptionGroupNotFoundError()
        {
            Error = ErrorName;
        }
    }
}
