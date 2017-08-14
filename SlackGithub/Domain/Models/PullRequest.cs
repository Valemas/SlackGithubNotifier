using Newtonsoft.Json;

namespace Domain.Models
{
    public class PullRequest
    {
        [JsonProperty("html_url")]
        public string Url { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public bool Merged { get; set; }

        public string UserName => User.UserName;
    }
}