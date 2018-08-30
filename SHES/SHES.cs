using Common;
using Common.Contract;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    public class SHES : ISHESContract
    {
        public static DataIO serializer = new DataIO();

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

        public void SendSolarPanelPower(ObservableCollection<SolarPanel> solars)
        {
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("-----------------------------------------------------------------");
            foreach (SolarPanel item in solars)
            {
                Console.WriteLine("Solar {0} prodice {1} power (kw)", item.Name, item.CurentPower);
            }
        }
    }
}
