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

            this.system = new Arrowhead.Models.System("consumer", "https://127.0.0.1", "8080");
            this.service = new Service(this.system, "hello-consumer", new string[] { "HTTPS-SECURE-JSON" });
            Service providerService = ServiceRegistry.GetService("hello-producer");
            ServiceResponse serviceResp = ServiceRegistry.RegisterService(this.service);
            this.system.id = providerService.providerSystem.id;
            Console.WriteLine(Authorization.Authorize(serviceResp.consumerId, new string[] { providerService.providerSystem.id }, new string[] { serviceResp.interfaceId }, new string[] { serviceResp.serviceDefinitionId }));
            // Console.WriteLine(ServiceRegistry.GetServices());
            Console.WriteLine(Orchestrator.Orchestrate(this.system, "hello-producer"));
        }

        private void InitCoreSystems()
        {
            Settings settings = new Settings();
            ServiceRegistry.InitServiceRegistry(settings);
            Authorization.InitAuthorization(settings);
            Orchestrator.InitOrchestrator(settings);
        }
    }
}
