using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using pvblocks_api.Exceptions;
using pvblocks_api.Model;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace pvblocks_api
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        public HttpService()
        {
            _httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000/v1/")};

        }

        public string Apikey { get; set; } = "";

        private JwtToken _token;
        private string _client;
        private DateTime _tokenCreated;

        public string Client
        {
            get => _client;

            set
            {
                try
                {
                    _httpClient = new HttpClient { BaseAddress = new Uri($"http://{value}/v1/") };
                    _client = value;
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
        public record ApikeyLoginRecord(string key);

        private bool TokenExpired()
        {
            var expired = (DateTime.Now - _tokenCreated).TotalMinutes > 30;
            return expired;
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            if (_token == null || TokenExpired())
            {
                await GetAccessTokenAsync();
            }

            TokenExpired();
            

            // add jwt auth header if user is logged in and request is to the api url
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Bearer);

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


        private async Task<bool> GetAccessTokenAsync()
        {
            try
            {
                var httpClient = new HttpClient { BaseAddress = new Uri($"http://{_client}/v1/") };

                var response = await httpClient.PostAsJsonAsync("authentication/Login", new ApikeyLoginRecord(Apikey));
                if (response.IsSuccessStatusCode)
                {
                    var token = response.Content.ReadFromJsonAsync<BearerToken>().Result;

                    if (token == null)
                        return false;

                    var jwtToken = new JwtSecurityToken(token.Bearer);
                    _token = new JwtToken { Bearer = token.Bearer, ValidTo = jwtToken.ValidTo.ToLongTimeString(), Claims = jwtToken.Claims.Select(p => new JwtToken.ClaimRecord(p.Type, p.Value)).ToList() };

                    _tokenCreated = DateTime.Now;
                    return true;

                }
            }
            catch (Exception e)
            {
               
            }
           

            return false;
        }

    }
}