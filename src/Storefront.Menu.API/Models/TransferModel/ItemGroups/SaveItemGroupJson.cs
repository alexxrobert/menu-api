using System.ComponentModel.DataAnnotations;
using Storefront.Menu.API.Models.DataModel.ItemGroups;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public sealed class SaveItemGroupJson
    {
        /// <summary>
        /// Title of the item group
        /// </summary>
        /// <value></value>
        [Required, MaxLength(50)]
        public string Title { get; set; }

        public ItemGroup MapTo(ItemGroup itemGroup)
        {
            itemGroup.Title = Title;

            return itemGroup;
        }
    }
}
