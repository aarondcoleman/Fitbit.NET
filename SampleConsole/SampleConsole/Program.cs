using Fitbit.Api.Portable;
using Fitbit.Models;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.IO;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Authorize with FitBit
            FitbitClient fc = AuthorizationHelper.GetAuthorizedFitBitClient("activity ", "nutrition ", "heartrate ", "location ", "nutrition ", "profile ", "settings ", "sleep ", "social ", "weight");

            // Retrieve the weight information for today
            DateTime start = DateTime.Now;
            Colorizer.WriteLine("Processing [Yellow!{0}]...", start.ToShortDateString());

            var user = fc.GetUserProfileAsync().Result;
            Colorizer.WriteLine("found [Green!{0}].", user.FullName);

            var weight = fc.GetWeightAsync(start, DateRangePeriod.OneMonth).Result;
            Colorizer.WriteLine("found [Green!{0}] weight entries.", weight.Weights.Count);

            // Save the downloaded information to disk
            System.Xml.Serialization.XmlSerializer src = new System.Xml.Serialization.XmlSerializer(typeof(Weight));
            using (StreamWriter sw = new StreamWriter("weight.txt"))
            {
                src.Serialize(sw, weight);
            }

            Console.ReadKey();
        }
    }
}
