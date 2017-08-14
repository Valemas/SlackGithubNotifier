using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

using Domain.Models;

namespace SlackGithub.Webservice.Controllers
{
    
    public class GithubController : ApiController
    {
        private readonly IPullRequestService _pullRequestService;

        public GithubController(IPullRequestService pullRequestService)
        {
            _pullRequestService = pullRequestService;
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
