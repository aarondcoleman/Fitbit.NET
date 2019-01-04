using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsole
{
    public class Options
    {
        public static string ClientId = System.Configuration.ConfigurationManager.AppSettings["FitbitConsumerKey"];
        public static string ClientSecret = System.Configuration.ConfigurationManager.AppSettings["FitbitConsumerSecret"];
        public static string[] AllScopes = new string[] { "activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight" };
    }
}
