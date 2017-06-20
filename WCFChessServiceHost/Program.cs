using System;
using System.ServiceModel;
using WCFChessService;
using System.ServiceModel.Description;

namespace WCFChessServiceHost
{
    class Program
    {
        static void Main(string[] args)
        { 
            
            using (var serviceHost = new ServiceHost(typeof(WCFChessServiceLib)))
            {
                serviceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
                serviceHost.Description.Behaviors.Add(
                    new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
                serviceHost.Open();
                
                Console.WriteLine("Service is open");
                Console.WriteLine("Press enter to terminate service");
                Console.ReadLine();
            }

        }

        
    }
}
