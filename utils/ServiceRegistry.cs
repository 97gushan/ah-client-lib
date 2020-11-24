using System;
using System.Net.Http;
using Newtonsoft.Json;
using Arrowhead.Core;

namespace Arrowhead.Utils
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
            try
            {
                HttpResponseMessage resp = http.Post("/mgmt", payload);
                return resp.StatusCode;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return e;
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
