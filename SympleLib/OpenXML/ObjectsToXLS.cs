using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SympleLib.Helpers;

namespace SympleLib.OpenXml
{
    public class ObjectsToXls
    {
        protected XLSWorker xWorker;

        public ObjectsToXls(string saveTo)
        {
            xWorker = XLSWorker.Create(saveTo, true);
        }

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName)
        {
            AddSheet(objectCollection, sheetName, null);
        }

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName, IEnumerable<string> includeProperties)
        {
            xWorker.AddNewSheet(sheetName);

            var objType = objectCollection.FirstOrDefault().GetType();

            var props = LoadProperties(includeProperties, objType);
            xWorker.AddHeaderRow(props, 1);

            ReadInRecords(objectCollection, props, objType);
        }
    
        private List<string> LoadProperties(IEnumerable<string> includeProperties, Type objType)
        {
            List<string> iProps = includeProperties != null ? includeProperties.ToList() : new List<string>();

            if (iProps.Count() == 0)
            {
                var objProperties = objType.GetProperties();
                foreach (var prop in objProperties)
                {
                    if (ObjectHelpers.NativeTypes.Contains(prop.PropertyType))
                    {
                        iProps.Add(prop.Name);
                    }
                }
            }

            return iProps;
        }

        private void ReadInRecords(IEnumerable<object> objectCollection, List<string> props, Type objType)
        {
            foreach (var row in objectCollection)
            {
                var xRow = xWorker.AddNewRow();
                foreach (var objProp in props)
                {
                    var oProp = objType.GetProperty(objProp);
                    var pVal = oProp.GetValue(row, null);
                    xRow[oProp.Name].Value = pVal;
                }
            }
        }

        public void Save()
        {
            xWorker.Save();
        }

        public void SaveAs(string path)
        {
            xWorker.SaveAs(path);
        }

        public Stream GetFileStream()
        {
            return xWorker.GetFileStream();
        }
    }
}