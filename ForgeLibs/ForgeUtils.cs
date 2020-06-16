using ForgeLibs.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ForgeLibs.Models.Forge;
using Newtonsoft.Json;

namespace ForgeLibs
{
    public class ForgeUtils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static async Task<ForgeAuth> GetForgeAuth(string clientId, string clientSecret, params string[] scopes)
        {
            string uri = "https://developer.api.autodesk.com/authentication/v1/authenticate";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("client_id", clientId);
            dic.Add("client_secret", clientSecret);
            dic.Add("grant_type", "client_credentials");
            string scopeStr = string.Join(" ", scopes);

            dic.Add("scope", scopeStr);

            using (HttpClient client = new HttpClient())
            {
                var encoded = new FormUrlEncodedContent(dic);
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri) { Content = encoded };
                HttpResponseMessage response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead);
                var statusCode = response.StatusCode;
                var content = response.Content;

                ForgeAuth auth = JsonConvert.DeserializeObject<ForgeAuth>(content.ReadAsStringAsync().Result);
                return auth;
            }
        }

        public static async Task<dynamic> GetObjects(string clientId, string clientSecret, string bucketKey)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "viewables:read", "data:read", "bucket:read");
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}/objects";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                ForgeObjects metadata = JsonConvert.DeserializeObject<ForgeObjects>(content);
                return metadata;
            }
        }

        public static async Task<ForgeViewsMetadata> GetForgeViews(string clientId, string clientSecret, string projectUrn64)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read");
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{projectUrn64}/metadata";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();
                ForgeViewsMetadata metadata = JsonConvert.DeserializeObject<ForgeViewsMetadata>(content);
                return metadata;
            }
        }

        public static async Task<dynamic> GetProjectMetadata(string clientId, string clientSecret, string projectUrn64, string viewGuid)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read");
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{projectUrn64}/metadata/{viewGuid}/properties?forceget=true";
            //string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{projectUrn64}/metadata/{viewGuid}?forceget=true";
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();
                dynamic metadata = JsonConvert.DeserializeObject(content);
                return metadata;
            }
        }
    }
}
