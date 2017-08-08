using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlackGithub.Models
{
    public class SlackMessage
    {
        public IEnumerable<SlackAttachment> attachments { get; set; }
        public string text { get; set; }
    }
}