using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FrontDeskCheckinClient
{
    public static class ApiService
    {
        private static string uriBase = "http://localhost:9958/Api/";

        public static async Task AddVisitor(Visitor v)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = string.Format("{0}AddVisitor", uriBase);
                var content = new StringContent(JsonConvert.SerializeObject(v));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(url, content);
            }
        }

        public static async Task Checkout(Visitor v)
        {
            using (HttpClient client = new HttpClient())
            {
                v.DepartedAt = DateTime.Now;
                var url = string.Format("{0}CheckoutVisitor", uriBase);
                var content = new StringContent(JsonConvert.SerializeObject(v));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync(url, content);
            }
        }

        public static async Task<List<Visitor>> GetVisitors(string terminalKey)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = string.Format("{0}GetVisitors/{1}", uriBase, terminalKey);
                var result = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Visitor>>(result).OrderBy(x => x.ToString()).ToList();
            }
        }

        public static async Task<Terminal> GetTerminal(string terminalKey)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = string.Format("{0}GetTerminal/{1}", uriBase, terminalKey);
                var result = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Terminal>(result);
            }
        }
    }
}
