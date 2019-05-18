using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace UserAudit.Web.Service
{
    public class UserService
    {
        public HttpClient Client { get; set; }
        public UserService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebApiUrl"].ToString());
        }

        public HttpResponseMessage GetResponseMessage(string url)
        {
            return Client.GetAsync(url, HttpCompletionOption.ResponseContentRead).Result;
        }

        public HttpResponseMessage Update(string url, object model)
        {
            return Client.PutAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage Insert(string url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }
        public HttpResponseMessage Delete(string url, object model)
        {
            return Client.DeleteAsync(url).Result;
        }
    }
}