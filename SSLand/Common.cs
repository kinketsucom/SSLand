using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SSLand
{
    public class Common
    {
        public class Account
        {
            public string email;
            public string password;
            public string fril_auth_token;
            public string nickname;
            public string userId;
            public DateTime expirationDate;
            public Account(string email, string password, string auth_token, string nickname, string userid, DateTime expirationDate)
            {
                this.email = email;
                this.password = password;
                this.fril_auth_token = auth_token;
                this.nickname = nickname;
                this.userId = userid;
                this.expirationDate = expirationDate;
            }
            public Account()
            {

            }
        }
        public static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //UnixTimeの開始時刻
        static public DateTime getDateFromUnixTimeStamp(long timestamp)
        {
            try
            {
                return UNIX_EPOCH.AddSeconds(timestamp).ToLocalTime();
            }
            catch
            {
                return UNIX_EPOCH.ToLocalTime();
            }
        }
        static public DateTime getDateFromUnixTimeStamp(string timestamp)
        {
            try
            {
                return getDateFromUnixTimeStamp(long.Parse(timestamp));
            }
            catch
            {
                return UNIX_EPOCH.ToLocalTime();
            }
        }
        //DBからemailとpass拾ってきてログインを行い,Cookie付きのAPIインスタンスを返す
        //失敗:DBに情報ない or ログイン失敗:null返す
        static public SecondStreetAPI getSecondStreetAPIWithLogin(){
            var settingsDBHelper = new SettingsDBHelper();
            string email = settingsDBHelper.getSettingValue("email");
            string password = settingsDBHelper.getSettingValue("password");
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;
            var api = new SecondStreetAPI();
            bool loginres = api.trySecondStreetLogin(email, password);
            if(loginres) return api;
            else return null;
        }

        // Base64エンコード
        public static string Base64Encode(string str)
        {
            try
            {
                var enc = new UTF8Encoding();
                byte[] bytes = enc.GetBytes(str);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                //Log.Logger.Error("Base64EncodeError");
                return "";
            }
        }

        // Base64デコード
        public static string Base64Decode(string str)
        {
            try
            {
                var enc = new UTF8Encoding();
                byte[] bytes = Convert.FromBase64String(str);
                return enc.GetString(bytes);
            }
            catch (Exception ex)
            {
                // Log.Logger.Error("Base64DecodeError");
                return "";
            }
        }



    }



}
