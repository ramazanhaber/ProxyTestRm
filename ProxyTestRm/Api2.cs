using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
namespace ProxyTestRm
{
    public class Api2
    {
        public HttpClient client;
        public string username = "";
        public string password = "";

        public Api2(string ip, string port, string username, string sifre)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            string proxyHost = ip, proxyPort = port, proxyUserName = username, proxyPassword = sifre;
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = true,
                // *** These creds are given to the proxy server, not the web server ***
                Credentials = new NetworkCredential(
        userName: proxyUserName,
        password: proxyPassword)
            };
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
                //UseCookies = true,
                //CookieContainer = cookies,
                //UseDefaultCredentials=true,
            };
            client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
        }
        public Api2(string ip, string port)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            string proxyHost = ip, proxyPort = port;
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                // *** These creds are given to the proxy server, not the web server ***
            };
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
                //UseCookies = true,
                //CookieContainer = cookies,
                //UseDefaultCredentials=true,
            };
            client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
        }

        public Api2()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
        }
        public void apiSifirla()
        {
            return;
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
        }
        public string requestPost(string url, Dictionary<string, string> dict)
        {
            apiSifirla();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var Content = new FormUrlEncodedContent(dict);
            HttpResponseMessage responseOtel = client.PostAsync(url, Content).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            return result;
        }
        public string requestPostJson(string url, object model)
        {
            apiSifirla();
            string json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseOtel = client.PostAsync(url, stringContent).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            return result;
        }
        public string requestPostJsonString(string url, string json)
        {
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseOtel = client.PostAsync(url, stringContent).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            return result;
        }
        public string requestGet(string url)
        {
            apiSifirla();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            HttpResponseMessage responseOtel = client.GetAsync(url).Result;
            string result = responseOtel.Content.ReadAsStringAsync().Result;
            result = System.Net.WebUtility.HtmlDecode(result);
            return result;
        }
        public string requestGetBase64(string apiKey, int carId)
        {
            string url = "resimurl";
            HttpResponseMessage response = client.GetAsync(url).Result;
            var result = response.Content.ReadAsByteArrayAsync().Result;
            return Convert.ToBase64String(result);
        }
        public string kes(string txt, string ilk, string son)
        {
            try
            {
                string parcali1 = (txt.Split(new string[] { ilk }, StringSplitOptions.None))[1];
                string parcali2 = (parcali1.Split(new string[] { son }, StringSplitOptions.None))[0];
                return parcali2.Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }
        public List<string> kesList(string txt, string ilk, string son)
        {
            try
            {
                List<string> list = new List<string>();
                string[] parcali1 = (txt.Split(new string[] { ilk }, StringSplitOptions.None));
                for (int i = 1; i < parcali1.Length; i++)
                {
                    string parcali2 = (parcali1[i].Split(new string[] { son }, StringSplitOptions.None))[0];
                    list.Add(parcali2.Trim());
                }
                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string[] splits(string text, string split)
        {
            try
            {
                return text.Split(new string[] { split }, StringSplitOptions.None);
            }
            catch (Exception)
            {
            }
            return null;
        }
        List<string> otelKodList = new List<string>();
        public bool loginOl()
        {
            apiSifirla();
            otelKodList = new List<string>();
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("program", "html002");
                dict.Add("login", username);
                dict.Add("rolehidden", "HOTELLOCAL");
                dict.Add("password", password);
                dict.Add("language", "E");
                dict.Add("verzend", "Login");
                string htmlText = requestPost("https://www.jil.travel/", dict);
                string otelKod = kes(htmlText, "/generalproductid/", "\"");
                if (otelKod.Contains("/")) otelKod = otelKod.Split('/')[0].Trim();
                if (otelKod == "" || htmlText == "") return false;
                otelKodList.Add(otelKod);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
