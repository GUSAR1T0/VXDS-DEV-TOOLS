using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.Extensions.Camunda;
using VXDesign.Store.DevTools.Common.Services.HTTP;
using HttpMethod = VXDesign.Store.DevTools.Common.Entities.Enums.HttpMethod;

namespace VXDesign.Store.DevTools.Common.Services.Camunda
{
    public interface ICamundaClientService
    {
        Task<TResponseModel> Send<TRequestModel, TResponseModel>(TRequestModel request) where TRequestModel : ICamundaRequestModel where TResponseModel : ICamundaResponseModel, new();
    }

    public class CamundaClientService : ICamundaClientService
    {
        private readonly SyrinxProperties properties;

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(properties.Host) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public CamundaClientService(SyrinxProperties properties)
        {
            this.properties = properties;
        }

        public async Task<TResponseModel> Send<TRequestModel, TResponseModel>(TRequestModel request) where TRequestModel : ICamundaRequestModel where TResponseModel : ICamundaResponseModel, new()
        {
            var service = new InternalCamundaClientService<TRequestModel, TResponseModel>(properties, GetHttpClient);
            return await service.Send(request);
        }
    }

    internal interface IInternalCamundaClientService<in TRequestModel, TResponseModel> : IHttpClientService<TRequestModel, TResponseModel>
        where TRequestModel : ICamundaRequestModel where TResponseModel : ICamundaResponseModel, new()
    {
    }

    internal class InternalCamundaClientService<TRequestModel, TResponseModel> : HttpClientService<TRequestModel, TResponseModel>, IInternalCamundaClientService<TRequestModel, TResponseModel>
        where TRequestModel : ICamundaRequestModel where TResponseModel : ICamundaResponseModel, new()
    {
        private readonly SyrinxProperties properties;
        private readonly Func<HttpClient> httpClientGetter;

        internal InternalCamundaClientService(SyrinxProperties properties, Func<HttpClient> httpClientGetter)
        {
            this.properties = properties;
            this.httpClientGetter = httpClientGetter;
        }

        protected override HttpClient Initialize() => httpClientGetter();

        public override async Task<TResponseModel> Send(TRequestModel request) => (await Send(properties.Api, properties.Request, HttpMethod.Post, request)).PostHandle();
    }
}