using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SympleLib.Helpers.MVC
{
    public class KendoUiHelper
    {
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection, HttpRequestBase request)
        {
            var skip = request["skip"].Parse<int>();
            var take = request["take"].Parse<int>();

            var sortOrd = request["sort[0][dir]"];
            var sortOn = request["sort[0][field]"];

            IOrderedQueryable<T> results = collection.Select(x => x).OrderBy("id");
            if (sortOn.IsNotEmpty())
            {
                results = sortOrd == "desc" ? results.OrderByDescending(sortOn) : results.OrderBy(sortOn);
            }

            //Convert Null Entries in String to ""
            var gridData = results.Skip(skip).Take(take);
            foreach(var gData in gridData)
            {
                typeof(T).GetProperties().ToList().ForEach(p=>
                    {
                        if (p.PropertyType == typeof(String))
                        {
                            if(p.GetValue(gData, null) == null)
                            {
                                p.SetValue(gData, "", null);
                            }
                        }
                    });
            }

            return new KendoGridResult<T>
            {
                Items = gridData,
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
