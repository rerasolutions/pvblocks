using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using pvblocks_api.Exceptions;
using pvblocks_api.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace pvblocks_api
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
       
        private ILocalStorageService _localStorageService;
        

        public HttpService(ILocalStorageService localStorageService)
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000/v1/")};

            _localStorageService = localStorageService;
        }


        public string Client
        {
            set
            {
                try
                {
                    _httpClient = new HttpClient { BaseAddress = new Uri($"http://{value}/v1/") };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
        }

        public async Task<T> Get<T>(string uri)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                return await sendRequest<T>(request);
            }
            catch (Exception e)
            {
                throw new HttpServiceException();
            }
           
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
                return await sendRequest<T>(request);
            }
            catch (Exception e)
            {
                throw new HttpServiceException();
            }
        }

        public async Task<T> Delete<T>(string uri)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                return await sendRequest<T>(request);
            }
            catch (Exception e)
            {
                throw new HttpServiceException();
            }
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, uri);
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
                return await sendRequest<T>(request);
            }
            catch (Exception e)
            {
                throw new HttpServiceException();
            }
        }

        // helper methods

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            // add jwt auth header if user is logged in and request is to the api url
            var user = await _localStorageService.GetItem<User>("user");
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;
            if (user != null && isApiUrl)
            {
                var token =  await _localStorageService.GetItem<BearerToken>("token");
                if( token != null)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Bearer);
            }

            using var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new HttpServiceException();
            }


            if (response.StatusCode == HttpStatusCode.NotFound)
                return default(T);

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                throw new Exception(error["message"]);
            }

            var c = response.Content;

            if (response.StatusCode == HttpStatusCode.NoContent)
                return default(T);

            try
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }

            return default(T);

        }
    }
}