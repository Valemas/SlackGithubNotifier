using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlackGithub.Models
{
    public class SlackAttachment
    {
        public string fallback { get; set; }
        public string color { get; set; }
        public IEnumerable<SlackAttachmentField> fields { get; set; }
        public string author_name => "";
        public string author_icon => "https://a.slack-edge.com/3429/plugins/github/assets/service_96.png";
    }
}