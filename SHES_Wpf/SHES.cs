using Common;
using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHES_Wpf
{
    public class SHES : ISHESContract, INotifyPropertyChanged
    {
        private string price = "0";
        private string powerProducedSolar = "0";
        private string powerConsumersUse = "0";
        private string powerInBattery = "0";
        private string batteryCapacity = "0";
        private string sumProduce = "0";
        private string sumConsume = "0";
        public event PropertyChangedEventHandler PropertyChanged;
        public static DataIO serializer = new DataIO();
        private static SHES instance = null;
        public static object lockInstance = new object();
        public static object lockbateries = new object();
        public static Thread calculatinThread;
        public static bool BateryIsCharging = false;

        public ObservableCollection<Battery> Bateries
        {
            get; set;
        }

        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        public string PowerProducedSolar
        {
            get { return powerProducedSolar; }
            set
            {
                powerProducedSolar = value;
                OnPropertyChanged("PowerProducedSolar");
            }
        }
        public string PowerConsumersUse
        {
            get { return powerConsumersUse; }
            set
            {
                powerConsumersUse = value;
                OnPropertyChanged("PowerConsumersUse");
            }
        }
        public string PowerInBattery
        {
            get { return powerInBattery; }
            set
            {
                powerInBattery = value;
                OnPropertyChanged("PowerInBattery");
            }
        }
        public string BatteryCapacity
        {
            get { return batteryCapacity; }
            set
            {
                batteryCapacity = value;
                OnPropertyChanged("BatteryCapacity");
            }
        }
        public string SumProduced
        {
            get { return sumProduce; }
            set
            {
                sumProduce = value;
                OnPropertyChanged("SumProduced");
            }
        }
        public string SumConsume
        {
            get { return sumConsume; }
            set
            {
                sumConsume = value;
                OnPropertyChanged("SumConsume");
            }
        }
        public SHES()
        {

        }
        public static SHES Instance
        {
            get
            {
                lock (lockInstance)
                {
                    if (instance == null)
                    {
                        instance = new SHES();
                        calculatinThread = new Thread(CalculationThread);
                        calculatinThread.Start();
                    }
                    return instance;
                }
            }
        }
        private static void CalculationThread()
        {
            while (true)
            {
                lock (lockInstance)
                {
                    if (double.Parse(Instance.PowerConsumersUse) > double.Parse(Instance.PowerProducedSolar))
                    {

                    }
                }
            }
        }

        public void AddNewBatteryInSystem(Battery b)
        {
            serializer.SerializeObject<Battery>(b, "Batteries.xml");
        }

        public void AddNewConsumerInSystem(Consumer c)
        {
            serializer.SerializeObject<Consumer>(c, "Consumers.xml");
        }

        public void AddNewSolarPanelInSystem(SolarPanel sp)
        {
            serializer.SerializeObject<SolarPanel>(sp, "SolarPanels.xml");
        }

        public void SendConsumerPower(ObservableCollection<Consumer> consumers)
        {
            double power = 0;
            foreach (Consumer item in consumers)
            {
                power += item.Power;
            }
            lock (lockInstance)
            {
                Instance.PowerConsumersUse = power.ToString();
            }
        }

        public void SendSolarPanelPower(ObservableCollection<SolarPanel> solars)
        {

            double power = 0;
            foreach (SolarPanel item in solars)
            {
                power += item.CurentPower;
            }
            lock (lockInstance)
            {
                Instance.PowerProducedSolar = power.ToString();
            }

        }

        public void SendUtilityPrice(double price)
        {
            lock (lockInstance)
            {
                Instance.Price = price.ToString();
            }
        }

        public void SendBatteryPower(ObservableCollection<Battery> batteries)
        {
            double powerInBattery = 0;
            double capacity = 0;
            foreach (Battery item in batteries)
            {
                powerInBattery += item.Power;
                capacity += item.Capacity;
            }
            lock (lockInstance)
            {
                Instance.BatteryCapacity = capacity.ToString();
                Instance.PowerInBattery = powerInBattery.ToString();
            }
        }

        public double PowerToSell()
        {
            double result = 0;
            lock (lockInstance)
            {
                if (BateryIsCharging)
                {
                    Instance.SumProduced = Instance.PowerProducedSolar;
                    Instance.SumConsume = (double.Parse(Instance.PowerConsumersUse) + double.Parse(Instance.PowerInBattery)).ToString();
                }
                else
                {
                    Instance.SumProduced = (double.Parse(Instance.PowerProducedSolar) + double.Parse(Instance.PowerInBattery)).ToString();
                    Instance.SumConsume = Instance.PowerConsumersUse;
                }
                result = (double.Parse(Instance.SumConsume) - double.Parse(Instance.SumProduced));// ovde menjati kapacitet baterija
            }
            return result;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<Battery> GetBateriesCapacities()
        {
            lock (lockbateries)
            {
                return Bateries;
            }
        }
    }
}
