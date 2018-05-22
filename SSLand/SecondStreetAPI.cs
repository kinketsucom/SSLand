﻿using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSLand {

    public class SecondStreetAPI{

        private const string USER_AGENT = "Mozilla/5.0 (iPad; COU OS 10_3_2 like Mac OS X) AppleWebKit/603.2.4 (KHTML, like Gecko) Mobile/14F89 Fril/6.7.1";
        private string proxy;
        private class SecondStreetRawResponse
        {
            public bool error = true;
            public string response = "";
        }

        public Common.Account account;
        public SecondStreetAPI(string email, string password)
        {
            this.account = new Common.Account();
            this.account.email = email;
            this.account.password = password;
        }

        public SecondStreetAPI(Common.Account account){
            this.account = account;
        }


        //新着商品を取得する
        public List<SecondStreetListItem> getNewItems(int count = 30, string max_id = "")
        {
            List<SecondStreetListItem> rst = new List<SecondStreetListItem>();
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("auth_token", this.account.fril_auth_token);
            param.Add("count", count.ToString());
            //if (string.IsNullOrEmpty(max_id) == false) param.Add("max_id", max_id);
            string url = "http://api.fril.jp/api/v3/timelines/current";
            SecondStreetRawResponse rawres = getFrilAPI(url, param);
            if (rawres.error)
            {
                Log.Logger.Error(string.Format("フリルタイムライン取得失敗"));
                return null;
            }
            dynamic resjson = DynamicJson.Parse(rawres.response);
            try
            {
                foreach (var itemjson in resjson.items)
                {
                    rst.Add(new SecondStreetListItem(itemjson));
                }
                Console.WriteLine("start: " + rst[0].item_id + " end: " + rst[rst.Count - 1].item_id);
                return rst;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("フリルタイムラインjsonパース失敗");
                return rst;
            }
        }
        


        //FrilAPIをGETでたたく
        private SecondStreetRawResponse getFrilAPI(string url, Dictionary<string, string> param)
        {
            SecondStreetRawResponse res = new SecondStreetRawResponse();
            try
            {
                //url = Uri.EscapeUriString(url);//日本語などを％エンコードする
                //パラメータをURLに付加 ?param1=val1&param2=val2...
                url += "?";
                List<string> paramstr = new List<string>();
                foreach (KeyValuePair<string, string> p in param)
                {
                    string k = Uri.EscapeDataString(p.Key);
                    string v = Uri.EscapeDataString(p.Value);
                    paramstr.Add(k + "=" + v);
                }
                url += string.Join("&", paramstr);
                //HttpWebRequestの作成
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = this.cc;
                req.UserAgent = SecondStreetAPI.USER_AGENT;
                req.Method = "GET";
                //プロキシの設定
                if (string.IsNullOrEmpty(this.proxy) == false)
                {
                    System.Net.WebProxy proxy = new System.Net.WebProxy(this.proxy);
                    req.Proxy = proxy;
                }
                //結果取得
                string content = "";
                var task = Task.Factory.StartNew(() => executeGetRequest(req));
                task.Wait(10000);
                if (task.IsCompleted)
                    content = task.Result;
                else
                    throw new Exception("Timed out");
                if (string.IsNullOrEmpty(content)) throw new Exception("webrequest error");
                res.error = false;
                res.response = content;
                Log.Logger.Info("SecondStreetGETリクエスト成功");
                return res;
            }
            catch (Exception e)
            {
                Log.Logger.Error("SecondStreetGETリクエスト失敗");
                return res;
            }
        }
        private string executeGetRequest(HttpWebRequest req)
        {
            try
            {
                HttpWebResponse webres = (HttpWebResponse)req.GetResponse();
                Stream s = webres.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                string content = sr.ReadToEnd();
                return content;
            }
            catch
            {
                return "";
            }
        }
        private string executePostRequest(HttpWebRequest req, byte[] bytes)
        {
            try
            {
                using (Stream requestStream = req.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                //結果取得
                string result = "";
                using (Stream responseStream = req.GetResponse().GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        private CookieContainer cc = new CookieContainer();
        private string postMultipartFril(string url, Dictionary<string, string> param, string file) {
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            string text = Environment.TickCount.ToString();
            byte[] bytes = encoding.GetBytes("\r\n");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.UserAgent = USER_AGENT;
            httpWebRequest.CookieContainer = this.cc;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + text;
            string text2 = "";
            foreach (KeyValuePair<string, string> current in param) {
                text2 = string.Concat(new string[]
				{   
					text2,
					"--",
					text,
					"\r\nContent-Disposition: form-data; name=\"",
					current.Key,
					"\"\r\n\r\n",
					current.Value,
					"\r\n"
				});
            }
            long num = 0L;
            Path.GetFileName(file);
            string s = "--" + text + "\r\nContent-Disposition: form-data; name=\"image\"; filename=\"image.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n";
            byte[] bytes2 = encoding.GetBytes(s);
            num += (long)encoding.GetBytes(s).Length + new FileInfo(file).Length;
            byte[] bytes3 = encoding.GetBytes(text2);
            byte[] bytes4 = encoding.GetBytes("--" + text + "--\r\n");
            httpWebRequest.ContentLength = (long)bytes3.Length + num + (long)bytes.Length + (long)bytes4.Length;
            string result;
            using (Stream requestStream = httpWebRequest.GetRequestStream()) {
                requestStream.Write(bytes3, 0, bytes3.Length);
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read)) {
                    requestStream.Write(bytes2, 0, bytes2.Length);
                    byte[] array = new byte[4096];
                    while (true) {
                        int num2 = fileStream.Read(array, 0, array.Length);
                        if (num2 == 0) {
                            break;
                        }
                        requestStream.Write(array, 0, num2);
                    }
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                requestStream.Write(bytes4, 0, bytes4.Length);
                WebResponse webResponse = null;
                string text3 = "";
                try {
                    webResponse = httpWebRequest.GetResponse();
                    Log.Logger.Info("access info:url->" + url);
                } catch (WebException ex) {
                    webResponse = (HttpWebResponse)ex.Response;
                    Log.Logger.Info("access info:url->" + url + " message->" + ex.Message);
                } finally {
                    using (Stream responseStream = webResponse.GetResponseStream()) {
                        using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"))) {
                            text3 = streamReader.ReadToEnd();
                        }
                    }
                }
                result = text3;
            }
            return result;
        }

        }
    }
