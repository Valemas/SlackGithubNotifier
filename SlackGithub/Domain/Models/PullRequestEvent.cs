using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Models
{
    public class PullRequestEvent
    {
        private const string HERE_NOTIFICATION = "<!here>";

        [JsonConverter(typeof(StringEnumConverter))]
        public PullRequestAction Action { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        public int Number { get; set; }

        public int PullRequestId => PullRequest.Id;
        public string UserName => PullRequest.UserName;

        public string CreatedText()
        {
            return
                $"{HERE_NOTIFICATION} <{PullRequest.Url}|#{Number} - {PullRequest.Title}> opened by {PullRequest.User.UserName}";
        }

        public string MergedText(string userName)
        {
            return $"<{PullRequest.Url}|#{Number} - {PullRequest.Title}> opened by {PullRequest.User.UserName} was merged by {userName}";
        }
    }
}