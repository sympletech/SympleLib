using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SympleLib.OpenXML
{
    public class ObjectsToXLS
    {
        protected XLSWorker xWorker;

        public ObjectsToXLS(string saveTo)
        {
            xWorker = XLSWorker.Create(saveTo);
        }


        /// <summary>Builds a sheet in the workbook from a collection of objects.  
        /// </summary>
        /// <param name="objectCollection">Collection to add in</param>
        /// <param name="sheetName">Name of the sheet</param>
        /// <param name="includeProperties">(optional) if included will limit the colums to those specified</param>
        public void AddSheet(IEnumerable<object> objectCollection, string sheetName, IEnumerable<string> includeProperties)
        {
            xWorker.AddNewSheet(sheetName);

            if (includeProperties == null)
            {
                var objProperties = objectCollection.FirstOrDefault().GetType().GetProperties();
            }
        }
    }
}
