using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IndoorNavigation
{
    public class RestClient : IRestClient, IDisposable
    {
        enum HttpVerbs
        {
            Get,
            Post,
            Put,
            Delete
        }
        private const string UserInfoHeader = "UserInfo";
        private const string RequestToken = "RequestToken";
        private const string SecurityToken = "SecurityToken";
        private const string RequestTokenValue = "B7A6A1F06D8A48BE826BBD184D0BBE17F224C661DABBD9B581ADAA7F2D56A375";
        //private const string RequestTokenValue = "vhzjwdfGY43yL1SRDebtvlW6FplOs2r4__hTvf-O816EZbLYcQdWmBT1JxMFZFf79pNT3to5kQpQj7QGZYnUA";
        private string baseUrl;
        private readonly HttpClient RestHttpClient;
        public RestClient()
        {
            RestHttpClient = new HttpClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public string BaseUrl
        {
            get { return baseUrl; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("base url is not defined");
                }
                baseUrl = value;
            }
        }

        #region Private Methods
        private string AddHeadersAndGetRestUrl(string url, params object[] args)
        {
            return string.Format(BaseUrl + string.Format(url, args));
        }

        /// <summary>
        ///     function to handle the exceptions thrown by the api
        /// </summary>
        /// <returns>exception</returns>
        private Exception HandleException(string restUrl, Exception ex)
        {
            if (!(ex is HttpRequestException))
                return ex;

            var message = new StringBuilder($"Request Failed: {restUrl}");
            if (ex.InnerException == null)
            {
                message.Append($"\n {ex.Message}");
                return new HttpRequestException(message.ToString(), ex);
            }
            message.Append($"\n {ex.InnerException.Message}");
            return new HttpRequestException(message.ToString(), ex.InnerException);
        }
        private async Task<HttpResponseMessage> GetHttpResponse(HttpVerbs httpVerb, string restUrl, object objectPayLoad, params object[] args)
        {
            RestHttpClient.DefaultRequestHeaders.Clear();
            RestHttpClient.DefaultRequestHeaders.Add(RequestToken, RequestTokenValue);
            string securityToken = string.Empty;
            if (App.Current.Properties.ContainsKey("SecurityToken"))
            {
                securityToken = Convert.ToString(App.Current.Properties["SecurityToken"]);
            }
            if (!string.IsNullOrEmpty(securityToken))
                RestHttpClient.DefaultRequestHeaders.Add(SecurityToken, securityToken);

            var response = new HttpResponseMessage();
            // var payLoad = objectPayLoad == null ? string.Empty : JsonConvert.SerializeObject(objectPayLoad);
            try
            {
                response = await RestHttpClient.GetAsync(restUrl);
                response.EnsureSuccessStatusCodeEx();
            }
            catch (Exception ex)
            {
                throw HandleException(restUrl, ex);
            }

            return response;
        }

        private async Task<HttpResponseMessage> GetHttpResponse<T>(HttpVerbs httpVerb, string restUrl, T payLoad, params object[] args)
        {
            var response = new HttpResponseMessage();
            RestHttpClient.DefaultRequestHeaders.Clear();
            RestHttpClient.DefaultRequestHeaders.Add(RequestToken, RequestTokenValue);


            string securityToken = string.Empty;
            if (App.Current.Properties.ContainsKey("SecurityToken"))
            {
                securityToken = Convert.ToString(App.Current.Properties["SecurityToken"]);
            }
            if (!string.IsNullOrEmpty(securityToken))
                RestHttpClient.DefaultRequestHeaders.Add(SecurityToken, securityToken);


            RestHttpClient.Timeout = TimeSpan.FromSeconds(30);

            try
            {
                HttpMethod httpMethod;

                switch (httpVerb)
                {
                    case HttpVerbs.Get:
                        httpMethod = HttpMethod.Get;
                        break;

                    case HttpVerbs.Post:
                        httpMethod = HttpMethod.Post;
                        break;

                    case HttpVerbs.Put:
                        httpMethod = HttpMethod.Put;
                        break;

                    case HttpVerbs.Delete:
                        httpMethod = HttpMethod.Delete;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(httpVerb), httpVerb, null);
                }
                var myContent = JsonConvert.SerializeObject(payLoad);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                var requestMessage = new HttpRequestMessage(httpMethod, restUrl);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                requestMessage.Content = byteContent;
                response = await RestHttpClient.SendAsync(requestMessage).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                throw HandleException(restUrl, ex);
            }

            response.EnsureSuccessStatusCodeEx();

            return response;
        }

        #endregion

        /// <summary>
        ///     GetAsync method for generic return types like class,enumerable etc.21
        ///     Exception in this method need to be handled by calling method.
        /// </summary>
        public async Task<T> GetAsync<T>(string url, params object[] args)
        {
            var json = await GetAsync(url, args);

            var serializedObject = JsonConvert.DeserializeObject<T>(json);

            return serializedObject;
        }

        /// <summary>
        ///     GetAsync method for type void.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);

            var response = await GetHttpResponse(HttpVerbs.Get, restUrl, null, args);

            var jsonResult = await response.Content.ReadAsStringAsync();

            return jsonResult;
        }

        /// <summary>
        ///     DeleteASync method.
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        public async Task DeleteAsync(string url, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);
            await GetHttpResponse(HttpVerbs.Delete, restUrl, null, args);
        }

        /// <summary>
        ///     Call DELETE method of Rest Api with Payload of type TPayload and expecting result of type TResult
        /// </summary>
        /// <param name="url">Url of the Rest Resource with placeholders for argument</param>
        /// <param name="payload">Payload to be posted to Rest api</param>
        /// <param name="args">Values for placeholder arguments</param>
        /// <returns>Value of type TResult</returns>
        public async Task<string> DeleteAsync(string url, object payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);
            var response = await GetHttpResponse(HttpVerbs.Delete, restUrl, payload, args);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        ///     Call DELETE method of Rest Api with Payload of type TPayload and expecting result of type TResult
        /// </summary>
        /// <typeparam name="TPayload">Type of Payload</typeparam>
        /// <typeparam name="TResult">Type of Result</typeparam>
        /// <param name="url">Url of the Rest Resource with placeholders for argument</param>
        /// <param name="payload">Payload to be posted to Rest api</param>
        /// <param name="args">Values for placeholder arguments</param>
        /// <returns>Value of type TResult</returns>
        public async Task<TResult> DeleteAsync<TPayload, TResult>(string url, TPayload payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);

            var response = await GetHttpResponse(HttpVerbs.Delete, restUrl, payload, args);

            var json = await response.Content.ReadAsStringAsync();

            var serializer = JsonConvert.DeserializeObject<TResult>(json);
            return serializer;
        }

        /// <summary>
        ///     Call DELETE method of Rest Api with Payload of type T
        /// </summary>
        /// <typeparam name="T">Type of Payload</typeparam>
        /// <param name="url">Url of the Rest Resource with placeholders for argument</param>
        /// <param name="payload">Payload to be posted to Rest api</param>
        /// <param name="args">Values for placeholder arguments</param>
        /// <returns></returns>
        public async Task DeleteAsync<T>(string url, T payload, params object[] args)
        {
            try
            {
                var restUrl = AddHeadersAndGetRestUrl(url, args);
                var response = await GetHttpResponse(HttpVerbs.Delete, restUrl, payload, args);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, object payload, params object[] args)
        {
            try
            {
                var restUrl = AddHeadersAndGetRestUrl(url, args);
                var response = await GetHttpResponse(HttpVerbs.Post, restUrl, payload, args);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return null;
            }

        }

        /// <summary>
        ///     Call POST method of Rest Api with Payload of type TPayload and expecting result of type TResult
        /// </summary>
        /// <typeparam name="TPayload">Type of Payload</typeparam>
        /// <typeparam name="TResult">Type of Result</typeparam>
        /// <param name="url">Url of the Rest Resource with placeholders for argument</param>
        /// <param name="payload">Payload to be posted to Rest api</param>
        /// <param name="args">Values for placeholder arguments</param>
        /// <returns>Value of type TResult</returns>
        public async Task<TResult> PostAsync<TPayload, TResult>(string url, TPayload payload, params object[] args)
        {
            try
            {
                var restUrl = AddHeadersAndGetRestUrl(url, args);

                var response = await GetHttpResponse(HttpVerbs.Post, restUrl, payload, args);

                var json = await response.Content.ReadAsStringAsync();

                var serializer = JsonConvert.DeserializeObject<TResult>(json);
                return serializer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        ///     Call POST method of Rest Api with Payload of type T
        /// </summary>
        /// <typeparam name="T">Type of Payload</typeparam>
        /// <param name="url">Url of the Rest Resource with placeholders for argument</param>
        /// <param name="payload">Payload to be posted to Rest api</param>
        /// <param name="args">Values for placeholder arguments</param>
        /// <returns></returns>
        public async Task PostAsync<T>(string url, T payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);
            await GetHttpResponse(HttpVerbs.Post, restUrl, payload, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string url, object payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);

            var response = await GetHttpResponse(HttpVerbs.Put, restUrl, payload, args);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        ///     PostAsync method for sending post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Post URL</param>
        /// <param name="payload">Arguments to be sent in post body</param>
        /// <param name="args">Arguments to be sent in query string</param>
        /// <returns></returns>
        public async Task PutAsync<T>(string url, T payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);
            await GetHttpResponse(HttpVerbs.Put, restUrl, payload, args);
        }

        /// <summary>
        ///     PostAsync method for sending post request
        /// </summary>
        /// <param name="url">Post URL</param>
        /// <param name="payload">Arguments to be sent in post body</param>
        /// <param name="args">Arguments to be sent in query string</param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TPayload, TResult>(string url, TPayload payload, params object[] args)
        {
            var restUrl = AddHeadersAndGetRestUrl(url, args);

            var response = await GetHttpResponse(HttpVerbs.Put, restUrl, payload, args);

            var json = await response.Content.ReadAsStringAsync();

            var serializer = JsonConvert.DeserializeObject<TResult>(json);
            return serializer;
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && RestHttpClient != null)
            {
                RestHttpClient.Dispose();
            }
        }
    }
}
