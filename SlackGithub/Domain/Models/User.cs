using Newtonsoft.Json;

namespace Domain.Models
{
    public class User
    {
        [JsonProperty("login")]
        public string UserName { get; set; }
    }
}