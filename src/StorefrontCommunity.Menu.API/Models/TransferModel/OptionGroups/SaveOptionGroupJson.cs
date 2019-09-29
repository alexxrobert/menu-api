using System.ComponentModel.DataAnnotations;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups
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
