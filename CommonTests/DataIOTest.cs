using Common;
using Common.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests
{
    [TestFixture]
    public class DataIOTests
    {
        private DataIO dataIO;
        static object testNull = null;
        static List<string> testListstrings = new List<string> { "1", "2", "3", "4" };
        static string filenameNull = null;
        static string filenameOk = "Consumers.xml";


        [OneTimeSetUp]
        public void SetupTest()
        {
            dataIO = new DataIO();
        }
        [Test]
        public void SerializeTestNotThrow()
        {
            Assert.DoesNotThrow(() => dataIO.SerializeObject<object>(testNull, filenameNull));
        }
        [Test]
        public void SerializeTestOk()
        {
            Assert.DoesNotThrow(() => dataIO.SerializeObject<List<string>>(testListstrings, filenameOk));
        }
        [Test]
        public void DeserializeTesThrow()
        {
            Assert.Catch(() => dataIO.DeSerializeObject<List<string>>("wrong"));
        }
    }
}
