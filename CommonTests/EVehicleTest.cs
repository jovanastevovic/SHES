using Common.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTests
{
    [TestFixture]
    public class EVehicleTest
    {
        private EVehicle ev;
        [OneTimeSetUp]
        public void SetUp()
        {
            ev = new EVehicle();
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new EVehicle());
        }
        [Test]
        public void ConstructorTestWithParams()
        {
            Assert.DoesNotThrow(() => new EVehicle("name", 3));
        }
        [Test]
        public void ReaminingCapacityPropertyTest()
        {
            double reaminingCapacity = 20;
            ev.RemainingCapacity = reaminingCapacity;
            Assert.AreEqual(ev.RemainingCapacity, reaminingCapacity);
        }
    }
}
