using System;
using Arrowhead.Core;
using Arrowhead.Models;
using Arrowhead.Utils;

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
            Settings settings = new Settings();
            ServiceRegistry.InitServiceRegistry(settings);
            Orchestrator.InitOrchestrator(settings);
        }
    }
}
