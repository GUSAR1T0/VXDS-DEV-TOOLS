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
        private readonly bool skipCertificateValidation;

        public SyrinxAuthenticationClientService(SyrinxProperties properties, bool skipCertificateValidation = false)
        {
            this.properties = properties;
            this.skipCertificateValidation = skipCertificateValidation;
        }

        private void SetupHeaders(VerifyAuthenticationRequest request)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Authorization", request.Token);
        }

        protected override HttpClient Initialize()
        {
            HttpClient httpClient;
            if (skipCertificateValidation)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(properties.Host.Internal) };
            }
            else
            {
                httpClient = new HttpClient { BaseAddress = new Uri(properties.Host.Internal) };
            }

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