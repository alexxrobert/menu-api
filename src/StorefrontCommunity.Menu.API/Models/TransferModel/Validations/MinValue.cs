using System;
using System.ComponentModel.DataAnnotations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Validations
{
    public sealed class MinValue : ValidationAttribute
    {
        private double _min;

        public MinValue(double min)
        {
            _min = min;

            ErrorMessage = $"The field {{0}} be a number with a minimum value of {min}.";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if (value is int intValue)
            {
                return (double)intValue >= _min;
            }

            if (value is decimal decimalValue)
            {
                return (double)decimalValue >= _min;
            }

            if (value is double doubleValue)
            {
                return doubleValue >= _min;
            }

            throw new ArgumentException("The value must be a numeric type.");
        }
    }
}
