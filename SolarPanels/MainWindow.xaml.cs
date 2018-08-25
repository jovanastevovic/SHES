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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolarPanels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Thread sunThread;
        public Thread sendPowerToSHESThread;
        private ObservableCollection<SolarPanel> solarPanels = new ObservableCollection<SolarPanel>();
        public static object lockObject = new object();
        public static object lockObjectSun = new object();
        public static double sunValue = 0;
        public static ISHESContract proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
         new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SolarPanel> SolarPanels
        {
            get { return solarPanels; }
            set
            {
                solarPanels = value;
                OnPropertyChanged("SolarPanels");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            sunThread = new Thread(SunPowerChange);
            sunThread.Start();

            sendPowerToSHESThread = new Thread(SendingPowerToSHES);
            sendPowerToSHESThread.Start();
        }

        private void dataGridSelectionChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addNewSolarPanel(object sender, RoutedEventArgs e)
        {
            SolarPanel sp = new SolarPanel();
            foreach (SolarPanel item in SolarPanels)
            {
                if (item.Name == name.Text)
                {
                    MessageBox.Show("Name already exist");
                    return;
                }
            }
            try
            {
                sp = new SolarPanel(name.Text, double.Parse(power.Text));
                proxy.AddNewSolarPanelInSystem(sp);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Bad power value");
                return;
            }
            lock (lockObject)
            {
                SolarPanels.Add(sp);
            }

        }
        private void SunPowerChange()
        {
            double newValue;
            double curentValue = 0;
            while (true)
            {
                try
                {
                    lock (lockObjectSun)
                    {
                        newValue = sunValue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Bad value");
                    Thread.Sleep(500);
                    continue;
                }
                lock (lockObject)
                {
                    foreach (SolarPanel item in SolarPanels)
                    {
                        item.CurentPower = item.Power * newValue / 100;
                    }
                    curentValue = newValue;
                }
                Thread.Sleep(1000);
            }
        }
        private void SetSunPower(object sender, RoutedEventArgs e)
        {
            try
            {
                lock (lockObjectSun)
                {
                    sunValue = double.Parse(sunPowerValue.Text);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Bad value");
                return;
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
                        if (SolarPanels.Count > 0)
                        {
                            proxy.SendSolarPanelPower(SolarPanels);
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

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


    }
}
