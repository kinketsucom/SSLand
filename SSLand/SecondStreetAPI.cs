using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSLand {

    public class SecondStreetAPI{

        public const string USER_AGENT = "Mozilla/5.0 (iPad; COU OS 10_3_2 like Mac OS X) AppleWebKit/603.2.4 (KHTML, like Gecko) Mobile/14F89 Fril/6.7.1";
        private const string LOGIN_USER_AGENT = "reuse_store_release/3.0.5 CFNetwork/897.15 Darwin/17.5.0";
        public const string agent = "reuse_store_release/3.0.2 CFNetwork/811.5.4 Darwin/16.7.0";
        private string proxy;

        public SecondStreetAPI()
        {
        }

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

        //ログイン処理
        //ログインしても実はaccess_tokenとかは使用しない
        //ログイン後のクッキーだけが必要になる
        //購入前にログインを行う
        public bool trySecondStreetLogin(string email, string password) {
            string url = "https://auth.geonet.jp/authorize";
            string client_id = "f92a4aeb4d2d07e4b8471177246e7eac3f7bf287ff984c2f3219c18c71b9c302";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("response_type", "code");
            param.Add("redirect_uri", "https://www.2ndstreet.jp/index.php/api_2_0/appuserauth/login");
            param.Add("state", "state");
            param.Add("scope", "openid profile delivery geoinfo geomasterid");
            param.Add("client_id", client_id);
            //csrf取り出し
            var rawres = getSecondStreetAPI(url, param, SecondStreetAPI.LOGIN_USER_AGENT);
            var resjson = DynamicJson.Parse(rawres.response);
            string csrf_token = resjson.csrf_token;

            Dictionary<string, string> param2 = new Dictionary<string, string>();
            param2.Add("response_type", "code");
            param2.Add("redirect_uri", "https://www.2ndstreet.jp/index.php/api_2_0/appuserauth/login");
            param2.Add("client_id", client_id);
            param2.Add("csrf_token", csrf_token);
            param2.Add("user_action", "accept");
            param2.Add("state", "state");
            param2.Add("scope", "openid profile delivery geoinfo geomasterid");
            param2.Add("login_id", email);
            param2.Add("password", password);
            //302で自動的にリダイレクトする
            var rawres2 = postSecondStreetAPI(url, param2, SecondStreetAPI.LOGIN_USER_AGENT);
            try {
                var resjson2 = DynamicJson.Parse(rawres2.response);
                string access_token = resjson2.access_token;
                string refresh_token = resjson2.refresh_token;
                /*string token_type = resjson.token_type;
                int expires_in = (int)resjson.expires_in;
                string geo_id = resjson.geo_id;
                string nickname = resjson.nickname;
                string geo_master_id = resjson.geo_master_id;*/
                Log.Logger.Info("セカンドストリートログイン成功");
                return true;
            } catch (Exception ex) {
                Log.Logger.Error("セカンドストリートログイン失敗");
                return false;
            }
        }
        public void addItemToCartWeb(string shops_id, string goods_id) {
            string url2 = "https://www.2ndstreet.jp/cart/updateForApp";
            Dictionary<string, string> param2 = new Dictionary<string, string>();
            param2.Add("releaseurl", "1");
            param2.Add("shopsId", shops_id);
            param2.Add("goodsId", goods_id);
            param2.Add("num", "1");
            param2.Add("sp", "on");
            param2.Add("ver", "3.0.5");
            var rawres2 = getSecondStreetAPI(url2, param2, SecondStreetAPI.USER_AGENT);
            Console.WriteLine(rawres2.response);
        }
        public void getGoodsDetail(string goods_id, string shops_id) {
            /*string url = "https://www.2ndstreet.jp/index.php/api_2_0/AppMain/getGoodsDetail";
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("goods_id", goods_id);
            param.Add("shops_id", shops_id);
            var rawres = postSecondStreetAPI(url, param, SecondStreetAPI.LOGIN_USER_AGENT);
            Console.WriteLine(rawres.response);*/
        }
        //FIXIT:新着アイテムの取得
        public List<SecondStreetListItem> postNewItem()
        {
            //リクエスト部
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.2ndstreet.jp/index.php/api_2_0/AppMain/getNewGoods");
            req.Method = "POST";
            req.UserAgent = agent;
            HttpWebResponse rawres = (HttpWebResponse)req.GetResponse();
            Stream s = rawres.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            string content = sr.ReadToEnd();
            //パース部
            dynamic resjson = DynamicJson.Parse(content);
            List<SecondStreetListItem> rst = new List<SecondStreetListItem>();
            try
            {
                foreach (var itemjson in resjson.value)
                {
                    rst.Add(new SecondStreetListItem(itemjson));
                    //Console.WriteLine(itemjson);
                    //MainForm.label1.Text = "待機状態";
                }
                
                return rst;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("セカンドストリートタイムラインjsonパース失敗");
                //MainForm.label1.Text = "新着アイテム取得エラー";
                return rst;
            }
        }

        private SecondStreetRawResponse postSecondStreetAPI(string url, Dictionary<string, string> param, string UserAgent) {
            SecondStreetRawResponse res = new SecondStreetRawResponse();
            try {
                string text = "";
                List<string> paramstr = new List<string>();
                int num = 0;
                foreach (KeyValuePair<string, string> p in param) {
                    string k = Uri.EscapeDataString(p.Key);
                    string v = Uri.EscapeDataString(p.Value);
                    if (num != 0) text += "&";
                    text = text + (k + "=" + v);
                    num++;
                }
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = UserAgent;
                req.Method = "POST";
                //リクエストヘッダを付加
                req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                req.Accept = "gzip, deflate";
                req.ContentLength = (long)bytes.Length;
                //クッキーコンテナの追加
                req.CookieContainer = this.cc;
                //タイムアウト設定
                req.Timeout = 5000;
                //POST
                string content = "";
                var task = Task.Factory.StartNew(() => executePostRequest(ref req, bytes));
                task.Wait(10000);
                if (task.IsCompleted)
                    content = task.Result;
                else
                    throw new Exception("Timed out");
                if (string.IsNullOrEmpty(content)) throw new Exception("webrequest error");
                res.error = false;
                res.response = content;
                Log.Logger.Info("SecondStreetPOSTリクエスト成功");
                req.Abort();
                return res;
            } catch (Exception e) {
                return res;
                Log.Logger.Error("SecondStreetPOSTリクエスト失敗");
            }
        }

        //FrilAPIをGETでたたく
        private SecondStreetRawResponse getSecondStreetAPI(string url, Dictionary<string, string> param, string UserAgent)
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
                req.UserAgent = UserAgent;
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
        private string executePostRequest(ref HttpWebRequest req, byte[] bytes)
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
            catch(WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream)) {
                    Console.WriteLine(reader.ReadToEnd());
                }
                return "";
            }
        }
        public CookieContainer cc = new CookieContainer();

        }
    }
