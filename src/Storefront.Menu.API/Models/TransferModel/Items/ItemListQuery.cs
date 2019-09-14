using System.ComponentModel.DataAnnotations;

namespace Storefront.Menu.API.Models.TransferModel.Items
{
    public sealed class ItemListQuery : QueryOffset
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        /// <value></value>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if item is available (true) or not (false). Null means all items.
        /// </summary>
        /// <value></value>
        public bool? Available { get; set; }
    }
}
