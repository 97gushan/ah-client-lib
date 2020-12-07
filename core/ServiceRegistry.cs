using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Arrowhead.Models;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class ServiceRegistry
    {

        private Http http;
        private string baseUrl;

        public ServiceRegistry(Http http, Settings settings)
        {
            this.baseUrl = settings.getServiceRegistryUrl() + "/serviceregistry";
            this.http = http;
        }

        public object RegisterService(Service payload)
        {
            JObject tmp = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(payload));
            JObject providerSystem = (JObject)tmp.GetValue("providerSystem");
            tmp.Remove("id");
            providerSystem.Remove("id");
            tmp.Remove("providerSystem");
            tmp.Add("providerSystem", providerSystem);
            try
            {
                HttpResponseMessage resp = this.http.Post(this.baseUrl, "/register", tmp);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    if (UnregisterService(payload))
                    {
                        return (ServiceResponse)RegisterService(payload);
                    }
                    else
                    {
                        throw new Exception("Could not unregister existing service");
                    }
                }
                else
                {
                    resp.EnsureSuccessStatusCode();
                    return new ServiceResponse(JsonConvert.DeserializeObject<JObject>(respMessage));
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
            }
        }

        public bool UnregisterService(Service payload)
        {
            // setup query parameters
            string serviceDefinition = "service_definition=" + payload.serviceDefinition;
            string address = "address=" + payload.providerSystem.address;
            string port = "port=" + payload.providerSystem.port;
            string systemName = "system_name=" + payload.providerSystem.systemName;
            try
            {
                HttpResponseMessage resp = this.http.Delete(this.baseUrl, "/unregister?" + serviceDefinition + "&" + address + "&" + port + "&" + systemName);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                resp.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Returns a ServiceResponse with IDs of provider, interfaces and service definition of a wanted
        /// service, system and interface combination
        /// </summary>
        /// <remarks>
        /// NOTE This is a call to the Management endpoint of the Orchestrator, thus it requires a Sysop Certificate
        /// </remarks>
        /// <param name = "serviceDefinition" ></ param >
        /// <param name="providerSystem"></param>
        /// <param name="providerInterfaces"></param>
        /// <returns></returns>
        public ServiceResponse GetService(string serviceDefinition, JObject providerSystem, string[] providerInterfaces)
        {

            HttpResponseMessage resp = this.http.Get(this.baseUrl, "/mgmt/servicedef/" + serviceDefinition);
            resp.EnsureSuccessStatusCode();

            string respMessage = resp.Content.ReadAsStringAsync().Result;
            JObject respObject = JsonConvert.DeserializeObject<JObject>(respMessage);

            JArray services = (JArray)respObject.SelectToken("data");
            int length = (int)respObject.SelectToken("count");
            for (int i = 0; i < length; i++)
            {
                JObject service = (JObject)services[i];
                JObject system = (JObject)service.SelectToken("provider");
                JArray interfaces = (JArray)service.SelectToken("interfaces");

                if (EqualSystems(system, providerSystem) && EqualInterfaces(interfaces, providerInterfaces))
                {
                    return new ServiceResponse(service);
                }
            }

            return new ServiceResponse();
        }


        public object GetServices()
        {
            try
            {
                HttpResponseMessage resp = this.http.Get(this.baseUrl, "/mgmt?direction=ASC&sort_field=id");
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(respMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
            }
        }

        public object GetSystems()
        {
            try
            {
                HttpResponseMessage resp = this.http.Get(this.baseUrl, "/mgmt/systems?direction=ASC&sort_field=id");
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(respMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
            }
        }

        /// <summary>
        /// Check if 2 systems have the same name, address and port
        /// </summary>
        /// <param name="sys1"></param>
        /// <param name="sys2"></param>
        /// <returns></returns>
        private static bool EqualSystems(JObject sys1, JObject sys2)
        {
            return sys1.GetValue("systemName").ToString() == sys2.GetValue("systemName").ToString() &&
                   sys1.GetValue("address").ToString() == sys2.GetValue("address").ToString() &&
                   sys1.GetValue("port").ToString() == sys2.GetValue("port").ToString();
        }

        /// <summary>
        /// Check if 2 lists of interfaces share a interface name
        /// </summary>
        /// <param name="interfaces1"></param>
        /// <param name="interfaces2"></param>
        /// <returns></returns>
        private static bool EqualInterfaces(JArray interfaces1, string[] interfaces2)
        {
            for (int i = 0; i < interfaces1.Count; i++)
            {
                for (int j = 0; j < interfaces2.Length; j++)
                {
                    JObject i1 = (JObject)interfaces1[i];
                    if (i1.GetValue("interfaceName").ToString() == interfaces2[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public struct ServiceResponse
    {
        public string providerId, interfaceId, serviceDefinitionId;
        public ServiceResponse(JObject payload)
        {
            JObject provider = (JObject)payload.GetValue("provider");
            JArray interfaces = (JArray)payload.GetValue("interfaces");
            JObject serviceDefinition = (JObject)payload.GetValue("serviceDefinition");

            this.providerId = (string)provider.GetValue("id");
            this.interfaceId = (string)((JObject)interfaces[0]).GetValue("id");
            this.serviceDefinitionId = (string)serviceDefinition.GetValue("id");
        }

        public override string ToString()
        {
            return "providerId: " + providerId + "\ninterfaceId: " + interfaceId + "\nserviceDefinitionId: " + serviceDefinitionId;
        }
    }
}
