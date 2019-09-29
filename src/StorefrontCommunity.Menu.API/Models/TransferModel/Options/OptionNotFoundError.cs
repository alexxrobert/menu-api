using StorefrontCommunity.Menu.API.Models.TransferModel;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Options
{
    public sealed class OptionNotFoundError : UnprocessableEntityError
    {
        public const string ErrorName = "OPTION_NOT_FOUND";

        public OptionNotFoundError()
        {
            Error = ErrorName;
        }
    }
}
