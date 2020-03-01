using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.HTTP;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public interface ICamundaServerService : IHttpClientService<CamundaRequest, CamundaResponse>
    {
    }

    public class CamundaServerService : HttpClientService<CamundaRequest, CamundaResponse>, ICamundaServerService
    {
        private readonly CamundaProperties properties;

        public CamundaServerService(CamundaProperties properties)
        {
            this.properties = properties;
        }

        protected override HttpClient Initialize()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(properties.Host) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(properties.Login) && !string.IsNullOrWhiteSpace(properties.Password))
            {
                httpClient.DefaultRequestHeaders.Add("Authentication", $"Basic {BasicAuth()}");
            }

            return httpClient;
        }

        private string BasicAuth() => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{properties.Login}:{properties.Password}"));

        public override async Task<CamundaResponse> Send(IOperation operation, CamundaRequest request) => await Send(operation, properties.Api, request.Endpoint.Path, request.Endpoint.Method, request);
    }
}