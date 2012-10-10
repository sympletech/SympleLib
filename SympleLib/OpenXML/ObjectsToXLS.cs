using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SympleLib.Helpers;
using System.ComponentModel;

namespace SympleLib.OpenXml
{
    public class ObjectsToXls
    {
        protected XLSWorker xWorker;

        public ObjectsToXls()
        {
            xWorker = new XLSWorker();
        }

        public ObjectsToXls(string saveTo)
        {
            xWorker = XLSWorker.Create(saveTo, true);
        }


        private PropertyDescriptorCollection objectProperties;

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName)
        {
            AddSheet(objectCollection, sheetName, null);
        }

        public void AddSheet(IEnumerable<object> objectCollection, string sheetName, List<string> includeProperties)
        {
            var objType = objectCollection.FirstOrDefault().GetType();
            this.objectProperties = TypeDescriptor.GetProperties(objType);

            if (includeProperties == null)
            {
                includeProperties = new List<string>();
                foreach (PropertyDescriptor p in this.objectProperties)
                {
                    if (ObjectHelpers.NativeTypes.Contains(p.PropertyType)){
                        includeProperties.Add(p.Name);
                    }
                }
            }

            xWorker.AddNewSheet(sheetName);

            xWorker.AddHeaderRow(includeProperties, 1);

            ReadInRecords(objectCollection, includeProperties);
        }

        private void ReadInRecords(IEnumerable<object> objectCollection, List<string> props)
        {
            foreach (var row in objectCollection)
            {
                var xRow = xWorker.AddNewRow();
                foreach (var objProp in props)
                {
                    xRow[objProp].Value = (string)this.objectProperties[objProp].GetValue(row);
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
            return xWorker.SaveToMemoryStream();
        }

        public void WriteToHttpResponse(string fileName)
        {
            xWorker.WriteToHttpResponse(fileName);
        }
    }
}