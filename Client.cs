using System;
using Arrowhead.Core;
using Arrowhead.Models;
using Newtonsoft.Json;

namespace Arrowhead
{
    public class Client
    {
        private Service service;
        private Arrowhead.Models.System system;


        public Client()
        {
            this.InitCoreSystems();

            this.system = new Arrowhead.Models.System("test-system", "https://127.0.0.1", "8080");
            this.service = new Service(this.system, "test-consumer", new string[] { "HTTPS-SECURE-JSON" });

            Console.WriteLine(ServiceRegistry.RegisterService(this.service));
        }

        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry("https://127.0.0.1", "8443");

        }
    }
}
