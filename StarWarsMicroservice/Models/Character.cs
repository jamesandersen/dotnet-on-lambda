using Newtonsoft.Json;

namespace StarWarsMicroservice.Models
{
    public class Character
    {
        public string Name { get; set; }
        public string Url { get; set; }

        [JsonProperty("eye_color")]
        public string EyeColor { get; set; }

        [JsonProperty("birth_year")]
        public string BirthYear { get; set; }
    }
}