using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace FreightSynchro
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

        public static T Get(string url, string id)
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

        public static bool Edit(string url, List<int> value)
        {
            HttpClient client = new HttpClient();
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.PutAsJsonAsync(url, value).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /// <summary>
        /// Post 请求数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postValue"></param>
        /// <returns></returns>
        public static string Post(string url, T postValue)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            string xmlString = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, postValue);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            string rsString = "";
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Accept", "*/*");
                var sendData = new NameValueCollection();
                sendData["requestCalcXml"] = xmlString;
                var rs = webClient.UploadValues(url, "POST", sendData);
                rsString = Encoding.UTF8.GetString(rs);
            }

            return rsString;
        }
    }
}
