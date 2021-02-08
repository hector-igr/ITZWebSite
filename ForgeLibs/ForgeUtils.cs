
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ForgeLibs.Models.Forge;
using Newtonsoft.Json;

using System.Net;
using System.IO;
using System.Linq;
//System.Text.Json.JsonSerializer;

namespace ForgeLibs
{
	public class ForgeUtils
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static T JsonDeserialze<T>(string content)
		{
            //var options = new JsonSerializerOptions()
            //{
            //    PropertyNameCaseInsensitive = true
            //};
            //return JsonSerializer.Deserialize<T>(content, options);

            return JsonConvert.DeserializeObject<T>(content);
        }
        public static string JsonSerialze(object content)
        {
            //var options = new JsonSerializerOptions()
            //{
            //    PropertyNameCaseInsensitive = true
            //};
            //return JsonSerializer.Deserialize<T>(content, options);

            return JsonConvert.SerializeObject(content);
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
                string content = await response.Content.ReadAsStringAsync();

                ForgeAuth auth = JsonDeserialze<ForgeAuth>(content);
                return auth;
            }
        }

   
        public static async Task<dynamic> CreateForgeBucket(string clientId, string clientSecret, string bucketName)
		{
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "bucket:create");
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets";
            using (HttpClient client = new HttpClient())
			{
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "bucketKey", bucketName },
                    //{ "authId", "" },
                    { "access", "full" },
                    { "policyKey", "persistent" }
                };
				//requestMessage.Content = new FormUrlEncodedContent(body);
				//requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"application/json");
				var requestContent = new StringContent(JsonSerialze(body), Encoding.UTF8, "application/json");
                request.Content = requestContent;
                HttpResponseMessage response = await client.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonDeserialze<dynamic>(content);
                return data;
            }
        }

        public static async Task<dynamic> GetBuckets(string clientId, string clientSecret)
		{
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "bucket:read");
            string uri = "https://developer.api.autodesk.com/oss/v2/buckets";
            using (HttpClient client = new HttpClient())
			{
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
                message.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                HttpResponseMessage response = await client.SendAsync(message);
                string content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonDeserialze<dynamic>(content);
                return data;
            }
		}

        public static async Task<HttpStatusCode> DeleteBucket(string clientId, string clientSecret, string bucketKey)
		{
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "bucket:delete");
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}";
			using (HttpClient client = new HttpClient())
			{
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, uri);
                request.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");

                HttpResponseMessage response = await client.SendAsync(request);
                //string content = await response.Content.ReadAsStringAsync();
                return response.StatusCode;
			}
        }

        public static async Task<dynamic> LoadByChunks(string clientId, string clientSecret, string bucketKey, string objectKey, string filePath)
		{
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:write");
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}/objects/{objectKey}/resumable";

            string lastResponse = "";

            long fileSize = (new FileInfo(filePath)).Length;
            int UPLOAD_CHUNK_SIZE = 2;
            if (fileSize > UPLOAD_CHUNK_SIZE * 1024 * 1024) // upload in chunks
            {
                long chunkSize = 2 * 1024 * 1024; // 2 Mb
                long numberOfChunks = (long)Math.Round((double)(fileSize / chunkSize)) + 1;

                long start = 0;
                chunkSize = (numberOfChunks > 1 ? chunkSize : fileSize);
                long end = chunkSize;
                string sessionId = Guid.NewGuid().ToString();
                
                // upload one chunk at a time
                using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
                {
                    for (int chunkIndex = 0; chunkIndex < numberOfChunks; chunkIndex++)
                    {
                        string range = string.Format("bytes {0}-{1}/{2}", start, end, fileSize);

                        long numberOfBytes = chunkSize + 1;
                        byte[] fileBytes = new byte[numberOfBytes];
                        MemoryStream memoryStream = new MemoryStream(fileBytes);
                        reader.BaseStream.Seek((int)start, SeekOrigin.Begin);
                        int count = reader.Read(fileBytes, 0, (int)numberOfBytes);
                        memoryStream.Write(fileBytes, 0, (int)numberOfBytes);
                        memoryStream.Position = 0;

                        //dynamic chunkUploadResponse = await objects.UploadChunkAsync(bucketKey, objectKey, (int)numberOfBytes, range, sessionId, memoryStream);

                        using (HttpClient client = new HttpClient())
                        {
                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, uri);
                            request.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                            //request.Headers.Add("Content-Length", numberOfBytes.ToString());
                            //request.Headers.Add("Content-Range", range);
                            request.Headers.Add("Session-Id", sessionId);

                            StreamContent body = new StreamContent(memoryStream);
                            request.Content = body;
                            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain") ;
                            request.Content.Headers.Add("Content-Length", numberOfBytes.ToString());
                            request.Content.Headers.Add("Content-Range", range);

                            HttpResponseMessage response = await client.SendAsync(request);
                            string content = await response.Content.ReadAsStringAsync();
                            lastResponse = content;
                        }   

                        start = end + 1;
                        chunkSize = ((start + chunkSize > fileSize) ? fileSize - start - 1 : chunkSize);
                        end = start + chunkSize;

                    }
                }
            }
            else // upload in a single call
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    //dynamic uploadedObj = await objects.UploadObjectAsync(bucketKey,
                    //       objectKey, (int)streamReader.BaseStream.Length, streamReader.BaseStream,
                    //       "application/octet-stream");
                }

            }
            return JsonDeserialze<dynamic>(lastResponse);
        }

        public static async Task<dynamic> GetObjects(string clientId, string clientSecret, string bucketKey)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "viewables:read", "data:read", "bucket:read");
            string accessToken = auth.Access_Token;
            return await GetObjects(accessToken, bucketKey);
        }

        public static async Task<dynamic> GetObjects(string accessToken, string bucketKey)
		{
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}/objects";
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();
                dynamic metadata = JsonDeserialze<dynamic>(content);
                return metadata;
            }
        }

        public static async Task<HttpStatusCode> DeleteObject(string clientId, string clientSecret, string bucketKey, string objectName)
		{
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:write");
            string uri = $"https://developer.api.autodesk.com/oss/v2/buckets/{bucketKey}/objects/{objectName}";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                return response.StatusCode;
            }
        }

        public static async Task<dynamic> GetForgeViews(string clientId, string clientSecret, string urn)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read");
            string accessToken = auth.Access_Token;
            return await GetForgeViews(accessToken, urn);
        }
        public static async Task<dynamic> GetForgeViews(string accessToken, string urn)
		{
            string urn64 = ForgeUtils.Base64Encode(urn);
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{urn64}/metadata";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();

                dynamic metadata = JsonDeserialze<dynamic>(content);
                return metadata;
            }
        }


        public static async Task<dynamic> GetProjectMetadata(string clientId, string clientSecret, string urn)
		{
            dynamic views = await ForgeUtils.GetForgeViews(clientId, clientSecret, urn);
            dynamic[] metadata = views.metadata;
            dynamic view = metadata.FirstOrDefault(x => x.role == "3d");
            if (view != null)
            {
                dynamic data = await ForgeUtils.GetProjectMetadata(clientId, clientSecret, urn, view.guid);
                return data;
            }
            else
            {
                return null;
            }
        }

        public static async Task<dynamic> GetProjectMetadata(string clientId, string clientSecret, string urn, string viewGuid)
        {
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read");
            string urn64 = ForgeUtils.Base64Encode(urn);
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{urn64}/metadata/{viewGuid}/properties?forceget=true";
            //string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{urn64}/metadata/{viewGuid}?forceget=true";
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");

                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string content = await response.Content.ReadAsStringAsync();
                dynamic metadata = JsonDeserialze<dynamic>(content);
                return metadata;
            }
        }


        public static async Task<dynamic> TranslateFileToSVF(string clientId, string clientSecret, string urn)
		{
            string urn64 = Base64Encode(urn);
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read", "data:write");
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/job";

			using (HttpClient client = new HttpClient())
			{
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");

                Dictionary<string, object> body = new Dictionary<string, object>()
                {
                    { "input", new Dictionary<string, object>() { { "urn", urn64 } } },
                    { "output", new Dictionary<string, object>() {
                        { "destination", new { region = "us" } },
                        { "formats", new dynamic[] { new { type = "svf2", views = new string []{ "2d", "3d" } } } } } },
                };
                string json = JsonSerialze(body);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                request.Content = requestContent;

                HttpResponseMessage response = await client.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonDeserialze<dynamic>(content);
                return content;
            }
        }

        public static async Task<dynamic> GetManifest(string clientId, string clientSecret, string urn)
		{
            string urn64 = Base64Encode(urn);
            ForgeAuth auth = await ForgeUtils.GetForgeAuth(clientId, clientSecret, "data:read");
            string uri = $"https://developer.api.autodesk.com/modelderivative/v2/designdata/{urn64}/manifest";

			using (HttpClient client = new HttpClient())
			{
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("Authorization", $"Bearer {auth.Access_Token}");

                HttpResponseMessage response = await client.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonDeserialze<dynamic>(content);
                return data;
            }
		}
    }


}
