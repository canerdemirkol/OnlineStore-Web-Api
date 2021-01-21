using OnlineStore.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OnlineStore.ServiceHost.API.Handlers
{
    public class ApiResponseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        //
        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content = null;
            string errorMessage = string.Empty;

            ValidateApiResponse(response, ref content, ref errorMessage);


            //yeni bir response oluşturma
            //yeni headerlar yüklendi (eski header ler i yukledik)
            var newResponse = CreateHttpResponseMessage(request, response, content, errorMessage);

            foreach (var loopHeader in response.Headers)
            {
                newResponse.Headers.Add(loopHeader.Key, loopHeader.Value);
            }

            return newResponse;
        }

        //validate işlemi yapılıyor
        //eger bir hata oluştuysa onu yakala
        //her methodun kendi işini üslenmesi nin kontrolü
        private void ValidateApiResponse(HttpResponseMessage response, ref object content, ref string errorMessage)
        {
            //content deki degeri almak için TryGetContentValue
            //herhangi bir aksi durum varmı yokmu kontrolü
            //herhangi bir hata oluştuysa onları errorMessage e aktar
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    StringBuilder sb = new StringBuilder();

                    foreach (var loopError in error)
                    {
                        sb.Append(string.Format("{0} {1}", loopError.Key, loopError.Value));
                    }

                    errorMessage = sb.ToString();
                }
            }
        }

        private HttpResponseMessage CreateHttpResponseMessage<T>(HttpRequestMessage request, HttpResponseMessage response, T content, string errorMessage)
        {
            return request.CreateResponse(response.StatusCode,
                new ApiResponse<T>(response.StatusCode, content, errorMessage));
        }

    }
}