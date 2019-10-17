using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Extensions.Camunda;
using VXDesign.Store.DevTools.Common.Services.HTTP;
using HttpMethod = VXDesign.Store.DevTools.Common.Enums.HTTP.HttpMethod;

namespace VXDesign.Store.DevTools.Common.Services.Syrinx
{
    public interface ISyrinxCamundaClientService
    {
        Task<TResponseModel> Send<TRequestModel, TResponseModel>(TRequestModel request) where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new();
    }

    public class SyrinxCamundaClientService : ISyrinxCamundaClientService
    {
        private readonly SyrinxProperties properties;

        public SyrinxCamundaClientService(SyrinxProperties properties)
        {
            this.properties = properties;
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(properties.Host) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async Task<TResponseModel> Send<TRequestModel, TResponseModel>(TRequestModel request) where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new()
        {
            var service = new InternalSyrinxClientService<TRequestModel, TResponseModel>(properties, GetHttpClient);
            return await service.Send(request);
        }
    }

    internal interface IInternalSyrinxClientService<in TRequestModel, TResponseModel> : IHttpClientService<TRequestModel, TResponseModel>
        where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new()
    {
    }

    internal class InternalSyrinxClientService<TRequestModel, TResponseModel> : HttpClientService<TRequestModel, TResponseModel>, IInternalSyrinxClientService<TRequestModel, TResponseModel>
        where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new()
    {
        private readonly SyrinxProperties properties;
        private readonly Func<HttpClient> httpClientGetter;

        internal InternalSyrinxClientService(SyrinxProperties properties, Func<HttpClient> httpClientGetter)
        {
            this.properties = properties;
            this.httpClientGetter = httpClientGetter;
        }

        protected override HttpClient Initialize() => httpClientGetter();

        public override async Task<TResponseModel> Send(TRequestModel request) => (await Send(properties.Api, properties.CamundaRequestEndpoint, HttpMethod.Post, request)).PostHandle();
    }
}