using System.Linq;
using NUnit.Framework;
using SympleLib.OpenXml;

namespace SympleTests.OpenXml
{
    class XLSWorkerReadTests
    {
        readonly string _testSource = System.IO.Path.GetFullPath(@"OpenXml\datasource.xlsx");

        private XLSWorker _testWorker;
        public XLSWorker TestWorker
        {
            get
            {
                if(this._testWorker == null)
                {
                    this._testWorker = XLSWorker.CreateOrOpen(_testSource);
                    this._testWorker.SetActiveSheet("Proposed Validation Sheet");
                }

                return this._testWorker;
            }
        }

        [Test]
        public void SetActiveSheetById()
        {
            TestWorker.SetActiveSheet(2);
            Assert.NotNull(TestWorker.CurrentSheet);
            Assert.AreEqual(TestWorker.CurrentSheet.Name, "Proposed Validation Sheet");
        }

        [Test]
        public void SetActiveSheetByName()
        {
            TestWorker.SetActiveSheet("Allocated Server Level Licenses");
            Assert.AreEqual(TestWorker.CurrentSheet.Name, "Allocated Server Level Licenses");
        }

        [Test]
        public void Read_In_Column_Names()
        {
            Assert.AreEqual(TestWorker.Columns[0], "id");
            Assert.AreEqual(TestWorker.Columns[5], "Virtual Machine");
        }

        [Test]
        public void Read_In_Toal_Row_Count_skipping_Header()
        {
            TestWorker.SetActiveSheet("Proposed Validation Sheet");
            Assert.AreEqual(TestWorker.Rows.Count(), 2906);
        }

        [Test]
        public void Read_Row_Content_by_ColName()
        {
            var inspect_row = TestWorker.Rows[4];
            Assert.AreEqual(inspect_row["Machine"].GetValue<string>(), "15471WS");
            Assert.AreEqual(inspect_row["Company"].GetValue<string>(), "VMC");
        }

        [Test]
        public void Search_Tests()
        {
            var search_result = TestWorker.Rows.Where(x => x["Company"].GetValue<string>() == "VMC");

            Assert.AreEqual(search_result.Count(), 383);
        }

        [Test]
        public void ApplyFilterTest()
        {
            TestWorker.ApplyFilter("Business Unit", "ARCTERN");
            Assert.AreEqual(TestWorker.Rows.Count(), 42);
        }
    }
}
