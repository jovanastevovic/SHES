using Common.Contract;
using Common.Model;
using NSubstitute;
using NUnit.Framework;
using SHES_Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES_WPFTests
{
    [TestFixture]
    public class SHESTests
    {
        private SHES shes;
        ISHESContract proxy = Substitute.For<ISHESContract>();

        [OneTimeSetUp]
        public void SetUpTest()
        {
            shes = new SHES();
            SHES.InstanceProxy = Substitute.For<ISHESContract>();
            SHES.InstanceProxy.SendEVPower(new EVehicle("ev1", 1)).Returns(true);
            SHES.InstanceProxy.SendEVPower(new EVehicle("ev1", -1)).Returns(false);
            SHES.InstanceProxy.AddNewSolarPanelInSystem(new SolarPanel("sp12", 3)).Returns(true);
            SHES.InstanceProxy.AddNewSolarPanelInSystem(new SolarPanel("sp12", -3)).Returns(false);
            SHES.InstanceProxy.AddNewBatteryInSystem(new Battery("b12", 3, 3)).Returns(true);
            SHES.InstanceProxy.AddNewBatteryInSystem(new Battery("b12", -3, -3)).Returns(false);
            SHES.InstanceProxy.AddNewConsumerInSystem(new Consumer("c12", 3)).Returns(true);
            SHES.InstanceProxy.AddNewConsumerInSystem(new Consumer("c12", -3)).Returns(false);

            //loadBalancer = new LoadBalancing();
            //LoadBalancing.Instance = Substitute.For<ILoadBalancerContract>();
            //LoadBalancing.Instance.Alive("").Returns(false);
            //LoadBalancing.Instance.RequestForTurnOnOff(true, "w1").Returns(true);
            //LoadBalancing.Instance.RequestForTurnOnOff(false, "w1").Returns(true);
            //LoadBalancing.Instance.RequestForTurnOnOff(true, "w2").Returns(false);
        }

        #region Propery test
        [Test]
        public void PriceProperyTest()
        {
            string price = "23";
            shes.Price = price;
            Assert.AreEqual(shes.Price, price);
        }
        [Test]
        public void PowerProducedSolarProperyTest()
        {
            string pps = "23";
            shes.PowerProducedSolar = pps;
            Assert.AreEqual(shes.PowerProducedSolar, pps);
        }
        [Test]
        public void PowerConsumerUseProperyTest()
        {
            string pcu = "23";
            shes.PowerConsumersUse = pcu;
            Assert.AreEqual(shes.PowerConsumersUse, pcu);
        }
        [Test]
        public void PowerEVUseProperyTest()
        {
            string pevu = "23";
            shes.PowerEVUse = pevu;
            Assert.AreEqual(shes.PowerEVUse, pevu);
        }
        [Test]
        public void PowerInBatteryProperyTest()
        {
            string pib = "23";
            shes.PowerInBattery = pib;
            Assert.AreEqual(shes.PowerEVUse, pib);
        }
        [Test]
        public void BatteryCapacityProperyTest()
        {
            string bc = "23";
            shes.BatteryCapacity = bc;
            Assert.AreEqual(shes.BatteryCapacity, bc);
        }
        [Test]
        public void SumProducedProperyTest()
        {
            string sp = "23";
            shes.SumProduced = sp;
            Assert.AreEqual(shes.SumProduced, sp);
        }
        [Test]
        public void SumConsumeProperyTest()
        {
            string sc = "23";
            shes.SumConsume = sc;
            Assert.AreEqual(shes.SumConsume, sc);
        }
        [Test]
        public void BatteryIsChargingProperyTest()
        {
            bool bic = true;
            shes.BatteryIsCharging = bic;
            Assert.AreEqual(shes.BatteryIsCharging, bic);
        }
        [Test]
        public void BatteryIsDisChargingProperyTest()
        {
            bool bidc = true;
            shes.BatteryIsDisCharging = bidc;
            Assert.AreEqual(shes.BatteryIsDisCharging, bidc);
        }


        #endregion
        [Test]
        public void EVehicleReturnTrue()
        {
            bool result = shes.SendEVPower(new EVehicle("ev1", 1));
            Assert.IsTrue(result);
        }
        [Test]
        public void EVehicleReturnFalse()
        {
            bool result = SHES.InstanceProxy.SendEVPower(new EVehicle("ev1", -1));
            Assert.IsFalse(result);
        }
        [Test]
        public void AddNewSolarPanelReturnTrue()
        {
            bool result = shes.AddNewSolarPanelInSystem(new SolarPanel("sp12", 3));
            Assert.IsTrue(result);
        }
        [Test]
        public void AddNewSolarPanelReturnFalse()
        {
            bool result = SHES.InstanceProxy.SendEVPower(new EVehicle("sp12", -3));
            Assert.IsFalse(result);
        }
        [Test]
        public void AddNewBatteryReturnTrue()
        {
            bool result = shes.AddNewBatteryInSystem(new Battery("b12", 3, 3));
            Assert.IsTrue(result);
        }
        [Test]
        public void AddNewBatteryReturnFalse()
        {
            bool result = SHES.InstanceProxy.AddNewBatteryInSystem(new Battery("b12", -3, -3));
            Assert.IsFalse(result);
        }
        [Test]
        public void AddNewConsumerReturnTrue()
        {
            bool result = shes.AddNewConsumerInSystem(new Consumer("c12", 3));
            Assert.IsTrue(result);
        }
        [Test]
        public void AddNewConsumerReturnFalse()
        {
            bool result = SHES.InstanceProxy.AddNewConsumerInSystem(new Consumer("c12", 3));
            Assert.IsFalse(result);
        }

        [Test]
        public void PowerToSellBatteryChargingTrueTest()
        {
            SHES.Instance.PowerConsumersUse = "14";
            SHES.Instance.PowerProducedSolar = "10";
            SHES.Instance.BatteryCapacity = "2";
            SHES.Instance.PowerInBattery = "2";
            SHES.Instance.BatteryIsCharging = true;
            double res = SHES.Instance.PowerToSell();
            Assert.AreEqual(res, 7.0);
        }
        [Test]
        public void PowerToSellBatteryChargingFalseTest()
        {
            SHES.Instance.PowerConsumersUse = "12";
            SHES.Instance.PowerProducedSolar = "10";
            SHES.Instance.BatteryCapacity = "2";
            SHES.Instance.PowerInBattery = "2";
            SHES.Instance.BatteryIsCharging = false;
            double res = SHES.Instance.PowerToSell();
            Assert.AreEqual(res, 3.0);
        }
        [Test]
        public void PowerToSellBatteryChargingFalse1Test()
        {
            SHES.Instance.PowerConsumersUse = "12";
            SHES.Instance.PowerProducedSolar = "10";
            SHES.Instance.BatteryCapacity = "2";
            SHES.Instance.PowerInBattery = "3";
            SHES.Instance.BatteryIsCharging = false;
            double res = SHES.Instance.PowerToSell();
            Assert.AreEqual(res, 0.0);
        }

        [Test]
        public void SendSolarPowerTest()
        {
            ObservableCollection<SolarPanel> sp = new ObservableCollection<SolarPanel>();
            SolarPanel solar = new SolarPanel("Sp23", 5);
            solar.CurentPower = 5;
            sp.Add(solar);
            SHES.Instance.SendSolarPanelPower(sp);
            Assert.AreEqual(SHES.Instance.PowerProducedSolar, "5");
        }
        [Test]
        public void SendUtilityPriceTest()
        {
            double price = 4.5;
            SHES.Instance.SendUtilityPrice(price);
            Assert.AreEqual(SHES.Instance.Price, "4.5");
        }
        [Test]
        public void SendConsumerPowerTest()
        {
            ObservableCollection<Consumer> consumers = new ObservableCollection<Consumer>();
            Consumer cons = new Consumer("C23", 5);
            consumers.Add(cons);
            SHES.Instance.SendConsumerPower(consumers);
            Assert.AreEqual(SHES.Instance.PowerConsumersUse, "5");
        }
        [Test]
        public void SendSendBatteryPowerTest()
        {
            ObservableCollection<Battery> batteries = new ObservableCollection<Battery>();
            Battery bat = new Battery("b23", 5, 5);
            batteries.Add(bat);
            SHES.Instance.SendBatteryPower(batteries);
            Assert.AreEqual(SHES.Instance.PowerInBattery, "5");
        }
        [Test]
        public void GetConsumersTest()
        {
            ObservableCollection<Consumer> consumers = new ObservableCollection<Consumer>()
            {
                new Consumer("C1",4),
                new Consumer("C2",5)
            };
            SHES.Instance.Consumers = consumers;
            var res = SHES.Instance.GetConsumers();
            Assert.AreEqual(res, SHES.Instance.Consumers);
        }
        [Test]
        public void GetSolarPanelsTest()
        {
            ObservableCollection<SolarPanel> solarpanels = new ObservableCollection<SolarPanel>()
            {
                new SolarPanel("Sp1",4),
                new SolarPanel("Sp2",5)
            };
            SHES.Instance.SolarPanels = solarpanels;
            var res = SHES.Instance.GetSolarPanels();
            Assert.AreEqual(res, SHES.Instance.SolarPanels);
        }
        [Test]
        public void GetBatteriesCapacityChargingTest()
        {
            ObservableCollection<Battery> batteries = new ObservableCollection<Battery>();

            Battery b1 = new Battery("B1", 4, 3);
            Battery b2 = new Battery("B2", 5, 5);
            b1.IsCharging = true;
            batteries.Add(b1);
            batteries.Add(b2);
            SHES.Instance.Batteries = batteries;
            ObservableCollection<Battery> res = SHES.Instance.GetBateriesCapacities();
            Assert.AreEqual(res[0].RemainingCapacity,3.05);
        }
        [Test]
        public void GetBatteriesCapacityDisChargingTest()
        {
            ObservableCollection<Battery> batteries = new ObservableCollection<Battery>();

            Battery b1 = new Battery("B1", 4, 4);
            Battery b2 = new Battery("B2", 5, 5);
            b1.IsDisCharging = true;
            batteries.Add(b1);
            batteries.Add(b2);
            SHES.Instance.Batteries = batteries;
            ObservableCollection<Battery> res = SHES.Instance.GetBateriesCapacities();
            Assert.AreEqual(res[0].RemainingCapacity, 3.95);
        }
    }
}
