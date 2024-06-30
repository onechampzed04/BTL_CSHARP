using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_2.Model
{
    public class Province
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name_with_type")]
        public string NameWithType { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("quan-huyen")]
        public Dictionary<string, District> Districts { get; set; }

        public override string ToString()
        {
            return NameWithType;
        }
    }
}
