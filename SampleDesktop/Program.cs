using Fitbit.Api;
using Fitbit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SampleDesktop
{
    class Program
    {
        const string consumerKey = "YOUR_CONSUMER_KEY_HERE";
        const string consumerSecret = "YOUR_CONSUMER_SECRET_HERE";
        
        static void Main(string[] args)
        {
            //Example of getting the Auth credentials for the first time by directoring the
            //user to the fitbit site to get a PIN. 

            var credentials = LoadCredentials();

            if (credentials == null)
            {
                credentials = Authenticate();
                SaveCredentials(credentials);
            }

            var fitbit = new FitbitClient(consumerKey, consumerSecret, credentials.AuthToken, credentials.AuthTokenSecret);
           
            var profile = fitbit.GetUserProfile();
            Console.WriteLine("Your last weight was {0}", profile.Weight);

            Console.ReadLine();
        }

        static AuthCredential Authenticate()
        {
            var requestTokenUrl = "http://api.fitbit.com/oauth/request_token";
            var accessTokenUrl = "http://api.fitbit.com/oauth/access_token";
            var authorizeUrl = "http://www.fitbit.com/oauth/authorize";

            var a = new Authenticator(consumerKey, consumerSecret, requestTokenUrl, accessTokenUrl, authorizeUrl);

            RequestToken token = a.GetRequestToken();

            var url = a.GenerateAuthUrlFromRequestToken(token, false);

            Process.Start(url);

            Console.WriteLine("Enter the verification code from the website");
            var pin = Console.ReadLine();

            var credentials = a.GetAuthCredentialFromPin(pin, token);
            return credentials;
        }

        static void SaveCredentials(AuthCredential credentials)
        {
            try
            {
                var path = GetAppDataPath();
                var serializer = new XmlSerializer(typeof(AuthCredential));
                TextWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, credentials);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static AuthCredential LoadCredentials()
        {
            AuthCredential credentials = null;
            try
            {
                var path = GetAppDataPath();

                if (File.Exists(path))
                {
                    var serializer = new XmlSerializer(typeof(AuthCredential));
                    FileStream fs = new FileStream(path, FileMode.Open);

                    credentials = serializer.Deserialize(fs) as AuthCredential;
                    fs.Close();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return credentials;
        }

        static string GetAppDataPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fitbit");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, "Credentials.xml");
        }
    }
}
