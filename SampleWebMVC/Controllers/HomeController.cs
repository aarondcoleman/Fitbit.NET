using System.Web.Mvc;

namespace SampleWebMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Session["FitbitAuthToken"] == null ||
                Session["FitbitAuthTokenSecret"] == null ||
                Session["FitbitUserId"] == null)
            {
                ViewBag.FitbitConnected = false;

            }
            else
            {
                ViewBag.FitbitConnected = true; // "Welcome Fitbit User " + Session["FitbitUserId"].ToString();
            }

            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
