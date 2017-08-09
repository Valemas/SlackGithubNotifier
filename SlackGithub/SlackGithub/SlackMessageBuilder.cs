namespace SlackGithub
{
    public static class SlackMessageBuilder
    {
        public static string BuildText(string pullRequestUrl,
                                       int pullRequestNumber,
                                       string pullRequestTitle,
                                       string user)
        {
            return $"<!here> <{pullRequestUrl}|#{pullRequestNumber} - {pullRequestTitle}> opened by {user}";
        }
    }
}