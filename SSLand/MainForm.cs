using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using System.Net;
using Codeplex.Data;
using System.Media;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium;
using System.Reflection;
using System.Collections;
using OpenQA.Selenium.Remote;

namespace SSLand
{
    public partial class MainForm : Form
    {
        WebClient webClient = new WebClient();//必要かは不明
        private const int MAX_PANEL_NUM = 40;//タイムラインに表示する商品数
        List<SecondStreetListItem> bindlist = new List<SecondStreetListItem>();//タイムラインにバインドされている商品
        List<SecondStreetListItem> oldlist = new List<SecondStreetListItem>(); //1回前のリクエストで取得した商品リスト(重複を避ける)
        List<SecondStreetListItem> addlist = new List<SecondStreetListItem>(); //GUI更新時にタイムラインに追加すべき商品リスト
        //static List<string> boughtItemIDList = new List<string>(); //手動・自動購入した商品IDのリスト
        //static List<BoughtItemListForm.BoughtItem> boughtItemList = new List<BoughtItemListForm.BoughtItem>();
        int nowfocus = 0; //タイムラインの中の上から何個目を選択しているか
        List<SettingForm.SearchConditionClass> conditions = new List<SettingForm.SearchConditionClass>();//検索条件
        bool soundOn = false;//新着商品時に音をならすか
        string cr = "U+55U+2BU+45U+36U+39U+36U+42U+30U+55U+2BU+45U+37U+39U+34U+42U+30U+55U+2BU+45U+35U+42U+41U+42U+37U+55U+2BU+45U+35U+41U+34U+41U+37U+55U+2BU+45U+33U+38U+30U+38U+31U+55U+2BU+45U+37U+39U+46U+42U+33U+55U+2BU+45U+35U+42U+41U+38U+41U+55U+2BU+45U+37U+41U+42U+39U+43U+55U+2BU+45U+34U+42U+38U+38U+30";
        static BackgroundWorker bgWorker;
        static int dummy_report_progress = 0;
        public const string Key_LicenseKey = "LicenseKey";
        public const string Registry_Path = @"HKEY_CURRENT_USER\Software\SecondStreetWatcher";

        //SSLandのエージェント
        string agent = "reuse_store_release/3.0.2 CFNetwork/811.5.4 Darwin/16.7.0";

        public MainForm()
        {
            InitializeComponent();
        }
        static public SecondStreetAPI api;

