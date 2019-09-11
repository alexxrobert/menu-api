using System.ComponentModel.DataAnnotations;

namespace Storefront.Menu.API.Models.TransferModel
{
    public class QueryOffset
    {
        [Required, Range(0, long.MaxValue)]
        public int? Index { get; set; } = 0;

        [Required, Range(1, 50)]
        public int? Length { get; set; } = 30;
    }
}
