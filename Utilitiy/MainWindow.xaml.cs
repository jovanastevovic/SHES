using Common.Contract;
using System;
using System.Collections.Generic;
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

namespace Utilitiy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public Thread sendPriceThread;
        public Thread sellPowerThread;
        public static ISHESContract proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();
        private string currentPowerSold;
        private string sumOfSoldPower;
        public static object lockObject = new object();
        public static double price = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public string CurrentPowerSold
        {
            get { return currentPowerSold; }
            set
            {
                currentPowerSold = value;
                OnPropertyChanged("CurrentPowerSold");
            }
        }
        public string SumOfSoldPower
        {
            get { return sumOfSoldPower; }
            set
            {
                sumOfSoldPower = value;
                OnPropertyChanged("SumOfSoldPower");
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            sendPriceThread = new Thread(SendPriceThread);
            sendPriceThread.Start();

            sellPowerThread = new Thread(SellPowerThread);
            sellPowerThread.Start();
        }
        private void SendPriceThread()
        {
            while (true)
            {
                try
                {
                    lock (lockObject)
                    {
                        proxy.SendUtilityPrice(price);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Bed request");
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
        private void SetPriceClick(object sender, RoutedEventArgs e)
        {
            lock (lockObject)
            {
                try
                {
                    price = double.Parse(priceTB.Text);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid price");
                }
            }
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void SellPowerThread()
        {
            while (true)
            {
                try
                {
                    double sellPower = proxy.PowerToSell();
                    CurrentPowerSold = sellPower.ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine("Bed connection");
                    Thread.Sleep(1200);
                }
                Thread.Sleep(1200);
            }
        }
    }
}
