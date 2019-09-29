using System;
using System.ComponentModel.DataAnnotations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Validations
{
    public class Precision : ValidationAttribute
    {
        private int _precision;
        private int _scale;

        public Precision(int precision) : this(precision, 0) { }

        public Precision(int precision, int scale)
        {
            _precision = precision;
            _scale = scale;

            ErrorMessage = $"The field {{0}} must be a number with a maximum precision of ({precision},{scale}).";
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            decimal number;

            if (decimal.TryParse(value.ToString(), out number))
            {
                var validPrecision = number % (decimal)Math.Pow(10, _precision - _scale) == number;
                var validScale = decimal.Round(number, _scale) == number;

                return validPrecision && validScale;
            }

            return false;
        }
    }
}
