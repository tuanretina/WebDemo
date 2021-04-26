using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;
namespace WebDemo.Models
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;
        static PaypalConfiguration()
        {
            var config = GetConfig();
            clientId = "ASgghunifQrTwNyGx5MnYDo5D7wBZuxoJvK0EGmoIwcU1pxA-97VQfZWP8IKl2Qmqw3U6y8CwoyzqDMI";
            clientSecret = "EE7sZsColXD-tS59U-g0xyjcx-LaFc7lSsJkTVvPQ9jgGRXIcM3oM9Q0nKVLu5Jm8uBPs6eJeptpUHHa";
        }

        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }
        //Create access Token
        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        //This will return API Context object
        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}