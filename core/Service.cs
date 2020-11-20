using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

using ArrowHead.Utils;

namespace ArrowHead.Core
{
    public class Service
    {
        private string url;
        private Http http;

        public Service(string url)
        {
            this.url = url;
            this.http = new Http(url, "/home/user/Projects/arrowhead/core-java-spring/certificates/testcloud2/sysop.p12", false);
        }

        public async void GetServices()
        {
            try
            {
                HttpResponseMessage resp = await http.Get("/mgmt?direction=ASC&sort_field=id");
                string respMessage = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(JsonConvert.DeserializeObject(respMessage));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void GetSystems()
        {
            try
            {
                HttpResponseMessage resp = await http.Get("/mgmt/systems?direction=ASC&sort_field=id");
                string respMessage = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(JsonConvert.DeserializeObject(respMessage));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
