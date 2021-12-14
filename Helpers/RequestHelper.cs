using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkController.Common.Helper;

namespace WorkControllerAdmin.Http.Helper
{
    public static class RequestHelper
    {
        private static async Task<HttpContent> GetNewHttpContent(IRequest request)
        {
            // Serialize our concrete class into a JSON String
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(request));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            return new StringContent(stringPayload, Encoding.UTF8, "application/json");
        }
        public static async Task<HttpResponseMessage> SendPostRequest(string URI, IHttpClientFactory _clientFactory, IRequest requestModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, URI);
            var client = _clientFactory.CreateClient("WorkController");
            HttpContent content = await RequestHelper.GetNewHttpContent(requestModel);
            return await client.PostAsync(request.RequestUri,content );
        }

        public static async Task<HttpResponseMessage> SendPostAuthRequest(string URI, IHttpClientFactory _clientFactory,string token, IRequest requestModel=null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, URI);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var client = _clientFactory.CreateClient("WorkController");
            //client.DefaultRequestHeaders.Authorization =
            //new AuthenticationHeaderValue("Bearer", token);
            HttpContent content = null;
            if (requestModel != null)
                content = await RequestHelper.GetNewHttpContent(requestModel);
            var r = await client.PostAsync(request.RequestUri, content);
            return r;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}
