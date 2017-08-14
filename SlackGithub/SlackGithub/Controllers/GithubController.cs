using System.Threading.Tasks;

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
        public async Task<IHttpActionResult> Submit([FromBody] PullRequestEvent pullRequestEvent)
        {
            if(pullRequestEvent == null)
                return BadRequest();

            if (await _pullRequestService.SendMessage(pullRequestEvent))
                return Ok();


            return InternalServerError();
        }
    }
}
