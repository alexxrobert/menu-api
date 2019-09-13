using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Storefront.Menu.API.Models.TransferModel.Errors
{
    public sealed class JsonFormatErrorMessage
    {
        private readonly ModelError _modelError;

        public JsonFormatErrorMessage(ModelError modelError)
        {
            _modelError = modelError;
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(_modelError.ErrorMessage))
            {
                return _modelError.ErrorMessage;
            }

            if (_modelError.Exception is JsonReaderException readerException)
            {
                return readerException.Message;
            }

            if (_modelError.Exception is JsonSerializationException serializationException)
            {
                var matches = Regex.Matches(serializationException.Message, "Path.*");

                if (matches.Any())
                {
                    return $"Parse error: {matches.First().Value}";
                }
            }

            return "Could not read the request body.";
        }
    }
}
