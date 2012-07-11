using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SympleLib.OpenXML.Reader
{
    public class OpenXLRow
    {
        protected WorkbookPart WbPart { get; set; }
        protected WorksheetPart WsPart { get; set; }
        protected List<string> Columns { get; set; }
        

        public OpenXLRow() { }

        public OpenXLRow(WorkbookPart wbPart, WorksheetPart wsPart, List<string> columns, Row xRow)
        {
            this.WbPart = wbPart;
            this.WsPart = wsPart;
            this.Columns = columns;
            this._cells = GetCells(xRow);
        }

        public OpenXLRow(WorkbookPart wbPart, WorksheetPart wsPart, List<string> columns, int rowNum)
            :
            this(wbPart, wsPart, columns, wsPart.Worksheet.Descendants<Row>().ElementAt(rowNum))
        {
        }

        public string this[int index]
        {
            get
            {
                if (this.CellValues == null)
                {
                    if (this.Cells.Count > index)
                    {
                        return this.ReadCell(this._cells.ElementAt(index));
                    }
                    return "";
                }
                return this.CellValues.Count > index ? this.CellValues.ElementAt(index) : "";
            }
        }

        public string this[string col]
        {
            get
            {
                int index = this.Columns.IndexOf(col);
                return this[index];
            }
        }

        public bool HasData
        {
            get { return this._cells.Any(x => x.CellValue != null && x.CellValue.InnerText != ""); }
        }

        private List<String> CellValues { get; set; }
        private IEnumerable<Cell> _cells { get; set; }
        public List<String> Cells
        {
            get
            {
                if (this.CellValues == null)
                {
                    this.CellValues = new List<string>();
                    foreach (var c in this._cells)
                    {
                        this.CellValues.Add(ReadCell(c));
                    }
                }
                return this.CellValues;
            }
        }

        protected String ReadCell(Cell cellToRead)
        {
            string val = cellToRead.InnerText;
            if (cellToRead.DataType != null)
            {
                switch (cellToRead.DataType.Value)
                {
                    case DocumentFormat.OpenXml.Spreadsheet.CellValues.SharedString:
                        var stringTable = this.WbPart.
                        GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        if (stringTable != null)
                        {
                            val = stringTable.SharedStringTable.
                            ElementAt(int.Parse(val)).InnerText;
                        }
                        break;

                    case DocumentFormat.OpenXml.Spreadsheet.CellValues.Boolean:
                        switch (val)
                        {
                            case "0":
                                val = "FALSE";
                                break;
                            default:
                                val = "TRUE";
                                break;
                        }
                        break;
                }
            }

            return val;
        }

        #region  Helpers


        /// <summary>
        /// Given a cell name, parses the specified cell to get the column name.
        /// </summary>
        /// <param name="cellReference">Address of the cell (ie. B2)</param>
        /// <returns>Column Name (ie. B)</returns>
        public string GetColumnName(string cellReference)
        {
            // Create a regular expression to match the column name portion of the cell name.
            var regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);

            return match.Value;
        }

        /// <summary>
        /// Given just the column name (no row index), it will return the zero based column index.
        /// Note: This method will only handle columns with a length of up to two (ie. A to Z and AA to ZZ). 
        /// A length of three can be implemented when needed.
        /// </summary>
        /// <param name="columnName">Column Name (ie. A or AB)</param>
        /// <returns>Zero based index if the conversion was successful; otherwise null</returns>
        public int? GetColumnIndexFromName(string columnName)
        {
            var theAlphabet = Enumerable.Range('A', 26).Select(i => (char)i).ToList();
            var colLetters = columnName.ToCharArray();

            if (colLetters.Length == 1)
            {
                return theAlphabet.IndexOf(colLetters[0]);
            }
            var multiplier = (theAlphabet.IndexOf(colLetters[0]) + 1) * 26;
            return multiplier + theAlphabet.IndexOf(colLetters[1]);
        }

        public IEnumerable<Cell> GetCells(Row xRow)
        {
            var newRow = new List<Cell>();

            int columnIndex = 0;
            foreach (Cell cell in xRow.Descendants<Cell>())
            {
                // Gets the column index of the cell with data
                int? columnIndexFromName = GetColumnIndexFromName(GetColumnName(cell.CellReference));
                if (columnIndexFromName != null)
                {
                    var cellColumnIndex = (int)columnIndexFromName;

                    if (columnIndex < cellColumnIndex)
                    {
                        do
                        {
                            newRow.Add(new Cell());
                            //tempRow[columnIndex] = //Insert blank data here;
                            columnIndex++;
                        }
                        while (columnIndex < cellColumnIndex);
                    }
                }
                newRow.Add(cell);
                columnIndex++;
            }

            return newRow;
        }
        #endregion

    }
}
