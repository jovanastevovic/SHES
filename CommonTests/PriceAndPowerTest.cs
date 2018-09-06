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
    public class PriceAndPowerTest
    {
        private PriceAndPower pap;

        [OneTimeSetUp]
        public void Setup()
        {
            pap = new PriceAndPower();
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new PriceAndPower());
        }
        [Test]
        public void ConstructorTestWithParams()
        {
            Assert.DoesNotThrow(() => new PriceAndPower(12, 12, DateTime.Now));
        }
        [Test]
        public void PricePropertyTest()
        {
            double price = 12;
            pap.Price = price;
            Assert.AreEqual(pap.Price, price);
        }

        [Test]
        public void PowerPropertyTest()
        {
            double power = 25;
            pap.Power = power;
            Assert.AreEqual(pap.Power, power);
        }
        [Test]
        public void TimePropertyTest()
        {
            DateTime time = DateTime.Now;
            pap.Time = time;
            Assert.AreEqual(pap.Time, time);
        }
    }
}
