using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PgsKanban.Dto
{
    public class IPGeoDto
    {
        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }
    }
}
