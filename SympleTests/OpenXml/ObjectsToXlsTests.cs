using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SympleLib.OpenXml;

namespace SympleTests.OpenXml
{
    public class ObjectsToXlsTests
    {
        public class TestObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime BirthDate { get; set; }

            public static List<TestObject> Collection
            {
                get
                {
                    List<TestObject> collection = new List<TestObject>();
                    for (int i = 0; i < 100; i++)
                    {
                        collection.Add(new TestObject { 
                            Name = "I Am Number " + i,
                            Age = i + 10,
                            BirthDate = DateTime.Now.AddYears(-50).AddMonths(i *2)
                        });
                    }
                    return collection;
                }
            }
        }

        [Test]
        public void ObjectToXlsTest()
        {
            ObjectsToXls ob2xl = new ObjectsToXls(@"c:\obj2xlstest.xls");
            ob2xl.AddSheet(TestObject.Collection, "Test Objects");
            ob2xl.Save();

            Assert.IsTrue(File.Exists(@"c:\obj2xlstest.xls"));
        }
    }
}
