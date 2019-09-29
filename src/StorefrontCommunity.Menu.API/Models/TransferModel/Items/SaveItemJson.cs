using System.ComponentModel.DataAnnotations;
using StorefrontCommunity.Menu.API.Models.DataModel.Items;
using StorefrontCommunity.Menu.API.Models.TransferModel.Validations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.Items
{
    public sealed class SaveItemJson
    {
        /// <summary>
        /// Group item ID.
        /// </summary>
        /// <value></value>
        [Required]
        public long? GroupId { get; set; }

        /// <summary>
        /// Name of the item.
        /// </summary>
        /// <value></value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        /// <value></value>
        [Required, MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Price of the item.
        /// </summary>
        /// <value></value>
        [Required, MinValue(0), Precision(5,2)]
        public decimal Price { get; set; }

        /// <summary>
        /// Indicates if the item is available (true) or not (false).
        /// </summary>
        /// <value></value>
        [Required]
        public bool? IsAvailable { get; set; }

        public Item MapTo(Item item)
        {
            item.ItemGroupId = GroupId.Value;
            item.Name = Name;
            item.Description = Description;
            item.Price = Price;
            item.IsAvailable = IsAvailable.Value;

            return item;
        }
    }
}
