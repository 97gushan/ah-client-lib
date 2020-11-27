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
                    return new ServiceResponse(respMessage);
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

        public static Service GetService(string serviceDefinition)
        {
            try
            {
                JObject payload = new JObject();
                payload.Add("serviceDefinitionRequirement", serviceDefinition);
                Console.WriteLine(payload);
                HttpResponseMessage resp = http.Post("/query", payload);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                Console.WriteLine(respMessage);
                JObject tmp = JsonConvert.DeserializeObject<JObject>(respMessage);

                if (tmp.GetValue("count").ToObject<Int32>() > 0)
                {
                    JToken json = tmp.GetValue("data")[0];
                    Service service = Service.Build(json);

                    return service;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return null;
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
    }

    public struct ServiceResponse
    {
        public string consumerId, interfaceId, serviceDefinitionId;
        public ServiceResponse(string payload)
        {
            JObject tmp = JsonConvert.DeserializeObject<JObject>(payload);
            JObject provider = (JObject)tmp.GetValue("provider");
            JArray interfaces = (JArray)tmp.GetValue("interfaces");
            JObject serviceDefinition = (JObject)tmp.GetValue("serviceDefinition");

            this.consumerId = (string)provider.GetValue("id");
            this.interfaceId = (string)((JObject)interfaces[0]).GetValue("id");
            this.serviceDefinitionId = (string)serviceDefinition.GetValue("id");
        }

        public override string ToString()
        {
            return "consumerId: " + consumerId + "\ninterfaceId: " + interfaceId + "\nserviceDefinitionId: " + serviceDefinitionId;
        }
    }
}
