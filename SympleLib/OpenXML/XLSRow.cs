using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ClosedXML.Excel;


namespace SympleLib.OpenXml
{
    public class XLSRow
    {
        protected List<string> Columns { get; set; }
        protected IXLRow BaseRow { get; set; }
        public IXLStyle Style
        {
            get { return this.BaseRow.Style; }
            set { this.BaseRow.Style = value; }
        }

        public XLSRow(List<string> columns, IXLRow baseRow)
        {
            this.Columns = columns;
            this.BaseRow = baseRow;
        }

        public IXLCell this[int index]
        {
            get
            {
                return this.BaseRow.Cell(index);
            }
        }

        public IXLCell this[string col]
        {
            get
            {
                if (this.Columns.Any(x=>x == col))
                {
                    int index = this.Columns.IndexOf(col);
                    return this[index + 1];
                }
                else
                {
                    return this.BaseRow.Cell(col);
                }
            }
        }
    }
}
