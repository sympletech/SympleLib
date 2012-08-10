using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SympleLib.Helpers;

namespace SympleLib.MVC
{
    public class KendoUiHelper
    {
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection)
        {
            return ParseGridData<T>(collection, new KendoGridPost());
        }
        public static KendoGridResult<T> ParseGridData<T>(IQueryable<T> collection, KendoGridPost requestParams)
        {
            IOrderedQueryable<T> results;
            if(requestParams.SortOrd.IsNotEmpty())
            {
                results = requestParams.SortOrd == "desc" 
                    ? collection.OrderByDescending(requestParams.SortOn) 
                    : collection.OrderBy(requestParams.SortOn);
            }else
            {
                if (collection is IOrderedQueryable<T>)
                {
                    results = collection as IOrderedQueryable<T>;
                }
                else
                {
                    results = collection.Select(x => x).OrderBy("id");
                }                
            }

            //Convert Null Entries in String to "" (if not kendoui grid lists NULL Values as "NULL" in grids)
            var gridData = results.Skip(requestParams.Skip).Take(requestParams.Take);
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
