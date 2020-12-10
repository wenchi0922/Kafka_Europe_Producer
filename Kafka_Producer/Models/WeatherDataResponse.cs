using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Producer
{
    public class WeatherDataResponse
    {
        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
        [JsonProperty("hourly")]
        public List<WeatherForecastHourly> WeatherForecastHourly { get; set; }

    }
}
