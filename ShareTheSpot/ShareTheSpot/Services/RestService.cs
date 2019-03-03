using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShareTheSpot.Helpers;
using ShareTheSpot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShareTheSpot.Services
{
    public class RestService
    {
        HttpClient client;
        string grant_type = "password";

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded' "));
        }

        /* Here user can communicate with db unauthorized.
         * We will adopt these methods using custom authorization measures,
         * till our app get connected with a Rest API. */

        //================ GET request -> returns data (read permission) ==================
        public async Task<List<KeyValuePair<string, User>>> GetUsers()
        {
            var uri = new Uri(string.Format(Constants.LoginUrl, string.Empty));
            var response = await client.GetAsync(uri);

            var users = new List<KeyValuePair<string, User>>();

            if (response.IsSuccessStatusCode)
            {
                string token = null;
                User user = null;
                var content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(content);

                //JArray inner = JArray.Parse(content);
                //JArray inner = entireJson["nameOfArray"].Value<JArray>();

                foreach (var itemDynamic in data)
                {
                    token = ((JProperty)itemDynamic).Name;
                    user = JsonConvert.DeserializeObject<User>(((JProperty)itemDynamic).Value.ToString());
                    users.Add(new KeyValuePair<string, User>(token, user));

                    //JProperty questionAnswerDetails = itemDynamic.Value<JProperty>(); Gets the value {"token1":{"key1":"value1", "key2":"value2"}, "token2":{"key1":"value3", "key2":"value4"}}
                    //var questionAnswerSchemaReference = questionAnswerDetails.Name; Gets the token {"token1":{"key1":"value1", "key2":"value2"}, "token2":{"key1":"value3", "key2":"value4"}}
                    //var propertyList = (JObject)itemDynamic[questionAnswerSchemaReference]; Gets the array of token's reference. It's useless in our case!

                    /* We use for-loop if have nested array lists in references
                     * i.e. {"reference":["key":"value", "key":"value"]} 
                     * 
                     foreach (var property in propertyList)
                     {

                     }*/

                    /*token = questionAnswerSchemaReference;
                    user = JsonConvert.DeserializeObject<User>(questionAnswerDetails.Value.ToString());

                    users.Add(new KeyValuePair<string, User>(token, user));*/
                }
            }
            return users;
        }

        //================ POST request -> registers data (read/write permissions) ==================
        public async Task<string> SaveUserAsync(User user, bool isNewItem = false)
        {
            var uri = new Uri(string.Format(Constants.LoginUrl, string.Empty));

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            if (isNewItem)
            {
                response = await client.PostAsync(uri, content);
            }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"                User successfully saved.");
            }

            JObject obj = null;

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                obj = JObject.Parse(data);
            }

            return obj["name"].ToString();
        }

        //================ UPDATE request -> updates data (read/write/change permissions) ==================
        public async Task UpdateUserAsync(User user, bool isNewItem = false)
        {
            var uri = new Uri(string.Format(Constants.LoginUrl, string.Empty));

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            if (isNewItem)
            {
                response = await client.PutAsync(uri, content);
            }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"                User successfully updated.");
            }
        }

        //================ DELETE request -> erases data (read/write/change permissions) ==================
        public async Task DeleteUserAsync(string id)
        {
            var uri = new Uri(string.Format(Constants.LoginUrl, id));

            var response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"                User successfully deleted.");
            }
        }

        /* Here user authorization is getting started.
         * This doesn't work if we haven't connected our application with
         * a web Rest API */

        //================ User sends its own data to our server ==================
        public async Task<Token> Login(User user)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type));
            postData.Add(new KeyValuePair<string, string>("username", user.Username));
            postData.Add(new KeyValuePair<string, string>("password", user.Password));
            var content = new FormUrlEncodedContent(postData);
            Token response = await PostResponseLogin<Token>(Constants.LoginUrl, content);
            Debug.WriteLine(@"                . " + response.ToString());
            DateTime dt = new DateTime();
            dt = DateTime.Today;
            response.expire_date = dt.AddSeconds(response.expire_in);
            return response;
        }

        //================ We'll get a response in which can be found auth token ==================
        public async Task<T> PostResponseLogin<T>(string weburl, FormUrlEncodedContent content) where T : class
        {
            var response = await client.PostAsync(weburl, content);
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonConvert.DeserializeObject<T>(jsonResult);
            return responseObject;
        }

        /* From now on out, we can make GET / POST / UPDATE / DELETE 
         * requests having authorized signature of our user */

        //================ [Auth] GET request -> returns data (read permission) ==================
        public async Task<T> GetRequest<T>(string weburl) where T : class
        {
            //var Token = App.TokenDatabase.GetToken();
            var Token = Settings.AccessToken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var response = await client.GetAsync(weburl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return contentResp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        //================ [Auth] POST request -> registers data (read/write permissions) ==================
        public async Task<T> PostRequest<T>(string weburl, string jsonstring) where T : class
        {
            //var Token = App.TokenDatabase.GetToken();
            var Token = Settings.AccessToken;
            string ContentType = "application/json";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var Result = await client.PostAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                if (Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = Result.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return contentResp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        //================ [Auth] UPDATE request -> updates data (read/write/change permissions) ==================
        public async Task<T> UpdateRequest<T>(string weburl, string jsonstring) where T : class
        {
            //var Token = App.TokenDatabase.GetToken();
            var Token = Settings.AccessToken;
            string ContentType = "application/json";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var Result = await client.PutAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                if (Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = Result.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return contentResp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        //================ [Auth] DELETE request -> erases data (read/write/change permissions) ==================
        public async Task<T> DeleteRequest<T>(string weburl, int id) where T : class
        {
            //var Token = App.TokenDatabase.GetToken();
            var Token = Settings.AccessToken;
            var uri = new Uri(string.Format(weburl, id));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var response = await client.DeleteAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return contentResp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
