namespace SlackGithub.Models
{
    public enum PullRequestAction
    {
        Opened,
        Closed,
        Reopened,
        //below are unused
        Edited,
        Assigned,
        Unassigned,
        review_requested,
        review_request_removed,
        Labeled,
        Unlabeled
    }
}