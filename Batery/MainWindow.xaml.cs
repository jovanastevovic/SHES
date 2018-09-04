using Common.Contract;
using Common.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Windows;

namespace Batery
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Thread sendPowerToSHESThread, changeBatteryCapacity;
        private static ObservableCollection<Battery> baterries = new ObservableCollection<Battery>();
        public static object lockObject = new object();
        public static ISHESContract proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
       new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();


        public ObservableCollection<Battery> Batteries
        {
            get { return baterries; }
            set
            {
                baterries = value;
                OnPropertyChanged("Batteries");
            }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            sendPowerToSHESThread = new Thread(SendingPowerToSHES);
            sendPowerToSHESThread.Start();

            changeBatteryCapacity = new Thread(ChangeBatteryCapacity);
            changeBatteryCapacity.Start();
        }

        private void addNewBattery(object sender, RoutedEventArgs e)
        {
            Battery battery = new Battery();
            lock (lockObject)
            {
                foreach (Battery item in Batteries)
                {
                    if (item.Name == name.Text)
                    {
                        MessageBox.Show("Name already exist");
                        return;
                    }
                }
            }
            try
            {
                battery = new Battery(name.Text, double.Parse(power.Text), double.Parse(capacity.Text));
                proxy.AddNewBatteryInSystem(battery);
            }
            catch (Exception)
            {
                Console.WriteLine("Bad power value");
                return;
            }
            lock (lockObject)
            {
                baterries.Add(battery);
            }
        }
        private void SendingPowerToSHES()
        {
            while (true)
            {
                try
                {
                    lock (lockObject)
                    {
                        if (Batteries.Count > 0)
                        {
                            proxy.SendBatteryPower(Batteries);
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("SHES is not avaiable");
                    proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
                            new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
        private void ChangeBatteryCapacity()
        {
            while (true)
            {
                try
                {
                    ObservableCollection<Battery> res = proxy.GetBateriesCapacities();
                    if (res != null)
                    {
                        lock (lockObject)
                        {
                            Batteries = res;
                 
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Bed request");
                    Thread.Sleep(1200);
                    continue;
                }
                Thread.Sleep(1200);
            }
        }
    }
}
