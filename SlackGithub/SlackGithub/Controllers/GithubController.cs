using SlackGithub.Models;

using System.Web.Http;

namespace SlackGithub.Controllers
{
    
    public class GithubController : ApiController
    {
        private PullRequestService _pullRequestService;

        public GithubController()
        {
            _pullRequestService = new PullRequestService(); 
        }

        [HttpPost, Route("github/submit")]
        public IHttpActionResult Submit([FromBody] PullRequestEvent pullRequestEvent)
        {
            if(pullRequestEvent == null)
                return BadRequest();

            if (_pullRequestService.SendMessage(pullRequestEvent))
                return Ok();


            return InternalServerError();
        }
    }
}
