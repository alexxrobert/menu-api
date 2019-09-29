using System.ComponentModel.DataAnnotations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class OptionGroupListQuery : QueryOffset
    {
        /// <summary>
        /// Title of the option group.
        /// </summary>
        /// <value></value>
        [MaxLength(50)]
        public string Title { get; set; }
    }
}
