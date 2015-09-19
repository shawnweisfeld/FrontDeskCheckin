using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskCheckinClient
{
    public class ClientIdentiy
    {
        public string Key { get; set; }

        public async static Task<Terminal> Get()
        {
            var configFile = "client.config";
            string terminalKey = Guid.NewGuid().ToString();
            Terminal terminal = null;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(configFile))
                {
                    //Get the key from isolated storage
                    using (StreamReader sr = new StreamReader(store.OpenFile(configFile, FileMode.Open)))
                    {
                        terminalKey = JsonConvert.DeserializeObject<Terminal>(sr.ReadToEnd()).Key;
                    }
                }

                //Get the details from the service
                var url = string.Format("http://localhost:9958/Api/GetTerminal/{0}", terminalKey);

                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    terminal = JsonConvert.DeserializeObject<Terminal>(result);
                }

                //cache it to isolated storage
                using (StreamWriter sw = new StreamWriter(store.OpenFile(configFile, FileMode.Create)))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(terminal));
                }

            }

            return terminal;

        }
    }
}
