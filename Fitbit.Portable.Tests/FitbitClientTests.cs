using System;
using Fitbit.Api.Portable;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientTests
    {
        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Empty()
        {
            new FitbitClient(string.Empty, "secret", "access", "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Null()
        {
            new FitbitClient(null, "secret", "access", "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Empty()
        {
            new FitbitClient("key", string.Empty, "access", "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Null()
        {
            new FitbitClient("key", null, "access", "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccesToken_Empty()
        {
            new FitbitClient("key", "secret", string.Empty, "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessToken_Null()
        {
            new FitbitClient("key", "secret", null, "accessSecret");
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Empty()
        {
            new FitbitClient("key", "secret", "access", null);
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Null()
        {
            new FitbitClient("key", "secret", "access", string.Empty);
        }

        [Test] [Category("Portable")]
        public void Constructor_HttpClient_Create()
        {
            var client = new FitbitClient("key", "secret", "access", "accessToken");
            Assert.IsNotNull(client.HttpClient);
        }
    }
}