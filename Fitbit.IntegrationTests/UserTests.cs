using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Fitbit.Api;
using RestSharp;
using Fitbit.Models;


namespace Fitbit.IntegrationTests
{
    [TestFixture]
    public class UserTests : TestsBase
    {
        [Test]
        public void Retrieve_User_Profile_Data()
        {
            UserProfile userProfile = client.GetUserProfile(Configuration.FitbitUserId);

            Assert.IsNotNull(userProfile);

            Assert.IsNotNull(userProfile.FullName);
            Console.WriteLine("FullName: " + userProfile.FullName);

            Assert.IsNotNull(userProfile.EncodedId);
            Console.WriteLine("EncodedId: " + userProfile.EncodedId);

            Assert.IsNotNull(userProfile.MemberSince);
            Assert.AreNotEqual(new DateTime(), userProfile.MemberSince); //checking for initialized but not set date
        }

        [Test]
        public void Retrieve_User_Friends()
        {
            List<UserProfile> userFriends = client.GetFriends();

            Assert.IsNotNull(userFriends);

            Console.WriteLine("# of friends:" + userFriends.Count);
            foreach(UserProfile friend in userFriends)
            {
                Console.WriteLine("Friend:" + friend.DisplayName + " - " + friend.EncodedId);
            }
        }

        [Test]
        public void Retrieve_User_Devices()
        {
            List<Device> userDevices = client.GetDevices();

            Assert.IsNotNull(userDevices);
            Assert.IsTrue(userDevices.Count > 0);

            Console.WriteLine("# of devices:" + userDevices.Count);
            foreach (Device device in userDevices)
            {
                Console.WriteLine("Device:" + device.Id + " - " + device.DeviceVersion + " - " + device.Type + " - Battery:" + device.Battery);
            }
        }
    }
}
