using System.Web.Http;

namespace WissAppWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Mapping mapping = new Mapping();
        }
    }
}
