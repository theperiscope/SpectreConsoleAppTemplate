using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace SpectreConsoleAppTemplate.CLI
{
    internal interface IPublicHolidayAPI
    {
        [Get("/PublicHolidays/{Year}/{CountryCode}")]
        Task<IList<PublicHolidayModel>> Get(int year, string countryCode);
    }

    public class PublicHolidayModel
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("localName")]
        public string LocalName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("fixed")]
        public bool Fixed { get; set; }

        [JsonProperty("global")]
        public bool Global { get; set; }

        [JsonProperty("counties")]
        public List<string> Counties { get; set; }

        [JsonProperty("launchYear")]
        public int? LaunchYear { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}
