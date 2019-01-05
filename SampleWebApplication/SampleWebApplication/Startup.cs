using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SampleWebMVCOAuth2.Startup))]
namespace SampleWebMVCOAuth2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}