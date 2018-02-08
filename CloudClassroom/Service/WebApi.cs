using CloudClassroom.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            string requestUrl = $"/api/token/token?username={userName}&password={password}";
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
            string requestUrl = "/api/user/userinfo";
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
            string requestUrl = "/api/user/schoolinfo";
            ResponseModel response = await Request(requestUrl);

            SchoolModel school = null;

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;

                    if (data != null)
                    {
                        school = JsonConvert.DeserializeObject<SchoolModel>(data.SelectToken("info").ToString());
                    }
                }
            }

            return school;
        }

        public async Task<IList<LessonModel>> GetWeeklyLessons()
        {
            string requestUrl = "/api/lesson/week/lessons";
            ResponseModel response = await Request(requestUrl);

            IList<LessonModel> lessons = null;

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;

                    if (data != null)
                    {
                        lessons = JsonConvert.DeserializeObject<IList<LessonModel>>(data.SelectToken("items").ToString());
                    }
                }
            }

            return lessons;
        }

        public async Task<IList<UserModel>> GetLessonAttendees(LessonType lessonType, int lessonId)
        {
            string requestUrl = $"/api/lesson/users/{lessonType}/{lessonId}";

            ResponseModel response = await Request(requestUrl);

            IList<UserModel> attendees = null;

            if (response.Status == 0)
            {
                if (response.Data !=null)
                {
                    JObject data = response.Data as JObject;

                    if (data !=null)
                    {
                        attendees = JsonConvert.DeserializeObject<IList<UserModel>>(data.SelectToken("users").ToString());
                    }
                }
            }

            return attendees;
        }

        public async Task<ZoomInfoModel> GetZoomInfo()
        {
            string requestUrl = "/api/zoom/zoominfo";
            ResponseModel response = await Request(requestUrl);

            ZoomInfoModel zoomInfo = null;

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;

                    if (data != null)
                    {
                        zoomInfo = JsonConvert.DeserializeObject<ZoomInfoModel>(data.SelectToken("info").ToString());
                    }
                }
            }

            return zoomInfo;
        }

        public async Task<IList<string>> GetLessonTypes()
        {
            string requestUrl = "/api/lesson/types";

            ResponseModel response = await Request(requestUrl);

            IList<string> lessonTypes = null;

            if (response.Status == 0)
            {
                if (response.Data != null)
                {
                    JObject data = response.Data as JObject;

                    if (data != null)
                    {
                        lessonTypes = JsonConvert.DeserializeObject<IList<string>>(data.SelectToken("types").ToString());
                    }
                }
            }

            return lessonTypes;
        }
    }
}
