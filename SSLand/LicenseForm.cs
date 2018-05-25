﻿using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSLand {
    public partial class LicenseForm : Form {
        public LicenseForm() {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            string registry_license_key = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, MainForm.Key_LicenseKey, "");
            bool ok = false;
            if (string.IsNullOrEmpty(registry_license_key)) {
                //ライセンスキー初めてなので登録を行う
                bool res = register(this.licenseKeyTextBox.Text.Trim(), Environment.MachineName);
                if (res) {
                    ok = true;
                    registerLicenseKey(this.licenseKeyTextBox.Text.Trim());
                    MessageBox.Show("購入ありがとうございます。\nライセンスキーを確認しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            else {
                //チェックを行う必要があるのでチェックする
                bool res = check(registry_license_key, Environment.MachineName);
                if (res) {
                    ok = true;
                    MessageBox.Show("ライセンスキーを確認しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            if (!ok) {
                MessageBox.Show("ライセンスキーが確認できません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static bool checkLicense() {
            string expiration_datestr = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, "Expire", "");
            try {
                //ライセンスが切れていたらfalse
                if (string.IsNullOrEmpty(expiration_datestr)) throw new Exception();
                DateTime expiration_date = DateTime.Parse(expiration_datestr);
                if (expiration_date >= DateTime.Now) {//いまだけ
                    return true;
                }
            }
            catch (Exception ex) {

            }
            string registry_license_key = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, MainForm.Key_LicenseKey, "");
            if (string.IsNullOrEmpty(registry_license_key) == false) {
                if (check(registry_license_key, Environment.MachineName)) {
                    return true;
                }
            }
            MessageBox.Show("ライセンス登録をしてください", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private void LicenseForm_Load(object sender, EventArgs e) {
            this.TopMost = true;
            //ライセンスが切れる日時を読み込み
            string expiration_datestr = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, "Expire", "");
            try {
                //ライセンスが切れていたらボタンを推せなくする
                if (string.IsNullOrEmpty(expiration_datestr)) throw new Exception();
                expireLabel.Text = expiration_datestr;
                DateTime expiration_date = DateTime.Parse(expiration_datestr);
                if (expiration_date < DateTime.Now) { //いまだけ
                    button2.Enabled = false;
                }
            }
            catch (Exception ex) {
                button2.Enabled = false;
            }

            //ライセンスキーがあればテキストボックスにいれておく
            string registry_license_key = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, MainForm.Key_LicenseKey, "");
            if (string.IsNullOrEmpty(registry_license_key) == false) this.licenseKeyTextBox.Text = registry_license_key;

        }
        public static void registerLicenseKey(string licensekey) {
            Microsoft.Win32.Registry.SetValue(MainForm.Registry_Path, MainForm.Key_LicenseKey, licensekey);
        }
        private bool register(string guid, string machine_name) {
            try {
                string text = "machine_name=" + Uri.EscapeUriString(machine_name);
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://160.16.69.60/api/register/" + guid);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = (long)bytes.Length;

                //POST
                using (Stream requestStream = req.GetRequestStream()) {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                //結果取得
                string result = "";
                using (Stream responseStream = req.GetResponse().GetResponseStream()) {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"))) {
                        result = streamReader.ReadToEnd();
                    }
                }
                dynamic resjson = DynamicJson.Parse(result);
                if (resjson.result == "succeeded") return true;
                return false;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        static public bool check(string guid, string machine_name) {
            try {
                string text = "machine_name=" + Uri.EscapeUriString(machine_name);
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://160.16.69.60/api/check/" + guid);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = (long)bytes.Length;

                //POST
                using (Stream requestStream = req.GetRequestStream()) {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                //結果取得
                string result = "";
                using (Stream responseStream = req.GetResponse().GetResponseStream()) {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"))) {
                        result = streamReader.ReadToEnd();
                    }
                }
                dynamic resjson = DynamicJson.Parse(result);
                if (resjson.result == "ok") return true;
                return false;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
