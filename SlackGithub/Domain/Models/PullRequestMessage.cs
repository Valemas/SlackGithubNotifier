namespace Domain.Models
{
    public class PullRequestMessage
    {
        public int PullRequestId { get; set; }
        public string TimeStamp { get; set; }
        public string UserName { get; set; }

        public PullRequestMessage(int pullRequestId, string timeStamp, string userName)
        {
            PullRequestId = pullRequestId;
            TimeStamp = timeStamp;
            UserName = userName;
        }
    }
}