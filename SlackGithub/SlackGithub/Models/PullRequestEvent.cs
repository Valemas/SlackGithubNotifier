using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlackGithub.Models
{
    public class PullRequestEvent
    {
        public string Action { get; set; }
        public int Number { get; set; }
        public PullRequest PullRequest => Pull_Request;
        public PullRequest Pull_Request {get; set;}
        public bool Merged { get; set; }

        public int PullRequestId => PullRequest.Id;
    }
}