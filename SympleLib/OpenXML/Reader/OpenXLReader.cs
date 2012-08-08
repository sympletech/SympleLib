using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SympleLib.OpenXml.Reader
{
    public class OpenXLReader : IDisposable
    {
        public string WorkBookPath { get; set; }
        public Sheet ActiveSheet { get; set; }

        protected SpreadsheetDocument XLDoc { get; set; }
        protected WorkbookPart WbPart { get; set; }
        protected WorksheetPart WsPart { get; set; }
        protected IEnumerable<Sheet> Sheets { get; set; }

        public OpenXLReader(string path)
        {
            this.WorkBookPath = path;
            this.XLDoc = SpreadsheetDocument.Open(File.OpenRead(path), false);
            this.WbPart = this.XLDoc.WorkbookPart;
            this.Sheets = this.WbPart.Workbook.Descendants<Sheet>();
            SetActiveSheet(1);
        }

        #region Set Active Sheet

        public void SetActiveSheet(int sheetNum)
        {
            this.ActiveSheet = this.Sheets.ElementAt(sheetNum - 1);
            this.SetActiveSheet();
        }
        public void SetActiveSheet(string sheetName)
        {
            this.ActiveSheet = this.Sheets.FirstOrDefault(x => x.Name == sheetName);
            this.SetActiveSheet();
        }
        protected void SetActiveSheet()
        {
            if (this.ActiveSheet == null)
            {
                throw new Exception("Sheet Not Found");
            }
            this.WsPart = (WorksheetPart)(this.WbPart.GetPartById(this.ActiveSheet.Id));
            this.HeaderRow = 0;
            this._columns = null;
        }

        #endregion

        #region Header / Columns

        public int HeaderRow { get; set; }
        private List<string> _columns;
        public List<string> Columns
        {
            get
            {
                if (this._columns == null)
                {
                    var hRow = new OpenXLRow(this.WbPart, this.WsPart, new List<string>(), this.HeaderRow);

                    this._columns = new List<string>();
                    foreach (var hVal in hRow.Cells)
                    {
                        this._columns.Add(hVal);
                    }
                }
                return this._columns;
            }
        }

        #endregion

        #region Rows

        private List<OpenXLRow> _rows;
        public List<OpenXLRow> Rows
        {
            get
            {
                if (this._rows == null)
                {
                    var allRows = this.WsPart.Worksheet.Descendants<Row>();
                    allRows.ElementAt(this.HeaderRow).Remove();

                    this._rows = new List<OpenXLRow>();
                    foreach (var r in allRows)
                    {
                        this._rows.Add(new OpenXLRow(this.WbPart, this.WsPart, this.Columns, r));
                    }
                }

                return this._rows;
            }
        }

        #endregion
        
        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            this.XLDoc.Close();
            this.XLDoc.Dispose();
            this.XLDoc = null;

            if (disposing)
            {
                //Destroy Managed Resources?
            }
        }

        #endregion
    }
}
