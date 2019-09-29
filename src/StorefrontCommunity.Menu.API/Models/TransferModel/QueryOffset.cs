using System.ComponentModel.DataAnnotations;

namespace StorefrontCommunity.Menu.API.Models.TransferModel
{
    public class QueryOffset
    {
        /// <summary>
        /// Index to start search (like indexes in an array). Minimum 0.
        /// </summary>
        /// <value></value>
        [Required, Range(0, long.MaxValue)]
        public int? Index { get; set; } = 0;

        /// <summary>
        /// Number of items in the search result (default 30). Minimum 1 and maximum 50.
        /// </summary>
        /// <value></value>
        [Required, Range(1, 50)]
        public int? Length { get; set; } = 30;
    }
}
