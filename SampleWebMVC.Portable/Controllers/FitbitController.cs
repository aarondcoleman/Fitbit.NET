using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Models;

namespace SampleWebMVC.Portable.Controllers
{
    public class FitbitController : Controller
    {
        // GET: Fitbit
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Authorize/
        // Setup - prepare the user redirect to Fitbit.com to prompt them to authorize this app.
        public async Task<ActionResult> Authorize()
        {
            //make sure you've set these up in Web.Config under <appSettings>:
            string consumerKey = ConfigurationManager.AppSettings["FitbitConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["FitbitConsumerSecret"];

            var authenticator = new Authenticator(consumerKey, consumerSecret);
            RequestToken token = await authenticator.GetRequestTokenAsync();
            Session.Add("FitbitRequestTokenSecret", token.Secret); //store this somehow, like in Session as we'll need it after the Callback() action

            //note: at this point the RequestToken object only has the Token and Secret properties supplied. Verifier happens later.
            string authUrl = authenticator.GenerateAuthUrlFromRequestToken(token, true);

            return Redirect(authUrl);
        }

        //Final step. Take this authorization information and use it in the app
        public async Task<ActionResult> Callback()
        {
            RequestToken token = new RequestToken();
            token.Token = Request.Params["oauth_token"];
            token.Secret = Session["FitbitRequestTokenSecret"].ToString();
            token.Verifier = Request.Params["oauth_verifier"];

            string consumerKey = ConfigurationManager.AppSettings["FitbitConsumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["FitbitConsumerSecret"];

            //this is going to go back to Fitbit one last time (server to server) and get the user's permanent auth credentials

            //create the Authenticator object
            var authenticator = new Authenticator(consumerKey, consumerSecret);

            //execute the Authenticator request to Fitbit
            AuthCredential credential = await authenticator.ProcessApprovedAuthCallbackAsync(token);

            //here, we now have everything we need for the future to go back to Fitbit's API (STORE THESE):
            //  credential.AuthToken;
            //  credential.AuthTokenSecret;
            //  credential.UserId;

            // For demo, put this in the session managed by ASP.NET
            Session["FitbitAuthToken"] = credential.AuthToken;
            Session["FitbitAuthTokenSecret"] = credential.AuthTokenSecret;
            Session["FitbitUserId"] = credential.UserId;

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Devices()
        {
            var client = GetClient();
            var response = await client.GetDevicesAsync();
            return View(response);
        }

        public async Task<ActionResult> Friends()
        {
            var client = GetClient();
            var response = await client.GetFriendsAsync();
            return View(response);
        }

        public async Task<ActionResult> UserProfile()
        {
            var client = GetClient();
            var response = await client.GetUserProfileAsync();
            return View(response);
        }

        public async Task<ActionResult> LastWeekDistance()
        {
            var client = GetClient();
            var response = await client.GetTimeSeriesAsync(TimeSeriesResourceType.Distance, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
            return View("TimeSeriesDataList", response);
        }

        public async Task<ActionResult> LastWeekSteps()
        {
            var client = GetClient();
            var response = await client.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
            return View("TimeSeriesDataList", response);
        }

        private IFitbitClient GetClient()
        {
            return new FitbitClient(ConfigurationManager.AppSettings["FitbitConsumerKey"],
                                                              ConfigurationManager.AppSettings["FitbitConsumerSecret"],
                                                              Session["FitbitAuthToken"].ToString(),
                                                              Session["FitbitAuthTokenSecret"].ToString());
        }
    }
}