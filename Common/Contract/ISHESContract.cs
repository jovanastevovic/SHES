using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract
{
    [ServiceContract]
    public interface ISHESContract
    {
        [OperationContract]
        void SendSolarPanelPower(ObservableCollection<SolarPanel> solars);
        [OperationContract]
        void SendConsumerPower(ObservableCollection<Consumer> consumers);
        [OperationContract]
        void AddNewSolarPanelInSystem(SolarPanel sp);
        [OperationContract]
        void AddNewBatteryInSystem(Battery b);
        [OperationContract]
        void AddNewConsumerInSystem(Consumer e);
        [OperationContract]
        void SendUtilityPrice(double price);
        [OperationContract]
        void SendBatteryPower(ObservableCollection<Battery> batteries);
        [OperationContract]
        double PowerToSell();
        [OperationContract]
        ObservableCollection<Battery> GetBateriesCapacities();
    }
}
