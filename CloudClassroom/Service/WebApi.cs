using Classroom.Models;
using CloudClassroom.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

                        response = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
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


        public async Task<ResponseModel> ApplyToken(string userName, string password)
        {
            string requestUrl = $"/api/Token/token?username={userName}&password={password}";
            ResponseModel response = await Request(requestUrl);

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;
                    if (data != null)
                    {
                        AuthHeaderValue = new AuthenticationHeaderValue("Bearer", data.SelectToken("accessToken").ToString());
                    }
                }
            }

            return response;
        }

        public async Task<UserModel> GetUserInfo()
        {
            string requestUrl = "/api/User/userinfo";
            ResponseModel response = await Request(requestUrl);

            UserModel user = null;

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;

                    if (data != null)
                    {
                        user = JsonConvert.DeserializeObject<UserModel>(data.SelectToken("info").ToString());
                    }
                }
            }

            return user;
        }

        public async Task<SchoolModel> GetSchoolInfo()
        {
            string requestUrl = "/api/User/schoolinfo";
            ResponseModel response = await Request(requestUrl);

            SchoolModel school = null;

            if (response.Status == 0)
            {
                if (response.Data!=null)
                {
                    JObject data = response.Data as JObject;

                    if (data!=null)
                    {
                        school = JsonConvert.DeserializeObject<SchoolModel>(data.SelectToken("info").ToString());
                    }
                }
            }

            return school;
        }

    }
}
