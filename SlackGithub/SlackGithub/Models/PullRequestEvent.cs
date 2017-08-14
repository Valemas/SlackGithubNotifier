using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SlackGithub.Models
{
    public class PullRequestEvent
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PullRequestAction Action { get; set; }

        public int Number { get; set; }
        public PullRequest PullRequest => Pull_Request;
        public PullRequest Pull_Request { get; set; }
        public int PullRequestId => PullRequest.Id;
    }
}