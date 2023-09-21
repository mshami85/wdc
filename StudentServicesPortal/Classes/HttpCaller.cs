using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace StudentServicesPortal.Classes
{
    public interface IHttpCaller
    {
        Task<bool> Delete(string url, object id);
        Task<IEnumerable<T>> Get<T>(string url, object? id = null) where T : class;
        Task<T> Find<T>(string url, object id) where T : class;
        Task<bool> Post<T>(string url, T model) where T : class;
        Task<bool> PostForm<T>(string url, T model, params string[] files) where T : class;
        Task<bool> Put<T>(string url, object id, T model) where T : class;
    }

    public class HttpCallerException : Exception
    {
        public int Code { get; set; }

        public HttpCallerException(int code, string message) : base(message)
        {
            Code = code;
        }

        public override string ToString() => $"Error Code: {Code}\n\rMessage: {Message}";
    }

    public class HttpCaller : IHttpCaller
    {
        HttpClient _client;
        AppSettings _settings;

        public HttpCaller(IOptionsSnapshot<AppSettings> option)
        {
            _settings = option.Value;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_settings.GatewayAddress)
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> Post<T>(string url, T model) where T : class
        {
            var response = await _client.PostAsJsonAsync(url, model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new HttpCallerException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<bool> Put<T>(string url, object id, T model) where T : class
        {
            var path = id == null ? url : url + "/" + id;
            var response = await _client.PutAsJsonAsync(path, model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new HttpCallerException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<bool> Delete(string url, object id)
        {
            var path = id == null ? url : url + "/" + id;
            var response = await _client.DeleteAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                throw new HttpCallerException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<bool> PostForm<T>(string url, T? model, params string[] files) where T : class
        {
            var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var fileContent = new StreamContent(File.OpenRead(file));
                content.Add(fileContent, "file", Path.GetFileName(file));
            }

            if (model != null)
            {
                var props = model.ExtractProperties();
                foreach (var prop in props)
                {
                    content.Add(new StringContent(prop.Value.ToString()), prop.Key);
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = response.StatusCode;
                var msg = await response.Content.ReadAsStringAsync();
                throw new HttpCallerException((int)error, msg);
            }
        }

        public async Task<bool> PostForm<T>(string url, T? model, params byte[][] files) where T : class
        {
            var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var fileContent = new StreamContent(files[0]);
                
                content.Add(fileContent, "file", Path.GetFileName(file));
            }

            if (model != null)
            {
                var props = model.ExtractProperties();
                foreach (var prop in props)
                {
                    content.Add(new StringContent(prop.Value.ToString()), prop.Key);
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));

            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var error = response.StatusCode;
                var msg = await response.Content.ReadAsStringAsync();
                throw new HttpCallerException((int)error, msg);
            }
        }

        public async Task<IEnumerable<T>> Get<T>(string url, object? id = null) where T : class
        {
            var path = id == null ? url : url + "/" + id;
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            }
            else
            {
                throw new HttpCallerException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }

        public async Task<T> Find<T>(string url, object id) where T : class
        {
            var path = url.TrimEnd('/') + "/" + id;
            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                throw new HttpCallerException((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
    }
}

