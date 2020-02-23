using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Extensions.GitHub;
using VXDesign.Store.DevTools.Core.Services.HTTP;

namespace VXDesign.Store.DevTools.Core.Services.GitHub
{
    public interface IGitHubClientService
    {
        Task<TResponseModel> Send<TRequestModel, TResponseModel>(IOperation operation, TRequestModel request) where TRequestModel : IGitHubRequest where TResponseModel : IGitHubResponse, new();
    }

    public class GitHubClientService : IGitHubClientService
    {
        private readonly string token;

        public GitHubClientService(string token)
        {
            this.token = token;
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.github.com") };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "VXDS-DEV-TOOLS");
            return httpClient;
        }

        public async Task<TResponseModel> Send<TRequestModel, TResponseModel>(IOperation operation, TRequestModel request)
            where TRequestModel : IGitHubRequest where TResponseModel : IGitHubResponse, new()
        {
            var service = new InternalGitHubClientService<TRequestModel, TResponseModel>(GetHttpClient);
            return await service.Send(operation, request);
        }
    }

    internal interface IInternalGitHubClientService<in TRequestModel, TResponseModel> : IHttpClientService<TRequestModel, TResponseModel>
        where TRequestModel : IGitHubRequest where TResponseModel : IGitHubResponse, new()
    {
    }

    internal class InternalGitHubClientService<TRequestModel, TResponseModel> : HttpClientService<TRequestModel, TResponseModel>, IInternalGitHubClientService<TRequestModel, TResponseModel>
        where TRequestModel : IGitHubRequest where TResponseModel : IGitHubResponse, new()
    {
        private readonly Func<HttpClient> httpClientGetter;

        internal InternalGitHubClientService(Func<HttpClient> httpClientGetter)
        {
            this.httpClientGetter = httpClientGetter;
        }

        protected override HttpClient Initialize() => httpClientGetter();

        public override async Task<TResponseModel> Send(IOperation operation, TRequestModel request)
        {
            var endpoint = GitHubEndpoint.GetEndpoint(request.Endpoint) ?? throw CommonExceptions.GitHubEndpointIsNotFoundByEndpointCode(operation);
            return (await Send(operation, string.Empty, endpoint.Path, endpoint.Method, request)).PostHandle();
        }
    }
}