using System.ComponentModel.DataAnnotations;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class ItemGroupListQuery : QueryOffset
    {
        /// <summary>
        /// Title of the item group.
        /// </summary>
        /// <value></value>
        [MaxLength(50)]
        public string Title { get; set; }
    }
}
