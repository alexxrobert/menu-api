using System.ComponentModel.DataAnnotations;

namespace Storefront.Menu.API.Models.TransferModel.Options
{
    public sealed class OptionListQuery : QueryOffset
    {
        /// <summary>
        /// Name of the option.
        /// </summary>
        /// <value></value>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if option is available (true) or not (false). Null means all options.
        /// </summary>
        /// <value></value>
        public bool? Available { get; set; }
    }
}
