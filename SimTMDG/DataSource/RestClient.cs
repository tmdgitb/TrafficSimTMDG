using Newtonsoft.Json;
using SimTMDG.Road;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.DataSource
{
    public class RestClient
    {
        static HttpClient client = new HttpClient();


        public RestClient()
        {
        }

        public void ParseTraffic(List<RoadSegment> segments)
        {
            CallApi(segments);
        }

        public async void CallApi(List<RoadSegment> segments)
        {
            List<WayTraffic> trafficList = null;
            trafficList = await RunAsync(); //.Wait();
            
            if (trafficList != null)
            {
                foreach (WayTraffic traffic in trafficList)
                {
                    var matchSegment = segments.FindAll(x => x.WayId == traffic.way_id);
                    Console.WriteLine("id: " + traffic.way_id);

                    foreach(RoadSegment segment in matchSegment)
                    {
                        segment.updateMaxSpeed(traffic.traffic);
                    }
                    
                }
            }
        }


        static async Task<List<WayTraffic>> RunAsync()
        {
            //client.BaseAddress = new Uri("http://localhost/testapi/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            try
            {
                List<WayTraffic> trafficList = null;


                trafficList = await GetTrafficAsync("http://localhost/testapi/api/trafficsource/");
                return trafficList;
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static async Task<List<WayTraffic>> GetTrafficAsync(string path)
        {
            List<WayTraffic> trafficList = null;

            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                trafficList = JsonConvert.DeserializeObject<List<WayTraffic>>(jsonString.Result);
            
            }

            return trafficList;
        }
    }
}
