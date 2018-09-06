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
    public class ConsumerTest
    {
        private Consumer c;
        [OneTimeSetUp]
        public void SetUp()
        {
            c = new Consumer();
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.DoesNotThrow(() => new Consumer());
        }
        [Test]
        public void ConstructorTestWithParams()
        {
            Assert.DoesNotThrow(() => new Consumer("name", 3));
        }

        [Test]
        public void IsActivePropertyTest()
        {
            bool isActive = true;
            c.IsActive = isActive;
            Assert.AreEqual(c.IsActive, isActive);
        }

    }
}
