using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IndoorNavigation.Services
{
    public class RestService
    {
        private static string responseString;
        private static string inputDataString;
        public async Task<string> PostRequestRaw(string url, string data)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                responseString = null;
                // Create a new HttpWebRequest object.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpResponseMessage response;
                request.ContentType = "application/json";

                // Set the Method property to 'POST' to post data to the URI.
                request.Method = "POST";
                inputDataString = data;
                //client.DefaultRequestHeaders.Add("Authorization", AppConstants.user_id);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                response = await client.PostAsync(new Uri(url), content);
                var placesJson = response.Content.ReadAsStringAsync().Result;
                return placesJson;
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data1 = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        var ErrorResponse = reader.ReadToEnd();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            //return responseString;
        }

        public string GetRequestRaw(string url)
        {
            try
            {
                responseString = null;
                // Create a new HttpWebRequest object.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpResponseMessage response;
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the Method property to 'POST' to post data to the URI.
                request.Method = "GET";
                //request.Headers.Add("Authorization", AppConstants.user_id);

                Stream resStream = request.GetResponse().GetResponseStream();
                StreamReader streamRead = new StreamReader(resStream);
                responseString = streamRead.ReadToEnd();
                return responseString;

            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            //return responseString;
        }

        public async Task<string> GetRequestRawAsync(string url)
        {
            try
            {
                HttpClient wc = new HttpClient();
                //wc.DefaultRequestHeaders.Add("Authorization", AppConstants.user_id);
                try
                {
                    var resp = await wc.GetStringAsync(url);
                    return resp;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            //return responseString;
        }
    }
}
