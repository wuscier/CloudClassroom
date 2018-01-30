using CloudClassroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CloudClassroom.Service
{
    public class WebApi
    {
        public Uri BaseAddress { get; private set; }
        public AuthenticationHeaderValue AuthHeaderValue { get; private set; }

        private WebApi()
        {
            BaseAddress = new Uri("http://202.85.212.211:9001");
            AuthHeaderValue = new AuthenticationHeaderValue("Bearer");
        }

        public static readonly WebApi Instance = new WebApi();

        public async Task<ResponseModel> Request(string url, HttpContent content = null)
        {
            ResponseModel response = new ResponseModel();

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = BaseAddress;
                httpClient.DefaultRequestHeaders.Authorization = AuthHeaderValue;

                HttpResponseMessage responseMessage = null;

                try
                {
                    responseMessage = content == null ? await httpClient.GetAsync(url) : await httpClient.PostAsync(url, content);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string responseContent = await responseMessage.Content.ReadAsStringAsync();


                    }
                    else
                    {
                        // logout if unauthorized
                        response.Status = (int)responseMessage.StatusCode;
                        response.Message = responseMessage.ReasonPhrase;
                    }
                }
                catch (Exception ex)
                {
                    response.Status = -1;
                    response.Message = ex.Message;
                }

                return response;
            }
        }
    }
}
