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
    public class ElementTest
    {
        private Element el;
        [OneTimeSetUp]
        public void SetupTest()
        {
            el = new Element();
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new Element());
        }
        [Test]
        public void ConstructorWithParamsTest()
        {
            Assert.DoesNotThrow(() => new Element("name",3));
        }

        [Test]
        public void NamePropertyTest()
        {
            string name = "name";
            el.Name = name;
            Assert.AreEqual(el.Name,name);
        }

        [Test]
        public void PowerPropertyTest()
        {
            double power = 25;
            el.Power = power;
            Assert.AreEqual(el.Power, power);
        }
    }
}
