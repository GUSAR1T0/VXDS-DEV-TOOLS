using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Core.HTTP
{
    public interface IHttpClientService<in TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse, new()
    {
        Task<TResponse> Send(IOperation operation, TRequest request);
    }

    public abstract class HttpClientService<TRequest, TResponse> : IHttpClientService<TRequest, TResponse> where TRequest : IRequest where TResponse : IResponse, new()
    {
        private static readonly MediaTypeHeaderValue MediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");

        private HttpClient httpClient;

        protected HttpClient HttpClient => httpClient ??= Initialize();

        protected abstract HttpClient Initialize();

        public abstract Task<TResponse> Send(IOperation operation, TRequest request);

        protected async Task<TResponse> Send(IOperation operation, string api, string path, HttpMethod method, TRequest request)
        {
            var fullRequestEndpoint = CreateFullRequestEndpoint(api, path, request.Path, request.Query);

            HttpResponseMessage response;
            switch (method)
            {
                case HttpMethod.Get:
                    response = await HttpClient.GetAsync(fullRequestEndpoint);
                    break;
                case HttpMethod.Post:
                    response = await HttpClient.PostAsync(fullRequestEndpoint, GetStringContentOfRequestBody(request.Body));
                    break;
                case HttpMethod.PostFile:
                    response = await HttpClient.PostAsync(fullRequestEndpoint, GetFileContentOfRequestBody(request.Body, request.Resources));
                    break;
                case HttpMethod.Put:
                    response = await HttpClient.PutAsync(fullRequestEndpoint, GetStringContentOfRequestBody(request.Body));
                    break;
                case HttpMethod.Delete:
                    response = await HttpClient.DeleteAsync(fullRequestEndpoint);
                    break;
                case HttpMethod.Options:
                    response = await HttpClient.SendAsync(new HttpRequestMessage(System.Net.Http.HttpMethod.Options, fullRequestEndpoint));
                    break;
                default:
                    return default;
            }

            return new TResponse
            {
                Status = (int) response.StatusCode,
                Output = await response.Content.ReadAsStringAsync(),
                Reason = response.ReasonPhrase
            };
        }

        private static string CreateFullRequestEndpoint(string api, string path, Dictionary<string, string> parameters, Dictionary<string, string> query)
        {
            api = api.Trim('/');
            path = path.Trim('/');
            FormatRequestPathWithParameters(ref path, parameters);
            FormatRequestPathWithQuery(ref path, query);
            return (!string.IsNullOrWhiteSpace(api) ? '/' + api : string.Empty) + '/' + path;
        }

        private static void FormatRequestPathWithParameters(ref string path, Dictionary<string, string> parameters)
        {
            if (parameters?.Any() == true)
            {
                path = string.Join("", path.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries).Select((item, index) => index % 2 == 1 ? parameters[item] : item));
            }
        }

        private static void FormatRequestPathWithQuery(ref string path, Dictionary<string, string> query)
        {
            if (query?.Any() == true)
            {
                path += '?' + string.Join('&', query.Select(item => $"{item.Key.ToCamelCase()}={item.Value}"));
            }
        }

        private static StringContent GetStringContentOfRequestBody(string body)
        {
            var content = new StringContent(body);
            content.Headers.ContentType = MediaTypeHeaderValue;
            return content;
        }

        private static MultipartFormDataContent GetFileContentOfRequestBody(string body, IReadOnlyList<LocalFile> resources)
        {
            var content = new MultipartFormDataContent();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);

            foreach (var (key, value) in dictionary)
            {
                content.Add(new StringContent(value), key);
            }

            foreach (var resource in resources)
            {
                content.Add(new StreamContent(resource.Stream), resource.Name, resource.FileName);
            }

            return content;
        }
    }
}