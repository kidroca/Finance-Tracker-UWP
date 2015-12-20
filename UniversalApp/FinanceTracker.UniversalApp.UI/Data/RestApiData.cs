namespace FinanceTracker.UniversalApp.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Exceptions;
    using Models;
    using Models.User;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Windows.Storage.Streams;
    using Windows.Web.Http;

    /// <summary>
    /// Todo Implement with Singelton with double null check lock
    /// </summary>
    public class RestApiData : IDataAuth
    {
        private string authnenticationToken;

        public RestApiData(string baseUrl)
        {
            this.BaseUri = new Uri(baseUrl);
        }

        public Uri BaseUri { get; private set; }

        public string AuthenticationToken
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Invalid Token: Null or Empty");
                }
                this.authnenticationToken = value;
            }
        }

        public async Task RegisterAsync(UserRegisterModel user)
        {
            var uri = new Uri(this.BaseUri, "api/Account/Register");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            string content = JsonConvert.SerializeObject(
                    new { email = user.Username, user.Password, user.ConfirmPassword });

            request.Content = new HttpStringContent(content, UnicodeEncoding.Utf8, "application/json");

            var response = await this.GetResponse(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("Error during registration, try again");
            }
        }

        public async Task<string> AuthenticateAsync(UserLoginModel user)
        {
            var uri = new Uri(this.BaseUri, "Token");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            string content = $"Username={user.Username}&Password={user.Password}&grant_type=password";
            request.Content = new HttpStringContent(content, UnicodeEncoding.Utf8, "application/x-www-form-urlencoded");

            var response = await this.GetResponse(request);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(json);

                string token = $"Bearer {obj["access_token"]}";
                return token;
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactions(
            string category = null, int page = 1, int size = 10)
        {
            var queryParameters = new[]
            {
                new KeyValuePair<string, string>(nameof(category), category),
                new KeyValuePair<string, string>(nameof(page), page.ToString()),
                new KeyValuePair<string, string>(nameof(size), size.ToString()),
            };

            var endPointUri = this.CreateUriWithQueryString("transactions", queryParameters);

            var request = new HttpRequestMessage(HttpMethod.Get, endPointUri);
            request.Headers.Accept.ParseAdd("application/json");

            var response = await this.GetResponse(request);
            var content = await response.Content.ReadAsStringAsync();
            var collection = await Task.Run(() => JsonConvert.DeserializeObject<TransactionsCollection>(content));

            return collection.Result;
        }

        public TransactionModel AddTransaction(TransactionModel transaction)
        {
            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendRequestAsync(request);
                return response;
            }
        }

        private Uri CreateUriWithQueryString(string endpoint, params KeyValuePair<string, string>[] parameters)
        {
            string query = null;
            using (var content = new HttpFormUrlEncodedContent(parameters))
            {
                query = content.ReadAsStringAsync().GetResults();
            }

            var uri = new Uri(this.BaseUri, endpoint + query);

            return uri;
        }
    }
}