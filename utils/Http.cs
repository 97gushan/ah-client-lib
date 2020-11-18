using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArrowHead.Utils
{
    public class Http
    {
        static readonly HttpClient client = new HttpClient();
        private string baseURI;

        public Http(string baseURI)
        {
            this.baseURI = baseURI;
        }

        public async Task<HttpResponseMessage> Get(string apiEndpoint)
        {
            HttpResponseMessage response = await client.GetAsync(this.baseURI + apiEndpoint);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> Post(string apiEndpoint, Object payload)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(this.baseURI + apiEndpoint, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
