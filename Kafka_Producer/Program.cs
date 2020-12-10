using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "myBooStrapServer",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "myUserName",
                SaslPassword = "myPassword"

            };
            using (var producer = new ProducerBuilder<string, string>(producerConfig).Build())
            {
                while (true)
                {
                    //this is the kafka topic we want to produce messages to
                    var topic = "Weather_72hr";
                    var weatherData = await GetWeatherData();
                    foreach(var r in weatherData)
                    {
                        foreach (var i in r.WeatherForecastHourly)
                        {
                            var time = Convert.ToInt64(i.DateTime);
                            var id = Guid.NewGuid();
                            var valueString = "{ \"DateTime\": \"" + DateTimeOffset.FromUnixTimeSeconds(time) + "\", \"Location\": \"" + r.TimeZone + "\", \"WindSpeed\": \"" + i.WindSpeed + "\",  \"WindDeg\": " + i.WindDeg + ",  \"Temp\": " + i.Temp + ",  \"Pressure\": " + i.Pressure + " }";
                            await producer.ProduceAsync(topic, new Message<string, string> { Key = i.DateTime, Value = valueString });

                            Console.WriteLine(valueString);
                        }
                    }


                    Thread.Sleep(60000);
                }

            }
        }
        public static async Task<List<WeatherDataResponse>> GetWeatherData()
        {
            var weatherDataResponses = new List<WeatherDataResponse>();
            //Europe Wind Speed Forecast
            var urls = new List<string>
            {
                //Belgium/Brussel
                "https://api.openweathermap.org/data/2.5/onecall?lat=50.8503&lon=4.3517&appid={myApiKey}",
                //Italy
                "https://api.openweathermap.org/data/2.5/onecall?lat=41.8719&lon=12.5674&appid={myApiKey}",
                //Netherlands
                "https://api.openweathermap.org/data/2.5/onecall?lat=52.1326&lon=5.2913&appid={myApiKey}",
                //Germany
                "https://api.openweathermap.org/data/2.5/onecall?lat=51.1657&lon=10.4515&appid={myApiKey}",
                //Austria
                "https://api.openweathermap.org/data/2.5/onecall?lat=47.5162&lon=14.5501&appid={myApiKey}",
                //France
                "https://api.openweathermap.org/data/2.5/onecall?lat=46.2276&lon=2.2137&appid={myApiKey}"

            };
            foreach(var url in urls)
            {
                var httpClient = new HttpClient();
                var res = await httpClient.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    var weatherDataResponse = JsonConvert.DeserializeObject<WeatherDataResponse>(json);
                    weatherDataResponses.Add(weatherDataResponse);
                }
            }
            return weatherDataResponses;

        }
    }
}
