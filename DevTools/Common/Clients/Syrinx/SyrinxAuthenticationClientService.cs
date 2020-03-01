using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Syrinx.Base;
using VXDesign.Store.DevTools.Common.Core.HTTP;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using HttpMethod = VXDesign.Store.DevTools.Common.Core.HTTP.HttpMethod;

namespace VXDesign.Store.DevTools.Common.Clients.Syrinx
{
    public interface ISyrinxAuthenticationClientService : IHttpClientService<VerifyAuthenticationRequest, VerifyAuthenticationResponse>
    {
    }

    public class SyrinxAuthenticationClientService : HttpClientService<VerifyAuthenticationRequest, VerifyAuthenticationResponse>, ISyrinxAuthenticationClientService
    {
        private readonly SyrinxProperties properties;

        public SyrinxAuthenticationClientService(SyrinxProperties properties)
        {
            this.properties = properties;
        }

        private void SetupHeaders(VerifyAuthenticationRequest request)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Authorization", request.Token);
        }

        protected override HttpClient Initialize()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(properties.Host) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Clear();
            return httpClient;
        }

        public override async Task<VerifyAuthenticationResponse> Send(IOperation operation, VerifyAuthenticationRequest request)
        {
            SetupHeaders(request);
            return await Send(operation, properties.Api, properties.VerifyAuthenticationEndpoint, HttpMethod.Get, request);
        }
    }
}