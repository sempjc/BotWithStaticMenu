using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class DirectLineTokenGenerator
    {
        private const string DlURL = "https://directline.botframework.com/v3/directline/tokens/generate";
        private readonly string DirectLineUrl;
        private readonly string DlSecret;
        private readonly HttpClient DlClient;

        public DirectLineTokenGenerator(
            string directLineUrl = null, string dlSecret = null)
        {
            if (string.IsNullOrWhiteSpace(dlSecret))
                throw new ArgumentException(
                    $"'{nameof(dlSecret)}' cannot be null or whitespace.",
                    nameof(dlSecret));

            DlSecret = dlSecret;
            DirectLineUrl = string.IsNullOrWhiteSpace(directLineUrl)
                ? DlURL
                : directLineUrl;
            DlClient = new HttpClient();
        }

        public async Task<ChatConfig> TokenAsync()
        {
            string userId = NewUserId();

            HttpRequestMessage request = NewRequest(userId);
            HttpResponseMessage response =
                await DlClient.SendAsync(request).ConfigureAwait(false);

            string token = string.Empty;
            if (response.IsSuccessStatusCode)
                token = await DeserializeToken(response);

            return new()
            {
                Token = token,
                UserId = userId
            };
        }

        private static async Task<string> DeserializeToken(
            HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<DirectLineToken>(body).token;
            return token;
        }

        private HttpRequestMessage NewRequest(string userId)
        {
            HttpRequestMessage request = new();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(DirectLineUrl);
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", DlSecret);
            request.Content = GenerateHttpContent(userId);
            return request;
        }

        private static StringContent GenerateHttpContent(string userId)
        {
            return new StringContent(JsonConvert.SerializeObject(
                new { User = new { Id = userId } }),
                Encoding.UTF8,
                "application/json");
        }

        private static string NewUserId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
