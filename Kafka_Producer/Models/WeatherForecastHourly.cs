using Newtonsoft.Json;
using System.Collections.Generic;

namespace Producer
{
    public class WeatherForecastHourly
    {
        [JsonProperty("dt")]
        public string DateTime { get; set; }
        [JsonProperty("temp")]
        public string Temp { get; set; }
        [JsonProperty("pressure")]
        public string Pressure { get; set; }
        [JsonProperty("wind_speed")]
        public string WindSpeed { get; set; }
        [JsonProperty("wind_deg")]
        public string WindDeg { get; set; }

    }
}