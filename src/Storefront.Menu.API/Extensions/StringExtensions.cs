using System;

namespace Storefront.Menu.API.Extensions
{
    public static class StringExtensions
    {
        public static string[] Words(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return new string[] { };

            return str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
