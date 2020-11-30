using System;
using Arrowhead.Core;
using Arrowhead.Models;
using Arrowhead.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arrowhead
{
    public class Admin
    {
        public Settings settings;

        public Admin(Settings settings)
        {
            this.settings = settings;
            this.InitCoreSystems();

            JObject providerSystem = new JObject();
            providerSystem.Add("systemName", this.settings.SystemName);
            providerSystem.Add("address", this.settings.Ip);
            providerSystem.Add("port", this.settings.Port);

            JObject cloud = new JObject();
            cloud.Add("operator", this.settings.CloudOperator);
            cloud.Add("name", this.settings.CloudName);

            try
            {
                ServiceResponse resp = ServiceRegistry.GetService(this.settings.ServiceDefinition, providerSystem, this.settings.Interfaces);
                Authorization.Authorize(this.settings.ConsumerSystemId, new string[] { resp.providerId }, new string[] { resp.interfaceId }, new string[] { resp.serviceDefinitionId });
                Orchestrator.StoreOrchestrate(this.settings.ConsumerSystemId, this.settings.ServiceDefinition, this.settings.Interfaces[0], providerSystem, cloud);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void InitCoreSystems()
        {
            ServiceRegistry.InitServiceRegistry(this.settings);
            Authorization.InitAuthorization(this.settings);
            Orchestrator.InitOrchestrator(this.settings);
        }
    }
}
