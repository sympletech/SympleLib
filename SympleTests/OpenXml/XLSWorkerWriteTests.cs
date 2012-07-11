using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClosedXML.Excel;
using NUnit.Framework;
using SympleLib.OpenXML;
using System.IO;

namespace SympleTests.OpenXml
{
    class XLSWorkerWriteTests
    {
        readonly string _testSource = System.IO.Path.GetFullPath(@"OpenXml\");

        public XLSWorker CreateTestWorker(string TestName)
        {
            return XLSWorker.Create(_testSource + TestName + ".xlsx", true);
        }

        [Test]
        public void CanCreateFileTest()
        {
            var tWorker = CreateTestWorker("CanCreateFileTest");
            tWorker.Save();

            Assert.IsTrue(File.Exists(tWorker.WorkBookPath));
        }

        [Test]
        public void CanWriteToA1Test()
        {
            var tWorker = CreateTestWorker("CanWriteToA1Test");
            tWorker.WriteToCell("A1", "Hello XLS");
            tWorker.Save();
        }

        [Test]
        public void CreateHeaderRowTest()
        {
            var tWorker = CreateTestWorker("CreateHeaderRowTest");
            List<string> HeaderRow = new List<string>{"Alpha", "Beta", "Charlie","Delta","Enuch"};
            tWorker.AddHeaderRow(HeaderRow, 1);
            tWorker.Save();
        }

        [Test]
        public void AddNewRowToSheet()
        {
            var tWorker = CreateTestWorker("AddNewRowToSheet");

            List<string> HeaderRow = new List<string> { "Alpha", "Beta", "Charlie", "Delta", "Enuch" };
            tWorker.AddHeaderRow(HeaderRow, 1);

            var new_row = tWorker.AddNewRow();
            new_row["Alpha"].Value = "Atest";
            new_row["Beta"].Value = "BTest";
            new_row[4].Value = "4test";

            tWorker.Save();
        }

        [Test]
        public void InsertRowTest()
        {
            var tWorker = CreateTestWorker("InsertRowTest");

            List<string> HeaderRow = new List<string> { "Alpha", "Beta", "Charlie", "Delta", "Enuch" };
            tWorker.AddHeaderRow(HeaderRow, 1);

            var xRow = tWorker.AddNewRow();
            xRow["Alpha"].Value = "First";
            xRow["Beta"].Value = "Row";

            var yRow = tWorker.InsertNewRow(2);
            yRow["Alpha"].Value = "Second";
            yRow["Beta"].Value = "Row";

            tWorker.Save();
        }

        [Test]
        public void AddNewStyledRow()
        {
            var tWorker = CreateTestWorker("AddNewStyledRow");

            List<string> HeaderRow = new List<string> { "Alpha", "Beta", "Charlie", "Delta", "Enuch" };
            tWorker.AddHeaderRow(HeaderRow, 1);

            var new_row = tWorker.AddNewRow();

            new_row.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            new_row.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            new_row.Style.Fill.BackgroundColor = XLColor.AirForceBlue;
            new_row.Style.Font.FontColor = XLColor.AntiqueFuchsia;

            new_row["Alpha"].Value = "Atest";
            new_row["Beta"].Value = "BTest";
            new_row[4].Value = "4test";

            tWorker.Save();            
        }

        [Test]
        public void FreezePanesTest()
        {
            var tWorker = CreateTestWorker("FreezePanesTest");

            List<string> HeaderRow = new List<string> { "Alpha", "Beta", "Charlie", "Delta", "Enuch" };
            tWorker.AddHeaderRow(HeaderRow, 1);

            for (int i = 0; i < 250; i++)
            {
                var new_row = tWorker.AddNewRow();
                new_row["Alpha"].Value = "Atest " + i;                
            }

            tWorker.Save();
        }

        [Test]
        public void AddNewSheetTest()
        {
            var tWorker = CreateTestWorker("AddNewSheetTest");

            List<string> HeaderRow = new List<string> { "Alpha", "Beta", "Charlie", "Delta", "Enuch" };
            tWorker.AddHeaderRow(HeaderRow, 1);

            var xRow = tWorker.AddNewRow();
            xRow["Alpha"].Value = "First";
            xRow["Beta"].Value = "Row";

            var yRow = tWorker.AddNewRow();
            yRow["Alpha"].Value = "Second";
            yRow["Beta"].Value = "Row";

            tWorker.AddNewSheet("Sheet Two");
            tWorker.AddHeaderRow(HeaderRow, 1);

            var zRow = tWorker.AddNewRow();
            zRow["Alpha"].Value = "Sheet";
            zRow["Beta"].Value = "Two";

            var qRow = tWorker.AddNewRow();
            qRow["Alpha"].Value = "Second";
            qRow["Beta"].Value = "Row";            

            tWorker.Save();            
        }

    }
}
