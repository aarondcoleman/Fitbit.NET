using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AsyncOAuth;

namespace SampleWebMVC.Portable
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            OAuthUtility.ComputeHash = (key, buffer) => { using (var hmac = new HMACSHA1(key)) { return hmac.ComputeHash(buffer); } };

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
