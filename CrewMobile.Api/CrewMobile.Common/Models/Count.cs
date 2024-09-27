using Newtonsoft.Json;

namespace  CrewMobile.Common.Models
{
    public class Count
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public bool IsSpecial { get; set; }

        [JsonIgnore]
        public int Order { get; set; }
    }
}