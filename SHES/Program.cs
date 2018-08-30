using Common.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SHES
{
    class Program
    {
        static void Main(string[] args)
        {

            var binding = new NetTcpBinding();

            ServiceHost svc = new ServiceHost(typeof(SHES));
            svc.Description.Name = "SHES";
            svc.AddServiceEndpoint(typeof(ISHESContract),
                                    binding,
                                    new Uri("net.tcp://localhost:5000/SHES"));

            svc.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            svc.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            svc.Open();
            Console.WriteLine("SHES servis ja otvoren");
            Console.ReadLine();
        }
    }
}
