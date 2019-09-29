using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorefrontCommunity.Menu.API.Models.DataModel.OptionGroups;

namespace StorefrontCommunity.Menu.API.Models.TransferModel.OptionGroups
{
    public sealed class OptionGroupListJson : IActionResult
    {
        public OptionGroupListJson() { }

        public OptionGroupListJson(ICollection<OptionGroup> optionGroups, long count)
        {
            OptionGroups = optionGroups
                .Select(optionGroup => new OptionGroupJson(optionGroup))
                .ToList();

            Count = count;
        }

        public ICollection<OptionGroupJson> OptionGroups { get; set; }
        public long Count { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
