using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.SRS.Camunda.Entities.API;
using HttpMethod = VXDesign.Store.DevTools.Common.Entities.Enums.HttpMethod;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public interface ICamundaService
    {
        Task<CamundaResponse> Send(CamundaEndpoint endpoint, string parameters);
    }
    
    public class CamundaService : ICamundaService
    {
        private static readonly MediaTypeHeaderValue MediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");

        private readonly CamundaProperties properties;
        private HttpClient httpClient;

        public CamundaService(CamundaProperties properties)
        {
            this.properties = properties;
        }

        private HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient { BaseAddress = new Uri(properties.Host) };
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!string.IsNullOrWhiteSpace(properties.Login) && !string.IsNullOrWhiteSpace(properties.Password))
                    {
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {BasicAuth()}");
                    }
                }

                return httpClient;
            }
        }

        private string BasicAuth() => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{properties.Login}:{properties.Password}"));

        public async Task<CamundaResponse> Send(CamundaEndpoint endpoint, string parameters)
        {
            HttpResponseMessage response;

            switch (endpoint.Method)
            {
                case HttpMethod.Get:
                    response = await HttpClient.GetAsync(CreateFullRequestEndpoint(endpoint.Path));
                    break;
                case HttpMethod.Post:
                    response = await HttpClient.PostAsync(CreateFullRequestEndpoint(endpoint.Path), GetStringContentOfRequestBody(parameters));
                    break;
                case HttpMethod.Put:
                    response = await HttpClient.PutAsync(CreateFullRequestEndpoint(endpoint.Path), GetStringContentOfRequestBody(parameters));
                    break;
                case HttpMethod.Delete:
                    response = await HttpClient.DeleteAsync(CreateFullRequestEndpoint(endpoint.Path));
                    break;
                case HttpMethod.Options:
                    response = await HttpClient.SendAsync(new HttpRequestMessage(System.Net.Http.HttpMethod.Options, endpoint.Path));
                    break;
                default:
                    return null;
            }

            return new CamundaResponse
            {
                Status = (int) response.StatusCode,
                Result = await response.Content.ReadAsStringAsync()
            };
        }

        private string CreateFullRequestEndpoint(string path) => properties.Api + path;

        private static StringContent GetStringContentOfRequestBody(string parameters)
        {
            var str = new StringContent(parameters);
            str.Headers.ContentType = MediaTypeHeaderValue;
            return str;
        }
    }
}