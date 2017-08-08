using SlackGithub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlackGithub
{
    public static class SlackMessageBuilder
    {
        private const string LIGHT_BLUE = "#c7f9ea";

        public static SlackMessage BuildRichPrCreatedMessage(PullRequestEvent pullRequestEvent)
        {
            var text = BuildText(pullRequestEvent.PullRequest.html_url, pullRequestEvent.Number, pullRequestEvent.PullRequest.Title, pullRequestEvent.PullRequest.User.UserName);
            var field = BuildField(text);
            var attachment = BuildAttachment(text, field);
            return new SlackMessage { attachments = new List<SlackAttachment> { attachment } };
        }

        public static SlackMessage BuildNonRichPrCreatedMessage(PullRequestEvent pullRequestEvent)
        {
            var text = BuildText(pullRequestEvent.PullRequest.html_url, pullRequestEvent.Number, pullRequestEvent.PullRequest.Title, pullRequestEvent.PullRequest.User.UserName);
            
            return new SlackMessage { text = text };
        }

        private static SlackAttachmentField BuildField(string Text)
        {
            return new SlackAttachmentField { value = Text };
        }

        private static SlackAttachment BuildAttachment(string FallbackText, SlackAttachmentField field)
        {
            return new SlackAttachment { fallback = FallbackText, fields = new List<SlackAttachmentField> { field }, color = LIGHT_BLUE };
        }

        public static string BuildText(string pullRequestUrl, int pullRequestNumber, string pullRequestTitle, string user)
        {
            var formattedNumberAndTitle = FormatPrNumberAndTitle(pullRequestTitle, pullRequestNumber);
            return $"<!here> <{pullRequestUrl}|{formattedNumberAndTitle}> opened by {user}";
        }

        public static string FormatPrNumberAndTitle(string pullRequestTitle, int pullRequestNumber)
        {
            return $"#{pullRequestNumber} - {pullRequestTitle}";
        }
    }
}