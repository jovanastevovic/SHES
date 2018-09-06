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
    public class SolarPanelsTest
    {
        private SolarPanel sp;
        [OneTimeSetUp]
        public void SetUp()
        {
            sp = new SolarPanel();
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new SolarPanel());
        }
        [Test]
        public void ConstructorTestWithParams()
        {
            Assert.DoesNotThrow(() => new SolarPanel("name", 3));
        }
        [Test]
        public void CurrentPowerPropertyTest()
        {
            double currentPower = 20;
            sp.CurentPower = currentPower;
            Assert.AreEqual(sp.CurentPower, currentPower);
        }
    }
}
