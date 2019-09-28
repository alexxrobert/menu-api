using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storefront.Menu.API.Models.DataModel.Options;

namespace Storefront.Menu.API.Models.TransferModel.Options
{
    public sealed class OptionListJson : IActionResult
    {
        public OptionListJson() { }

        public OptionListJson(ICollection<Option> options, long count)
        {
            Options = options
                .Select(option => new OptionJson(option))
                .ToList();

            Count = count;
        }

        public ICollection<OptionJson> Options { get; set; }
        public long Count { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}
