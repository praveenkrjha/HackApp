using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IndoorNavigation
{
    public static class HttpClientEx
    {
        /// <summary>
        ///     Method is responsible to catch the Restclient errors, make the custom exception as RestClientFailureException
        ///     and set the all required parameters
        /// </summary>
        /// <param name="sender">HttpResponseMessage </param>
        public static void EnsureSuccessStatusCodeEx(this HttpResponseMessage sender)
        {
            if (sender.StatusCode == HttpStatusCode.OK)
                return;
            //Get the message from the sender which is used to forward the error message
            var message = sender.ReasonPhrase;
            throw new Exception(message);
        }

        private static async Task<HttpResponseMessage> GetHttpResponse(HttpMethod method, string url,
                                                    string payloadAsJson, HttpClient httpClient)
        {
            var request = new HttpRequestMessage(method, url);

            if (string.IsNullOrEmpty(payloadAsJson))
                return await httpClient.SendAsync(request);
            request.Content = new StringContent(payloadAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await httpClient.SendAsync(request);
        }

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient httpClient, string url,
                                                                        string payloadAsJson)
        {
            return await GetHttpResponse(HttpMethod.Delete, url, payloadAsJson, httpClient);
        }
    }
}
