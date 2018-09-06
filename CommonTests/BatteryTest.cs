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
    public class BatteryTest
    {
        private Battery b;
        [OneTimeSetUp]
        public void SetUp()
        {
            b = new Battery();
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new Battery());
        }
        [Test]
        public void ConstructorTestWithParams()
        {
            Assert.DoesNotThrow(() => new Battery("b1", 12, 12));
        }

        [Test]
        public void CapacityPropertyTest()
        {
            double capacity = 2;
            b.Capacity = capacity;
            Assert.AreEqual(b.Capacity, capacity);
        }

        [Test]
        public void IsChargingPropertyTest()
        {
            bool isCharging = true;
            b.IsCharging = isCharging;
            Assert.AreEqual(b.IsCharging, isCharging);
        }
        [Test]
        public void IsDisChargingPropertyTest()
        {
            bool isDisCharging = true;
            b.IsDisCharging = isDisCharging;
            Assert.AreEqual(b.IsDisCharging, isDisCharging);
        }
        [Test]
        public void ReaminingCapacityPropertyTest()
        {
            double reaminingCapacity = 20;
            b.RemainingCapacity = reaminingCapacity;
            Assert.AreEqual(b.RemainingCapacity, reaminingCapacity);
        }

    }
}
