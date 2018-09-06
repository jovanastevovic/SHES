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
        bool SendSolarPanelPower(ObservableCollection<SolarPanel> solars);
        [OperationContract]
        bool SendConsumerPower(ObservableCollection<Consumer> consumers);
        [OperationContract]
        bool SendEVPower(EVehicle vehicle);
        [OperationContract]
        bool AddNewSolarPanelInSystem(SolarPanel sp);
        [OperationContract]
        bool AddNewBatteryInSystem(Battery b);
        [OperationContract]
        bool AddNewConsumerInSystem(Consumer e);
        [OperationContract]
        bool SendUtilityPrice(double price);
        [OperationContract]
        bool SendBatteryPower(ObservableCollection<Battery> batteries);
        [OperationContract]
        double PowerToSell();
        [OperationContract]
        ObservableCollection<Battery> GetBateriesCapacities();
        [OperationContract]
        ObservableCollection<SolarPanel> GetSolarPanels();
        [OperationContract]
        ObservableCollection<Consumer> GetConsumers();
    }
}