        private void アカウント管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form account_form = new Account();
            account_form.Show();
        }
        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form setting_form = new SettingForm();
            setting_form.Show();

        }


        private async void MainWindow_Load(object sender, EventArgs e)
        {
            AdjustGUISize();
            SecondStreetItemPanel.setMainFormInstance(this);
            new SettingsDBHelper().onCreate();
            string stringValue = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, "Expire", "");
            string datestr = DateTime.Now.AddDays(7).ToString();
            if (string.IsNullOrEmpty(stringValue)) Microsoft.Win32.Registry.SetValue(Registry_Path, "Expire", datestr);
            string licensekey = (string)Microsoft.Win32.Registry.GetValue(MainForm.Registry_Path, "LicenseKey", "");
            if (string.IsNullOrEmpty(licensekey)) new LicenseForm().Show();
            if (await Task.Run(() => SecondStreetMaster.init()) == false)
            {
                MessageBox.Show("セカンドストリートからデータの読み込みに失敗しました.プログラムを終了します.\nインターネット環境を確認してください", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //GUI更新
            //リストに存在していないものだけをリストにいれる
            //まだリストに存在していないものだけをリストにいれる
            foreach (var item in addlist)
            {
                this.flowLayoutPanel1.SuspendLayout();
                bool isexist = false;
                foreach (var binditem in bindlist) if (item.goods_id == binditem.goods_id) isexist = true;
                if (isexist == false)
                {
                    bindlist.Add(item);
                    AddSecondStreetItemPanel(item);
                    if (soundOn) {
                        SoundPlayer simpleSound = new SoundPlayer(@".\notification.wav");
                        simpleSound.Play();
                    }
                }
                this.flowLayoutPanel1.ResumeLayout();
            }
            //if (addlist.Count > 0 && soundOn)
            //{
            //    SoundPlayer simpleSound = new SoundPlayer(@".\notification.wav");
            //    simpleSound.Play();
            //}
            //追加が終わったあとにmax_timelineを超えていれば超えた分だけ削除する
            //max_sizeを超えている場合は一番上のデータを削除する
            if (bindlist.Count > MAX_PANEL_NUM)
            {
                int deletenum = bindlist.Count - MAX_PANEL_NUM;
                for (int i = 0; i < deletenum; i++)
                {
                    bindlist.RemoveAt(0);
                    flowLayoutPanel1.Controls[0].Dispose();
                }
            }
        }


        private void AddSecondStreetItemPanel(SecondStreetListItem item)
        {
            SecondStreetItemPanel panel = new SecondStreetItemPanel(item);
            panel.Tag = item;
            panel.TabStop = false;
            this.flowLayoutPanel1.Controls.Add(panel);
            this.flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
            if (SettingForm.getAutoScroll()) 
            {
                panel.SetFocusBuyButton();
                this.flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                nowfocus = 0;
            }
            else
            {
                nowfocus++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy) return;
            else this.backgroundWorker1.RunWorkerAsync();
        }


        private void startProcessButton_Click(object sender, EventArgs e)
        {
            ToggleMonitoring();
        }
        
        private void ToggleMonitoring()
        {
            if (this.startProcessButton.BackColor != Color.Red)
            {
                if (this.backgroundWorker1.IsBusy)
                {
                    MessageBox.Show("しばらくたってから再実行してください");
                    return;
                }
                //OFF -> ON
                //監視の開始・停止を行う
                MainForm.api = Common.getSecondStreetAPIWithLogin();
                if (api == null) {
                    MessageBox.Show("アカウントを確認して下さい");
                    return;
                }
                this.startProcessButton.BackColor = Color.Red;
                this.startProcessButton.Text = "監視停止(z)";
                this.timer1.Enabled = true;
                addlist.Clear();
                bindlist.Clear();
                ClearSecondStreetItemPanel();
                oldlist.Clear();
                //boughtItemIDList.Clear();
                //SecondStreetItemPanel.ReloadPhotoSize();
            }
            else
            {
                //ON -> OFF
                this.startProcessButton.BackColor = Color.Transparent;
                this.startProcessButton.Text = "監視開始(z)";
                this.timer1.Enabled = false;
                this.backgroundWorker1.CancelAsync();
            }
        }
        private void ClearSecondStreetItemPanel()
        {
            this.flowLayoutPanel1.Controls.Clear();
        }

        //こちらは動かなかった
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'z') ToggleMonitoring();
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Home)
            {
                this.flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
                int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                if (panel_cnt <= 0) return;
                try
                {
                    SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 1];
                    //panel.SetFocusBuyButton();
                    nowfocus = 0;
                }
                catch (Exception ex)
                {

                }
            }
            if (e.KeyCode == System.Windows.Forms.Keys.PageUp)
            {
                this.flowLayoutPanel1.AutoScrollPosition = new Point(-this.flowLayoutPanel1.AutoScrollPosition.X, -this.flowLayoutPanel1.AutoScrollPosition.Y - 120);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.PageDown)
            {
                this.flowLayoutPanel1.AutoScrollPosition = new Point(-this.flowLayoutPanel1.AutoScrollPosition.X, -this.flowLayoutPanel1.AutoScrollPosition.Y + 120);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Tab && e.Shift != true)
            {
                int old = nowfocus;
                nowfocus++;
                if (nowfocus >= bindlist.Count) nowfocus = 0;
                try
                {
                    //フォーカスする
                    int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                    SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 1 - nowfocus];
                    //panel.SetFocusBuyButton();
                }
                catch (Exception ex)
                {
                    //失敗したので戻す
                    nowfocus = old;
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.Tab && e.Shift == true)
            {
                int old = nowfocus;
                nowfocus--;
                if (nowfocus < 0) nowfocus = 0;
                if (nowfocus >= bindlist.Count) nowfocus = 0;
                try
                {
                    //フォーカスする
                    int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                    SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 1 - nowfocus];
                    //panel.SetFocusBuyButton();
                }
                catch (Exception ex)
                {
                    //失敗したので戻す
                    nowfocus = old;
                }
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F1)
            {
                //bool execute = SettingForm.getf1f4Enable();
                //if (execute)
                //{
                    //int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                    //if (panel_cnt >= 1)
                    //{
                    //    SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 1];
                    //    SecondStreetItem item = (SecondStreetItem)panel.Tag;
                    //    ExecuteBuyItem(item, true);
                    //}
                //}
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F2)
            {
                //bool execute = SettingForm.getf1f4Enable();
                //if (execute)
                //{
                //    int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                //    if (panel_cnt >= 2)
                //    {
                //        SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 2];
                //        SecondStreetItem item = (SecondStreetItem)panel.Tag;
                //        ExecuteBuyItem(item, true);
                //    }
                //}
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F3)
            {
                //bool execute = SettingForm.getf1f4Enable();
                //if (execute)
                //{
                //    int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                //    if (panel_cnt >= 3)
                //    {
                //        SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 3];
                //        SecondStreetItem item = (SecondStreetItem)panel.Tag;
                //        ExecuteBuyItem(item, true);
                //    }
                //}
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.F4)
            {
                //bool execute = SettingForm.getf1f4Enable();
                //if (execute)
                //{
                //    int panel_cnt = this.flowLayoutPanel1.Controls.Count;
                //    if (panel_cnt >= 4)
                //    {
                //        SecondStreetItemPanel panel = (SecondStreetItemPanel)this.flowLayoutPanel1.Controls[panel_cnt - 4];
                //        SecondStreetItem item = (SecondStreetItem)panel.Tag;
                //        ExecuteBuyItem(item, true);
                //    }
                //}
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //api = Common.checkFrilAPI(api);
            try
            {
                bgWorker = (BackgroundWorker)sender;
                //停止リクエストがあれば終了
                if (bgWorker.CancellationPending) return;
                addlist.Clear();
                List<SecondStreetListItem> rst = new List<SecondStreetListItem>();
                //新着商品を取得
                rst = GetItemProcess.getNewMatchingItems();
                foreach (var newitem in rst)
                {
                    bool isnew = true;
                    foreach (var olditem in oldlist)
                    {
                        if (newitem.goods_id == olditem.goods_id)
                        {
                            isnew = false;
                            break;
                        }
                    }
                    if (isnew)
                    {
                        addlist.Add(newitem);
                    }
                }
                oldlist = new List<SecondStreetListItem>(rst);
                if (addlist.Count != 0)
                {
                    //GUI更新リクエスト
                    dummy_report_progress += 1;
                    if (dummy_report_progress > 100) dummy_report_progress = 0;
                    bgWorker.ReportProgress(dummy_report_progress);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e) {
            AdjustGUISize();
        }
        private void AdjustGUISize() {
            this.flowLayoutPanel1.Width = this.Width - this.flowLayoutPanel1.Left - 60;
            this.flowLayoutPanel1.Height = this.Height - this.flowLayoutPanel1.Top - 60;
        }
        private void button5_Click(object sender, EventArgs e) {
           //test button
        }

        private void ライセンスToolStripMenuItem_Click(object sender, EventArgs e) {
            new LicenseForm().Show();
        }

        public async void BuyItem(SecondStreetListItem item) {
            if (SettingForm.getShowPrompt()) {
                if (DialogResult.OK != MessageBox.Show(string.Format("{0}を購入しますか？", item.goods_name), "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)) {
                    return;
                }
            }
            this.toolStripStatusLabel1.Text = string.Format("購入処理開始:{0}", item.goods_name);
            bool res = await Task.Run(() => ExecuteItem(item.shops_id.ToString(), item.goods_id.ToString()));
            if (res) {
                this.toolStripStatusLabel1.Text = string.Format("購入成功: {0}", item.goods_name);
                MessageBox.Show(string.Format("{0}を購入しました", item.goods_name), "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else {
                this.toolStripStatusLabel1.Text = string.Format("購入失敗: {0}", item.goods_name);
                MessageBox.Show(string.Format("{0}の購入に失敗しました", item.goods_name), "確認", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool ExecuteItem(string shopsId, string goodsId) {
            ChromeDriver chromeDriver = null;
            try {
                bool useCard = SettingForm.getUseCard();
                string cardnumber = SettingForm.getCardNumber();
                string cardmonth = SettingForm.getCardMonth();
                string cardyear = SettingForm.getCardYear();
                string securitycode = SettingForm.getCardSecurityCode();
                string cardlastname = SettingForm.getCardLastName();
                string cardfirstname = SettingForm.getCardFirstName();
                string vpasspassword = SettingForm.getVpassPassword();
                //SeleniumWebDriverを使用する
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless"); //Webページを表示しない
                options.AddArgument("window-size=1920,1920");
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true; //コンソールを表示しない
                chromeDriver = new ChromeDriver(service, options);
                var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
                //まず2ndstreetのページに飛ばないとCookie追加できない
                chromeDriver.Url = "https://www.2ndstreet.jp/";
                //APIからログインCookie取り出してWebDrierにセット
                foreach (System.Net.Cookie cookie in api.cc.GetCookies(new Uri("https://www.2ndstreet.jp"))) {
                    Console.WriteLine(cookie.Name);
                    chromeDriver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie.Name, cookie.Value, ".2ndstreet.jp", "/", DateTime.Now.AddDays(30)));
                }
                //カートに追加
                api.addItemToCartWeb(shopsId, goodsId);
                //ここからWebDriver操作
                //カートの商品一覧を開く
                chromeDriver.Url = "https://www.2ndstreet.jp/cart/updateForApp";//?releaseurl=1&num=1&sp=on&ver=3.0.5&goodsId={0}&shopsID={1}", goodsId, shopsId);
                //決済へ進む
                try {
                    var element = chromeDriver.FindElementByXPath("/html/body/article/ul/form");
                    element.Submit();
                } catch {
                    //submit成功後elementが消えてエラーになる: TODO:真面目に処理
                }
                //ポイントを使用するか
                try {
                    var element2 = chromeDriver.FindElementByXPath("//*[@id=\"reduceForm\"]");
                    element2.Submit();
                } catch {

                }
                if (useCard) {
                    //クレジットカード払いを選択
                    var credit_radio_input = chromeDriver.FindElementByXPath("//*[@id=\"payment_choose_member2\"]");
                    credit_radio_input.Click();
                    //カード情報および住所の入力:住所はデフォルトで選択されているものを使用する
                    bool input_new_card = true;
                    if (input_new_card) {
                        //新しいカード番号を入力するを選択
                        var radio_input = chromeDriver.FindElementByXPath("//input[@id=\"card_choose_other\"]");
                        radio_input.Click();
                        //情報の記入
                        var cardTypeSelct = chromeDriver.FindElementByXPath("//select[@name=\"creditCardType\"]");
                        new SelectElement(cardTypeSelct).SelectByText("VISA");
                        var cardnumber_input = chromeDriver.FindElementByXPath("//input[@id=\"card_number\"]");
                        cardnumber_input.Click();
                        cardnumber_input.SendKeys(cardnumber);
                        var cccsc_input = chromeDriver.FindElementByXPath("//input[@id=\"cc-csc\"]");
                        System.Threading.Thread.Sleep(3000);
                        cccsc_input.Click();
                        cccsc_input.SendKeys(securitycode);
                        var monthSelct = chromeDriver.FindElementByXPath("//select[@name=\"creditAvailableMonth\"]");
                        new SelectElement(monthSelct).SelectByText(cardmonth);
                        var yearSelct = chromeDriver.FindElementByXPath("//select[@name=\"creditAvailableYear\"]");
                        new SelectElement(yearSelct).SelectByText(cardyear);
                        var lastname_input = chromeDriver.FindElementByXPath("//input[@id=\"creditLastName\"]");
                        lastname_input.Click();
                        lastname_input.SendKeys(cardlastname);
                        var firstname_input = chromeDriver.FindElementByXPath("//input[@id=\"creditFirstName\"]");
                        firstname_input.Click();
                        firstname_input.SendKeys(cardfirstname);
                        System.Threading.Thread.Sleep(4000);
                    } else {
                        //既に登録されているクレカ選択
                        var registered_card = chromeDriver.FindElementByXPath("//*[@id=\"card_choose_0\"]");
                        registered_card.Click();
                    }
                } else {
                    //代引き払い
                    var radio_input = chromeDriver.FindElementByXPath("//*[@id=\"payment_choose_member1\"]");
                    radio_input.Click();
                }
                //支払い方法・住所確定
                try {
                    var element3 = chromeDriver.FindElementByXPath("//button[@id=\"submit-btn\"]");
                    var remote = element3 as RemoteWebElement;
                    var hack = remote.LocationOnScreenOnceScrolledIntoView;
                    element3.Click();
                } catch {

                }
                //最終確認へ
                System.Threading.Thread.Sleep(2000);
                try {
                    var element4 = chromeDriver.FindElementByXPath("//*[@id=\"flownext_btn\"]/button");
                    var remote = element4 as RemoteWebElement;
                    var hack = remote.LocationOnScreenOnceScrolledIntoView;
                    element4.Click();
                } catch {

                }
                if (useCard) {
                    //VPASSのみ使用可能
                    System.Threading.Thread.Sleep(2000);
                    try {
                        var element5 = chromeDriver.FindElementByXPath("//input[@name=\"Password\"]");
                        element5.Click();
                        element5.SendKeys(vpasspassword);
                    } catch {

                    }
                }
                chromeDriver.Quit();
                Log.Logger.Info(string.Format("購入成功: ショップID:{0} 商品ID:{1}", shopsId, goodsId));
                return true;
            } catch (Exception) {
                if (chromeDriver != null) chromeDriver.Quit();
                Log.Logger.Error(string.Format("購入失敗: ショップID:{0} 商品ID:{1}", shopsId, goodsId));
                return false;
            }
        }
       
        private void soundToggleButton_Click(object sender, EventArgs e) {
            if (soundOn) {
                this.soundToggleButton.BackColor = Color.Transparent;
                soundOn = false;
            } else {
                this.soundToggleButton.BackColor = Color.GreenYellow;
                soundOn = true;
            }
        }
    }
}
