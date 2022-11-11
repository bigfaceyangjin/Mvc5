using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using DBHelper.Entitys;

namespace DBHelper
{
    public static class WebApiClient<T>
    {
        public static List<T> GetAll(string url)
        {
            List<T> li = new List<T>();
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all products.
            // 列出所有产品
            HttpResponseMessage response = client.GetAsync(url).Result;// Blocking call（阻塞调用）! 
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                // 解析响应体。阻塞！
                li = response.Content.ReadAsAsync<List<T>>().Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return li;
        }

        public static T GetByFilter(string url)
        {
            T entity = default(T);
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all products.
            // 列出所有产品
            HttpResponseMessage response = client.GetAsync(url).Result;// Blocking call（阻塞调用）! 
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                // 解析响应体。阻塞！
                entity = response.Content.ReadAsAsync<T>().Result;
            }
            return entity;
        }

        public static T Get(string url,string id)
        {
            T entity=default(T);
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // List all products.
            // 列出所有产品
            HttpResponseMessage response = client.GetAsync(string.Format("{0}/{1}",url,id)).Result;// Blocking call（阻塞调用）! 
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                // 解析响应体。阻塞！
                entity = response.Content.ReadAsAsync<T>().Result;
            }
            return entity;
        }

        public static bool Edit(string url,List<int> value)
        {         
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PutAsJsonAsync(url,value).Result;
            if (response.IsSuccessStatusCode)
            {              
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<TI> GetList<TI>(string url, List<int> value)
        {
            List<TI> list = new List<TI>();
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PostAsJsonAsync(url, value).Result;
            if (response.IsSuccessStatusCode)
            {
                list = response.Content.ReadAsAsync<List<TI>>().Result;                
            }
            else
            {
                list = new List<TI>();
            }
            return list;
        }
    }
}
