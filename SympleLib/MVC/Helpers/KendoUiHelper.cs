using System.Collections.Generic;
using System.Linq;
using System.Web;
using SympleLib.Helpers;

namespace SympleLib.MVC.Helpers
{
    public class KendoUiHelper
    {
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection, HttpRequestBase request)
        {
            var skip = request["skip"].Parse<int>();
            var take = request["take"].Parse<int>();

            var sortOrd = request["sort[0][dir]"];
            var sortOn = request["sort[0][field]"];

            var results = collection.Select(x => x);
            if (sortOn.IsNotEmpty())
            {
                results = sortOrd == "desc" ? results.OrderByDescending(sortOn) : results.OrderBy(sortOn);
            }

            results = results.Skip(skip).Take(take);

            return new KendoGridResult<T>
            {
                Items = results,
                TotalCount = collection.Count()
            };
        }

        public class KendoGridResult<T>
        {
            public IEnumerable<T> Items { get; set; }
            public int TotalCount { get; set; }
        }
    }
}
