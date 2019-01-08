using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace BubbleWar
{
    public abstract class BaseGraphQLService
    {
        #region Constant Fields
        static readonly Lazy<JsonSerializer> _serializerHolder = new Lazy<JsonSerializer>();
        static readonly Lazy<HttpClient> _clientHolder = new Lazy<HttpClient>(() => CreateHttpClient(TimeSpan.FromSeconds(60)));
        #endregion

        #region Fields
        static int _networkIndicatorCount = 0;
        #endregion

        #region Properties
        static HttpClient Client => _clientHolder.Value;
        static JsonSerializer Serializer => _serializerHolder.Value;
        #endregion

        #region Methods
        protected static async Task<TResponse> PostObjectToAPI<TRequest, TResponse>(string apiUrl, TRequest data)
        {
            using (var responseMessage = await PostObjectToAPI(apiUrl, data).ConfigureAwait(false))
                return await DeserializeResponse<TResponse>(responseMessage).ConfigureAwait(false);
        }

        protected static Task<HttpResponseMessage> PostObjectToAPI<T>(string apiUrl, T data) => SendAsync(HttpMethod.Post, apiUrl, data);

        static HttpClient CreateHttpClient(TimeSpan timeout)
        {
            HttpClient client;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    client = new HttpClient();
                    break;
                default:
                    client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
                    break;
            }

            client.Timeout = timeout;
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        static async Task<HttpRequestMessage> GetHttpRequestMessage<T>(HttpMethod method, string apiUrl, T requestData = default)
        {
            var httpRequestMessage = new HttpRequestMessage(method, apiUrl);

            switch (requestData)
            {
                case T data when !typeof(T).IsValueType && data == null:
                    break;

                case Stream stream:
                    httpRequestMessage.Content = new StreamContent(stream);
                    httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    break;

                default:
                    var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(requestData)).ConfigureAwait(false);
                    httpRequestMessage.Content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    break;
            }

            return httpRequestMessage;
        }

        static async Task<HttpResponseMessage> SendAsync<T>(HttpMethod httpMethod, string apiUrl, T requestData)
        {
            using (var httpRequestMessage = await GetHttpRequestMessage(httpMethod, apiUrl, requestData))
            {
                try
                {
                    UpdateActivityIndicatorStatus(true);

                    return await Client.SendAsync(httpRequestMessage).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    Report(e);
                    throw;
                }
                finally
                {
                    UpdateActivityIndicatorStatus(false);
                }
            }
        }

        static void UpdateActivityIndicatorStatus(bool isActivityIndicatorDisplayed)
        {
            if (isActivityIndicatorDisplayed)
            {
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = true);
                _networkIndicatorCount++;
            }
            else if (--_networkIndicatorCount <= 0)
            {
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.IsBusy = false);
                _networkIndicatorCount = 0;
            }
        }

        static async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            httpResponseMessage.EnsureSuccessStatusCode();

            try
            {
                using (var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var reader = new StreamReader(contentStream))
                using (var json = new JsonTextReader(reader))
                {
                    if (json is null)
                        return default;

                    return await Task.Run(() => Serializer.Deserialize<T>(json)).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Report(e);
                throw;
            }
        }

        static void Report(Exception e, [CallerMemberName]string callerMemberName = "") => AppCenterService.Report(e, callerMemberName: callerMemberName);
        #endregion
    }
}
