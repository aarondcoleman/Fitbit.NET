using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using OutputColorizer;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SampleConsole
{
    public static class AuthorizationHelper
    {
        public static FitbitClient GetAuthorizedFitBitClient(params string[] scopes)
        {
            // try to retrieve the token from disk
            OAuth2AccessToken token = GetAccessTokenAsync(scopes).Result;

            return new FitbitClient(new FitbitAppCredentials() { ClientId = Options.ClientId, ClientSecret = Options.ClientSecret }, token, true);
        }

        public static async Task<OAuth2AccessToken> GetAccessTokenAsync(params string[] scopes)
        {
            var token = ReadTokenFromDisk();
            if (token == null || token.UtcExpirationDate < DateTime.Now.ToUniversalTime())
            {
                // we need admin to retrieve the token automatically
                RestartAsElevatedIfNeeded();

                token = await AuthorizeToFitBitAsync(scopes.Length == 0 ? Options.AllScopes : scopes);

                SaveTokenToFile(token);
            }

            return token;
        }

        private static void SaveTokenToFile(OAuth2AccessToken token)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(OAuth2AccessToken));
                using (StreamWriter sw = new StreamWriter("token.dat"))
                {
                    ser.Serialize(sw, token);
                }
            }
            catch
            {
            }
        }

        private static OAuth2AccessToken ReadTokenFromDisk()
        {
            if (!File.Exists("token.dat"))
            {
                return null;
            }

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(OAuth2AccessToken));
                using (StreamReader sr = new StreamReader("token.dat"))
                {
                    return ser.Deserialize(sr) as OAuth2AccessToken;
                }
            }
            catch
            {
                return null;
            }
        }

        private static async Task<OAuth2AccessToken> AuthorizeToFitBitAsync(string[] scopes)
        {
            Colorizer.WriteLine("Sending authorization request...");
            string scope = string.Join("%20", scopes);
            string authorizeUrl = $"https://www.fitbit.com/oauth2/authorize?response_type=code&client_id={Options.ClientId}&scope={scope}&expires_in=86400";
            Process.Start(authorizeUrl);

            Colorizer.WriteLine("Waiting for callback at [Yellow!http://localhost]");
            string code;
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost/");
                listener.Start();
                HttpListenerContext context = listener.GetContext();

                Colorizer.WriteLine("Request received, retrieving code");
                HttpListenerRequest request = context.Request;

                //retrieve the code from the raw request.
                code = request.QueryString["code"];
            }

            Colorizer.WriteLine("Exchanging code for authentication token");
            using (HttpClient hc = new HttpClient())
            {
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                requestMsg.Method = HttpMethod.Post;
                requestMsg.RequestUri = new Uri("https://api.fitbit.com/oauth2/token");
                string authorizationHeader = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));
                requestMsg.Headers.Add("Authorization", "Basic " + authorizationHeader);
                requestMsg.Content = new StringContent($"client_id={Options.ClientSecret}&grant_type=authorization_code&code={code}", Encoding.ASCII, "application/x-www-form-urlencoded");

                Colorizer.Write("Making request...");
                using (var responseMsg = await hc.SendAsync(requestMsg))
                {
                    Colorizer.WriteLine("[Green!done].");
                    if (responseMsg.IsSuccessStatusCode)
                    {
                        var tok = OAuth2Helper.ParseAccessTokenResponse(await responseMsg.Content.ReadAsStringAsync());
                        tok.UtcExpirationDate = DateTime.Now.ToUniversalTime().AddSeconds(tok.ExpiresIn);

                        return tok;
                    }
                }
            }

            return null;
        }

        private static void RestartAsElevatedIfNeeded()
        {
            // we can't authorize if we are not admin because we require access to register a listener to http://localhost.
            if (!WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
            {
                // start the same process (as admin) 
                Process elevatedProcess = new Process();
                elevatedProcess.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                elevatedProcess.StartInfo.Arguments = string.Join(" ", Environment.GetCommandLineArgs()); // pass whatever arguments were passed before.
                elevatedProcess.StartInfo.Verb = "runas"; //run as admin
                elevatedProcess.Start();

                Environment.Exit(0);
            }
        }
    }
}
