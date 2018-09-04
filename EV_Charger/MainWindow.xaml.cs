using Common.Contract;
using Common.Model;
using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace EV_Charger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Thread drive_Charge_Thread, changeBatteryCapacity;
        public static EVehicle vehicle;
        public static object lockObject = new object();
        public static ISHESContract proxy = new ChannelFactory<ISHESContract>(new NetTcpBinding(),
       new EndpointAddress("net.tcp://localhost:5000/SHES")).CreateChannel();

    
        public MainWindow()
        {
            InitializeComponent();
            drive_Charge_Thread = new Thread(Drive_Charge_Thread);
            drive_Charge_Thread.Start();
        }

        private void Drive_Charge_Thread()
        {
            while (true)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if ((bool)driveCb.IsChecked)
                    {
                        chargeCB.IsEnabled = false;
                        lock (lockObject)
                        {
                            if (vehicle.RemainingCapacity > 0)
                            {
                                vehicle.RemainingCapacity -= 0.05;
                                evRemaining.Content = vehicle.RemainingCapacity.ToString();
                            }
                        }
                    }
                    else
                    {
                        chargeCB.IsEnabled = true;
                    }
                    if ((bool)chargeCB.IsChecked)
                    {
                        driveCb.IsEnabled = false;
                        lock (lockObject)
                        {
                            if (vehicle.RemainingCapacity <= vehicle.Power)
                            {
                                vehicle.RemainingCapacity += 0.05;
                                evRemaining.Content = vehicle.RemainingCapacity.ToString();
                            }
                        }
                    }
                    else
                    {
                        driveCb.IsEnabled = true;
                    }
                }));
                Thread.Sleep(1000);
            }
        }

        private void AddVehicle(object sender, RoutedEventArgs e)
        {
            evname.Content = name.Text;
            evpower.Content = power.Text;
            evRemaining.Content = power.Text;
            vehicle = new EVehicle(name.Text, double.Parse(power.Text));

            addEV.IsEnabled = false;
        }
    }
}
