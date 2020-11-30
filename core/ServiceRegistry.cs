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

        static Http http;

        public static void InitServiceRegistry(Settings settings)
        {
            string baseUrl = settings.getServiceRegistryUrl() + "/serviceregistry";
            http = new Http(baseUrl, settings.CertificatePath, settings.VerifyCertificate);
        }

        public static object RegisterService(Service payload)
        {
            JObject tmp = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(payload));
            JObject providerSystem = (JObject)tmp.GetValue("providerSystem");
            tmp.Remove("id");
            providerSystem.Remove("id");
            tmp.Remove("providerSystem");
            tmp.Add("providerSystem", providerSystem);

            try
            {
                HttpResponseMessage resp = http.Post("/register", tmp);
                string respMessage = resp.Content.ReadAsStringAsync().Result;

                if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    UnregisterService(payload);
                    return (ServiceResponse)RegisterService(payload);
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

        public static bool UnregisterService(Service payload)
        {
            // setup query parameters
            string serviceDefinition = "service_definition=" + payload.serviceDefinition;
            string address = "address=" + payload.providerSystem.address;
            string port = "port=" + payload.providerSystem.port;
            string systemName = "system_name=" + payload.providerSystem.systemName;
            try
            {
                HttpResponseMessage resp = http.Delete("/unregister?" + serviceDefinition + "&" + address + "&" + port + "&" + systemName);
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

        public static ServiceResponse GetService(string serviceDefinition, JObject providerSystem, string[] providerInterfaces)
        {
            try
            {
                HttpResponseMessage resp = http.Get("/mgmt/servicedef/" + serviceDefinition);
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
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return new ServiceResponse();
            }
        }


        public static object GetServices()
        {
            try
            {
                HttpResponseMessage resp = http.Get("/mgmt?direction=ASC&sort_field=id");
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(respMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
            }
        }

        public static object GetSystems()
        {
            try
            {
                HttpResponseMessage resp = http.Get("/mgmt/systems?direction=ASC&sort_field=id");
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
