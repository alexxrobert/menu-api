using System.ComponentModel.DataAnnotations;
using Storefront.Menu.API.Models.DataModel.OptionGroups;

namespace Storefront.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class SaveOptionGroupJson
    {
        /// <summary>
        /// Title of the option group.
        /// </summary>
        /// <value></value>
        [Required, MaxLength(50)]
        public string Title { get; set; }

        public OptionGroup MapTo(OptionGroup optionGroup)
        {
            optionGroup.Title = Title;

            return optionGroup;
        }
    }
}
