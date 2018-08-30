using Common.Contract;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;

namespace SHES_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {


        public MainWindow()
        {
            InitializeComponent();
            var binding = new NetTcpBinding();
            DataContext = SHES.Instance;
            ServiceHost svc = new ServiceHost(typeof(SHES));
            svc.Description.Name = "SHES";
            svc.AddServiceEndpoint(typeof(ISHESContract),
                                    binding,
                                    new Uri("net.tcp://localhost:5000/SHES"));

            svc.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            svc.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            
            svc.Open();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
