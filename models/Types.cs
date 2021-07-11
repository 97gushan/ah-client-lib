using Newtonsoft.Json.Linq;

namespace Arrowhead.Models
{
    public struct ServiceResponse
    {
        public string ProviderId, InterfaceId, ServiceDefinitionId;
        public ServiceResponse(JObject payload)
        {
            JObject provider = (JObject)payload.GetValue("provider");
            JArray interfaces = (JArray)payload.GetValue("interfaces");
            JObject serviceDefinition = (JObject)payload.GetValue("serviceDefinition");

            this.ProviderId = (string)provider.GetValue("id");
            this.InterfaceId = (string)((JObject)interfaces[0]).GetValue("id");
            this.ServiceDefinitionId = (string)serviceDefinition.GetValue("id");
        }

        public override string ToString()
        {
            return "providerId: " + ProviderId + "\ninterfaceId: " + InterfaceId + "\nserviceDefinitionId: " + ServiceDefinitionId;
        }
    }

    public readonly struct OrchestratorResponse
    {
        public Service Service {get;}
        public System Provider {get;}
        public string ServiceUri {get;}
        public string Secure {get;}
        public string[] Interfaces {get;}

        public OrchestratorResponse(JObject payload) {
            JObject provider = (JObject)payload.GetValue("provider");
            this.Provider = System.Build(provider);

            JObject service = (JObject) payload.GetValue("service");
            this.Service = Service.Build(service);

            JArray interfaces = (JArray) payload.GetValue("interfaces");
            this.Interfaces = new string[1];
            for(int i = 0; i < interfaces.Count; i++)
            {
                this.Interfaces[i] = (string) interfaces[i].SelectToken("interfaceName");
            }
            
            this.ServiceUri = (string) payload.GetValue("serviceUri");
            this.Secure = (string) payload.GetValue("secure");
        }
    }
}
