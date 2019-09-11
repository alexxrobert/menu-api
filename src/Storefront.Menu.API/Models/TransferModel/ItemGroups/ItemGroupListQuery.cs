using System.ComponentModel.DataAnnotations;

namespace Storefront.Menu.API.Models.TransferModel.ItemGroups
{
    public class ItemGroupListQuery : QueryOffset
    {
        [MaxLength(50)]
        public string Title { get; set; }
    }
}
