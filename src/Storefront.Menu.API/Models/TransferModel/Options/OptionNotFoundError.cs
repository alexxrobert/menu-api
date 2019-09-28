using Storefront.Menu.API.Models.TransferModel;

namespace Storefront.Menu.API.Models.TransferModel.Options
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
