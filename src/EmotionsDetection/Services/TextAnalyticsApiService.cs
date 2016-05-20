using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EmotionsDetection.Models;
using Newtonsoft.Json;

namespace EmotionsDetection.Services
{
    public class TextAnalyticsApiService
    {
        private const string RequestUrl = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        private readonly string _subscriptionKey;

        public TextAnalyticsApiService(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<double> Analyze(string text)
        {
            var requestObj = new { documents = new[] { new { id = "1", text }} } ;
            var requestBody = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");

            using (var client = GetHttpClient())
            {
                var response = await client.PostAsync(RequestUrl, requestBody);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(content);
                }

                var result = JsonConvert.DeserializeObject<TextAnalysisApiResultModel>(content);

                // Just take the first score for demo purposes.
                return result.documents.First().score;
            }
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
            return client;
        }
    }
}