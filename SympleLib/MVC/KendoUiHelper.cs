using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Bibliography;
using PagedList;
using SympleLib.Helpers;
using SympleLib.OpenXml;

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
            if (requestParams.Export.IsNotEmpty())
            {
                ReturnXlsExport<T>(collection, requestParams);                
            }else
            {
                return ReturnGridData<T>(requestParams, ref collection);                
            }

            return null;
        }
  
        
        private static void ReturnXlsExport<T>(IQueryable<T> collection, KendoGridPost requestParams)
        {
            var o2x = new ObjectsToXls();
            o2x.AddSheet((IEnumerable<object>)collection, requestParams.Export);
            o2x.WriteToHttpResponse(requestParams.Export + ".xlsx");
        }
  
        private static KendoGridResult<T> ReturnGridData<T>(KendoGridPost requestParams, ref IQueryable<T> collection)
        {
            if (requestParams.SortOrd.IsNotEmpty())
            {
                collection = requestParams.SortOrd == "desc"
                             ? collection.OrderByDescending(requestParams.SortOn)
                             : collection.OrderBy(requestParams.SortOn);
            }
            IPagedList<T> gridData;
            try
            {
                gridData = collection.ToPagedList(requestParams.Page, requestParams.PageSize);
            }
            catch
            {
                collection = collection.Select(x => x).OrderBy("id");
                gridData = collection.ToPagedList(requestParams.Page, requestParams.PageSize);
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
