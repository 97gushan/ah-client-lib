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

        private Settings settings;

        public Client()
        {
            this.settings = new Settings();

            this.InitCoreSystems();

            this.system = new Arrowhead.Models.System("test_consumer", "https://127.0.0.1", "8080", this.settings.getSSL());
            this.service = new Service(this.system, "hello-consumer", new string[] { "HTTPS-SECURE-JSON" });
            ServiceResponse serviceResp = ServiceRegistry.RegisterService(this.service);

            // Service providerService = ServiceRegistry.GetService("hello-producer");
            // this.system.id = providerService.providerSystem.id;
            // Console.WriteLine(Authorization.Authorize(serviceResp.consumerId, new string[] { providerService.providerSystem.id }, new string[] { serviceResp.interfaceId }, new string[] { serviceResp.serviceDefinitionId }));
            // // Console.WriteLine(ServiceRegistry.GetServices());
            // Console.WriteLine(Orchestrator.Orchestrate(this.system, "hello-producer"));
        }

        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry(this.settings);
            Authorization.InitAuthorization(this.settings);
            Orchestrator.InitOrchestrator(this.settings);
        }
    }
}
