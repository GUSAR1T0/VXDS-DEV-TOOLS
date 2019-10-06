using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.Containers.Syrinx.VerifyAuthentication;
using VXDesign.Store.DevTools.Common.Services.HTTP;
using HttpMethod = VXDesign.Store.DevTools.Common.Entities.Enums.HttpMethod;

namespace VXDesign.Store.DevTools.Common.Services.Syrinx
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

        public override async Task<VerifyAuthenticationResponse> Send(VerifyAuthenticationRequest request)
        {
            SetupHeaders(request);
            return await Send(properties.Api, properties.VerifyAuthenticationEndpoint, HttpMethod.Get, request);
        }
    }
}