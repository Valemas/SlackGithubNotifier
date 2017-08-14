namespace SlackGithub.Models
{
    public class PullRequest
    {
        public string Html_Url { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public bool Merged { get; set; }
    }
}