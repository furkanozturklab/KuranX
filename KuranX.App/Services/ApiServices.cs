using KuranX.App.Core.Classes.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace KuranX.App.Services
{
    public class ApiServices
    {


        public string api_token = App.config.AppSettings.Settings["api_token"].Value;
        public string api_server = App.config.AppSettings.Settings["api_adress"].Value;
        public string api_name = App.config.AppSettings.Settings["post_apiName"].Value;

        public async Task<List<Project>> projectGet(string id = "0")
        {
            try
            {
                // api üzerinden dosya listeleme işlemleri

                using (var client = new HttpClient())
                {
                    var postingdata = new Dictionary<string, string>
                    {
                        { "post_action", "SELECT_PROJECT" },
                        { "post_token", api_token},
                        { "post_apiName", api_name},
                        { "project_id", id }
                    };

                    var endpoint = new Uri(api_server);
                    var content = new FormUrlEncodedContent(postingdata);
                    var result = client.PostAsync(endpoint, content).Result;
                    string json = result.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<ApiProject>(json).Data;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return null;
            }


        }

        public async Task<List<UpdateNote>> updateGet(string id = "0", string newVersion = "0.0.0")
        {
            try
            {
                // api üzerinden dosya listeleme işlemleri

                using (var client = new HttpClient())
                {
                    var postingdata = new Dictionary<string, string>
                    {
                        { "post_action", "SELECT_PROJECT" },
                        { "post_token", api_token},
                        { "post_apiName", api_name},
                        { "project_id", id },
                        { "update_version", newVersion },
                    };

                    var endpoint = new Uri(api_server);
                    var content = new FormUrlEncodedContent(postingdata);
                    var result = client.PostAsync(endpoint, content).Result;
                    string json = result.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<ApiUpdateNote>(json).Data;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return null;
            }


        }

    }
}
