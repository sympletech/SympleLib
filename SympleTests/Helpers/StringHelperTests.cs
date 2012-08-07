using System;
using NUnit.Framework;
using SympleLib.Helpers;

namespace SympleTests.Helpers
{
    
    public class StringHelperTests
    {
        [Test]
        public void ParseIntTest()
        {
            string rawText = "45";
            int result = rawText.Parse<int>();
            Assert.AreEqual(result, 45);
        }

        [Test]
        public void ParseDateTimeTest()
        {
            string rawText = "8/7/2012";
            DateTime result = rawText.Parse<DateTime>();
            Assert.AreEqual(result, DateTime.Parse("8/7/2012"));
        }

        [Test]
        public void ParseBoolTest()
        {
            string rawText = "true";
            bool result = rawText.Parse<bool>();
            Assert.AreEqual(result, true);            
        }

        [Test]
        public void KnownParsersTest()
        {
            var i1 = "100".Parse<int>();
            Assert.IsTrue(StringHelpers.KnownParsers.ContainsKey(typeof (int)));
            var i2 = "150".Parse<int>();
        }
    }
}
