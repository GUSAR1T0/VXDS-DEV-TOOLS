using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Extensions;
using VXDesign.Store.DevTools.Common.Core.HTTP;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using HttpMethod = VXDesign.Store.DevTools.Common.Core.HTTP.HttpMethod;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda
{
    public interface ISyrinxCamundaClientService
    {
        Task<TResponseModel> Send<TRequestModel, TResponseModel>(IOperation operation, TRequestModel request) where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new();
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
            var httpClient = new HttpClient { BaseAddress = new Uri(properties.Host.Internal) };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async Task<TResponseModel> Send<TRequestModel, TResponseModel>(IOperation operation, TRequestModel request) where TRequestModel : ICamundaRequest where TResponseModel : ICamundaResponse, new()
        {
            var service = new InternalSyrinxClientService<TRequestModel, TResponseModel>(properties, GetHttpClient);
            return await service.Send(operation, request);
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

        public override async Task<TResponseModel> Send(IOperation operation, TRequestModel request)
        {
            return (await Send(operation, properties.Api, properties.CamundaRequestEndpoint, HttpMethod.PostFile, request)).PostHandle(operation);
        }
    }
}