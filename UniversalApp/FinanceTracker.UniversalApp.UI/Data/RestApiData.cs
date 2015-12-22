namespace FinanceTracker.UniversalApp.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Exceptions;
    using Models;
    using Models.Categories;
    using Models.Transactions;
    using Models.User;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Windows.Storage.Streams;
    using Windows.Web.Http;
    using Windows.Web.Http.Headers;

    /// <summary>
    /// Singleton Implementation, use GetInstance to obtain the instance of this class
    /// Implemented a synchronization lock for the GetInstance method
    /// </summary>
    public class RestApiData : IDataAuth, IData
    {
        private static RestApiData instance;

        private static readonly object syncLock = new object();

        private string authnenticationToken;

        private RestApiData()
        {
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
                this.IsAuthenticated = true;
            }
        }

        public bool IsAuthenticated { get; private set; }

        public static RestApiData GetInstance(string baseUrl)
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new RestApiData();
                    }
                }
            }

            instance.BaseUri = new Uri(baseUrl);
            return instance;
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

        public async Task<IEnumerable<TransactionModel>> GetTransactionsAsync(
            params KeyValuePair<string, string>[] queryParameters)
        {
            this.ConfirmAuthentication();

            var endPointUri = this.CreateUriWithQueryString("api/Transactions", queryParameters);

            var request = new HttpRequestMessage(HttpMethod.Get, endPointUri);
            request.Headers.Add("Authorization", this.authnenticationToken);
            request.Headers.Accept.ParseAdd("application/json");

            var response = await this.GetResponse(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var collection = await Task.Run(() => JsonConvert
                    .DeserializeObject<List<TransactionModel>>(content));

                return collection;
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            this.ConfirmAuthentication();

            var uri = new Uri(this.BaseUri, "api/categories");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.ParseAdd("application/json");
            request.Headers.Add("Authorization", this.authnenticationToken);

            var response = await this.GetResponse(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                IEnumerable<string> categoryNames = await Task.Run(() =>
                {
                    var categories = JsonConvert.DeserializeObject<List<CategoriesResponseModel>>(content)
                        .Select(c => c.Name);

                    return categories;
                });

                return categoryNames;
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<BalanceResponseModel> GetBalanceInformationAsync()
        {
            this.ConfirmAuthentication();

            var uri = new Uri(this.BaseUri, "api/Account/Balance");
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", this.authnenticationToken);
            request.Headers.Accept.ParseAdd("application/json");

            var response = await this.GetResponse(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var balanceInfo = JsonConvert.DeserializeObject<BalanceResponseModel>(content);

                return balanceInfo;
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task AddTransactionAsync(TransactionModel transaction)
        {
            this.ConfirmAuthentication();

            var uri = new Uri(this.BaseUri, "api/Transactions");
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            var asJson = JsonConvert.SerializeObject(transaction);

            request.Content = new HttpStringContent(asJson, UnicodeEncoding.Utf8, "application/json");
            request.Headers.Add("Authorization", this.authnenticationToken);

            var response = await this.GetResponse(request);

            if (response.IsSuccessStatusCode)
            {
                // Contains the transaction id
                var createdAt = response.Headers.Location;
            }
            else
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task UpdateTransactionAsync(TransactionType transaction)
        {
            this.ConfirmAuthentication();

            var uri = new Uri(this.BaseUri, "api/Transactions");
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", this.authnenticationToken);
            request.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

            var response = await this.GetResponse(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        protected virtual void ConfirmAuthentication()
        {
            if (!this.IsAuthenticated)
            {
                throw new ApplicationException("You don't have the rights to access this page.");
            }
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
            // Todo: Add Escaping Character
            var uriBuilder = new UriBuilder(new Uri(this.BaseUri, endpoint));

            if (parameters.Length == 0)
            {
                return uriBuilder.Uri;
            }

            var first = parameters.First();
            uriBuilder.Query = string.Format("{0}={1}", first.Key, first.Value ?? "null");

            for (int i = 1; i < parameters.Length; i++)
            {
                var pair = parameters[i];
                uriBuilder.Query = uriBuilder.Query.Substring(1) +
                    string.Format("&{0}={1}", pair.Key, pair.Value ?? "null");
            }

            return uriBuilder.Uri;
        }
    }
}