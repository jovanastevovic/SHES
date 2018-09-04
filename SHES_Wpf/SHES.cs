using Common;
using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SHES_Wpf
{
    public class SHES : ISHESContract,INotifyPropertyChanged
    {
        #region Fields
        private string price = "0";
        private string powerProducedSolar = "0";
        private string powerConsumersUse = "0";
        private string powerEVUse = "0";
        private string powerInBattery = "0";
        private string batteryCapacity = "0";
        private string sumProduce = "0";
        private string sumConsume = "0";
        public static DataIO serializer = new DataIO();
        private static SHES instance = null;
        public static object lockInstance = new object();
        public static object lockbateries = new object();
        public static object lockSolars = new object();
        public static object lockConsumers = new object();
        public static Thread calculatinThread;
        private static bool bateryIsCharging = false;

        #endregion
        #region Properties
        public static IBatteryContract proxy = new ChannelFactory<IBatteryContract>(new NetTcpBinding(),
       new EndpointAddress("net.tcp://localhost:6000/Batteries")).CreateChannel();
        public ObservableCollection<Battery> Batteries = new ObservableCollection<Battery>();
        public ObservableCollection<Consumer> Consumers = new ObservableCollection<Consumer>();
        public ObservableCollection<SolarPanel> SolarPanels = new ObservableCollection<SolarPanel>();

        public event PropertyChangedEventHandler PropertyChanged;

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
        public string PowerEVUse
        {
            get { return powerEVUse; }
            set
            {
                powerEVUse = value;
                OnPropertyChanged("PowerEVUse");
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
        public bool BatteryIsCharging
        {
            get { return bateryIsCharging; }
            set
            {
                bateryIsCharging = value;
                OnPropertyChanged("BatteryIsCharging");
            }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        #endregion
        public SHES()
        {
            try
            {
                SolarPanels = serializer.DeSerializeObject<ObservableCollection<SolarPanel>>("SolarPanels.xml");
                Consumers = serializer.DeSerializeObject<ObservableCollection<Consumer>>("Consumers.xml");
                Batteries = serializer.DeSerializeObject<ObservableCollection<Battery>>("Batteries.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
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
                    //veca potrosnja nego proizvodnja ///Dodati ono konrolu punjenja i praznjennja kao u zadatku
                    if (double.Parse(Instance.PowerConsumersUse) > double.Parse(Instance.PowerProducedSolar))
                    {
                        try
                        {
                            if (double.Parse(Instance.BatteryCapacity) > 0.15)
                                proxy.DischargeBatteries();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Bad connection");
                        }
                    }
                    else if (double.Parse(Instance.PowerConsumersUse) < double.Parse(Instance.PowerProducedSolar))
                    {
                        try
                        {
                            if (double.Parse(Instance.BatteryCapacity) < double.Parse(Instance.PowerInBattery) - 0.15)
                                proxy.ChargeBatteries();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Bad connection");
                        }
                    }
                }
                Thread.Sleep(1300);
            }
        }

        public void AddNewBatteryInSystem(Battery b)
        {
            lock (lockbateries)
            {
                Batteries.Add(b);
                serializer.SerializeObject<ObservableCollection<Battery>>(Batteries, "Batteries.xml");
            }
        }

        public void AddNewConsumerInSystem(Consumer c)
        {
            lock (lockConsumers)
            {
                Consumers.Add(c);
                serializer.SerializeObject<ObservableCollection<Consumer>>(Consumers, "Consumers.xml");
            }
        }

        public void AddNewSolarPanelInSystem(SolarPanel sp)
        {
            lock (lockSolars)
            {
                SolarPanels.Add(sp);
                serializer.SerializeObject<ObservableCollection<SolarPanel>>(SolarPanels, "SolarPanels.xml");
            }
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
            double remainingCapacity = 0;
            foreach (Battery item in batteries)
            {
                powerInBattery += item.Power;
                remainingCapacity += item.RemainingCapacity;
            }
            lock (lockInstance)
            {
                Instance.BatteryCapacity = remainingCapacity.ToString();
                Instance.PowerInBattery = powerInBattery.ToString();
                Instance.BatteryIsCharging = batteries.FirstOrDefault().IsCharging;
            }
        }

        public double PowerToSell()
        {
            double result = 0;
            lock (lockInstance)
            {
                if (Instance.BatteryIsCharging)
                {
                    Instance.SumProduced = Instance.PowerProducedSolar;
                    Instance.SumConsume = (double.Parse(Instance.PowerConsumersUse) + double.Parse(Instance.PowerInBattery)+ double.Parse(
                        Instance.PowerEVUse)).ToString();
                }
                else
                {
                    if (double.Parse(Instance.BatteryCapacity) >= double.Parse(Instance.PowerInBattery) - 0.15)
                    {
                        Instance.SumProduced = Instance.PowerProducedSolar;
                        Instance.SumConsume = (double.Parse(Instance.PowerConsumersUse) + double.Parse(Instance.PowerEVUse)).ToString();
                    }
                    else
                    {
                        Instance.SumProduced = (double.Parse(Instance.PowerProducedSolar) + double.Parse(Instance.PowerInBattery)).ToString();
                        Instance.SumConsume = (double.Parse(Instance.PowerConsumersUse) + double.Parse(Instance.PowerEVUse)).ToString();
                    }
                }
                result = (double.Parse(Instance.SumConsume) - double.Parse(Instance.SumProduced));
            }
            //Thread.Sleep(300);
            return result;
        }
        public ObservableCollection<Battery> GetBateriesCapacities()
        {
            lock (lockbateries)
            {
                return Batteries;
            }
        }

        public ObservableCollection<SolarPanel> GetSolarPanels()
        {
            lock (lockSolars)
            {
                return SolarPanels;
            }
        }

        public ObservableCollection<Consumer> GetConsumers()
        {
            lock (lockConsumers)
            {
                return Consumers;
            }
        }

        public void SendEVPower(EVehicle vehicle)
        {
            double power = 0;
            power = vehicle.Power;
            lock (lockInstance)
            {
                Instance.PowerEVUse = power.ToString();
            }
        }
    }
}
