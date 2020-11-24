using System;
using System.Net.Http;
using Newtonsoft.Json;
using Arrowhead.Models;
using Arrowhead.Utils;

namespace Arrowhead.Core
{
    public class ServiceRegistry
    {

        static Http http;

        private static string baseUrl;


        public static void InitServiceRegistry(string url, string port)
        {
            baseUrl = url + ":" + port + "/serviceregistry";
            http = new Http(baseUrl, "/home/user/Projects/arrowhead/core-java-spring/certificates/testcloud2/sysop.p12", false);
        }

        public static object RegisterService(Service payload)
        {

            Service existingService = ServiceRegistry.GetService(payload.serviceDefinition);

            // check if the service definition already exists in the Service Registry, 
            // if it does then unregister the old one before the new service can be registered 
            if (existingService != null)
            {
                ServiceRegistry.UnregisterService(payload);
            }

            try
            {
                HttpResponseMessage resp = http.Post("/mgmt", payload);
                resp.EnsureSuccessStatusCode();
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject(respMessage);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
            }
        }

        public static object UnregisterService(Service payload)
        {

            Service service = GetService(payload.serviceDefinition);
            if (service == null)
            {
                return null;
            }
            else
            {
                try
                {

                    HttpResponseMessage resp = http.Delete("/mgmt/" + service.id);
                    resp.EnsureSuccessStatusCode();
                    string respMessage = resp.Content.ReadAsStringAsync().Result;
                    return service;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);
                    return e;
                }
            }
        }

        public static Service GetService(string serviceDefinition)
        {
            try
            {
                HttpResponseMessage resp = http.Get("/mgmt/servicedef/" + serviceDefinition);
                string respMessage = resp.Content.ReadAsStringAsync().Result;
                Newtonsoft.Json.Linq.JObject tmp = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(respMessage);

                if (tmp.GetValue("count").ToObject<Int32>() > 0)
                {
                    Newtonsoft.Json.Linq.JToken json = tmp.GetValue("data")[0];
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
}
