using System.ComponentModel.DataAnnotations;
using StorefrontCommunity.Menu.API.Models.DataModel.Options;
using StorefrontCommunity.Menu.API.Models.TransferModel.Validations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Options
{
    public sealed class SaveOptionJson
    {
        /// <summary>
        /// Group option ID.
        /// </summary>
        /// <value></value>
        [Required]
        public long? GroupId { get; set; }

        /// <summary>
        /// Name of the option.
        /// </summary>
        /// <value></value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the option.
        /// </summary>
        /// <value></value>
        [Required, MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Price of the option.
        /// </summary>
        /// <value></value>
        [Required, MinValue(0), Precision(5,2)]
        public decimal Price { get; set; }

        /// <summary>
        /// Indicates if the option is available (true) or not (false).
        /// </summary>
        /// <value></value>
        [Required]
        public bool? IsAvailable { get; set; }

        public Option MapTo(Option option)
        {
            option.OptionGroupId = GroupId.Value;
            option.Name = Name;
            option.Description = Description;
            option.Price = Price;
            option.IsAvailable = IsAvailable.Value;

            return option;
        }
    }
}
