using Common.Contract;
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
namespace Consumer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Thread sendPowerToShes;
        private ObservableCollection<Common.Model.Consumer> consumers = new ObservableCollection<Common.Model.Consumer>();
        public static object lockObject = new object();
        public static ISHESContract proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
        new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();


        public ObservableCollection<Common.Model.Consumer> Consumers
        {
            get { return consumers; }
            set
            {
                consumers = value;
                OnPropertyChanged("Consumers");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            sendPowerToShes = new Thread(SendingPowerToSHES);
            sendPowerToShes.Start();
            Consumers =  proxy.GetConsumers();
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void addNewConsumer(object sender, RoutedEventArgs e)
        {
            Common.Model.Consumer consumer = new Common.Model.Consumer();
            foreach (Common.Model.Consumer item in Consumers)
            {
                if (item.Name == name.Text)
                {
                    MessageBox.Show("Name already exist");
                    return;
                }
            }
            try
            {
                consumer = new Common.Model.Consumer(name.Text, double.Parse(power.Text));
                proxy.AddNewConsumerInSystem(consumer);
                
            }
            catch (Exception)
            {
                Console.WriteLine("Bad power value");
                return;
            }
            lock (lockObject)
            {
                Consumers.Add(consumer);
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
                        if (Consumers.Count > 0)
                        {
                            proxy.SendConsumerPower(Consumers);
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
    }
}
