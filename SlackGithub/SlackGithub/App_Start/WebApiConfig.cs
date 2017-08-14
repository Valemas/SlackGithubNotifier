using System.Web.Configuration;
using System.Web.Http;

using Persistence.Azure.Adapter;

using SimpleInjector;

using SlackWebApiClient;

namespace SlackGithub.Webservice
{
    public static class WebApiConfig
    {
        private static readonly Container _container = new Container();
        private static PersistenceAzureAdapter _persistenceAdapter;

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            RegisterDependencies();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void RegisterDependencies()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings[0];
            _persistenceAdapter = new PersistenceAzureAdapter(connectionString.ConnectionString);
            
            _persistenceAdapter.Register(_container);

            var authKey = WebConfigurationManager.AppSettings.GetValues("SlackAuthKey");
            var slackApi = new SlackApi(authKey[0]);

            _container.RegisterSingleton(slackApi);

            _container.Register<IPullRequestService, PullRequestService>();
        }
    }
}
