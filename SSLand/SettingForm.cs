using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SSLand
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }



        bool isEditMode = false;//新規追加中:false 編集中;true
        int nowedit_index;//現在編集中の検索条件のDB内リストのインデックス
        //SecondStreetMaster.Category selected_category;//現在選択中のカテゴリ
        public class SearchConditionClass
        {
            public int minprice;
            public int maxprice;
            public string keyword_text;
            public string ngkeyword_text;
            public bool usecategory;
            public bool usebrand;
            public bool usecondition;
            public int category_level1;//-1だと使用しない
            public int category_level2;
            public int category_level3;
            public string category_level1_name;
            public string category_level2_name;
            public string category_level3_name;
            public bool autobuyoption = false;
            public int brand_id;
            public string brand_name;
            public string ngseller_text;
            public string seller_text;
            public string condition_name;
            public bool ischeckdetail_for_ng;
            public List<int> item_condition_list;
            public override string ToString()
            {
                string rst = "";
                try
                {
                    rst += this.condition_name + " ";
                    //if (this.autobuyoption) rst += "自動購入ON";
                    //rst += minprice.ToString() + "円～" + maxprice.ToString() + "円";
                    //if (!String.IsNullOrEmpty(keyword_text)) rst += "キーワード: " + keyword_text;
                    //if (!String.IsNullOrEmpty(ngkeyword_text)) rst += "NGキーワード: " + ngkeyword_text;
                    //if (!String.IsNullOrEmpty(seller_text)) rst += "出品者指定: " + seller_text;
                    //if (!String.IsNullOrEmpty(ngseller_text)) rst += "NG出品者: " + ngseller_text;
                    //if (usecategory)
                    //{
                    //    rst += " カテゴリ:";
                    //    if (category_level1 > 0) rst += category_level1_name;
                    //    if (category_level2 > 0) rst += category_level2_name;
                    //    if (category_level3 > 0) rst += category_level3_name;
                    //}
                    if (usebrand)
                    {
                        //rst += " ブランド:";
                        if (brand_id > 0) rst += brand_name;
                    }
                    //if (usecondition)
                    //{
                    //    rst += "商品の状態 ";
                    //    if (item_condition_list != null) foreach (int id in item_condition_list) rst += id.ToString();
                    //}
                    //if (ischeckdetail_for_ng)
                    //{
                    //    rst += "NG商品説明チェック";
                    //}


                }
                catch (Exception ex)
                {

                }
                return rst;
            }
        }

        public const string brandenable = "brandenable";
        public const string autoscroll = "autoscroll";
        //public const string f1f4enable = "f1f4enable";
        public const string photosize = "photosize";

        static public bool getAutoScroll()
        {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue(autoscroll);
            if (String.IsNullOrEmpty(rst)) return false;//default
            if (rst == "False") return false;
            else return true;
        }
        static public int getPhotoSize()
        {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue(photosize);
            if (String.IsNullOrEmpty(rst)) return 160;//default;
            try
            {
                int photos = int.Parse(rst);
                return photos;
            }
            catch (Exception ex)
            {
                return 160;
            }
        }
        static public bool getBrandEnable()
        {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue(brandenable);
            if (String.IsNullOrEmpty(rst)) return false;//default
            if (rst == "False") return false;
            else return true;
        }
        static public bool getShowPrompt() {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue("showprompt");
            if (String.IsNullOrEmpty(rst)) return true;//default
            if (rst == "False") return false;
            else return true;
        }
        static public bool getUseCard() {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue("usecard");
            if (String.IsNullOrEmpty(rst)) return false;//default
            if (rst == "False") return false;
            else return true;
        }
        static public string getCardNumber() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("cardnumber");
        }
        static public string getCardMonth() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("cardmonth");
        }
        static public string getCardYear() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("cardyear");
        }
        static public string getCardSecurityCode() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("securitycode");
        }
        static public string getCardFirstName() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("cardfirstname");
        }
        static public string getCardLastName() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("cardlastname");
        }
        static public string getVpassPassword() {
            var settingsDBHelper = new SettingsDBHelper();
            return settingsDBHelper.getSettingValue("vpasspassword");
        }
        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.showPromptBeforeBuyCheckbox.Checked = getShowPrompt();
            this.autoscrollCheckBox.Checked = getAutoScroll();
            this.photosizeNumericDown.Value = getPhotoSize();
            this.useBrandCheckBox.Checked = getBrandEnable();
            this.cardNumberTextBox.Text = getCardNumber();
            this.expireMonthTextBox.Text = getCardMonth();
            this.expireYearTextBox.Text = getCardYear();
            this.securityCodeTextBox.Text = getCardSecurityCode();
            this.cardFirstNameTextBox.Text = getCardFirstName();
            this.cardLastNameTextBox.Text = getCardLastName();
            this.vpassPasswordTextbox.Text = getVpassPassword();
            this.radioButton1.Checked = getUseCard();
            this.radioButton2.Checked = !getUseCard();

            //ブランドデータ
            foreach (var pair in SecondStreetMaster.brandDictionary)
            {
                
                this.brandComboBox.Items.Add(pair.Value.name);

            }
            RefreshListBox();
        }



        static private string getSettingStr(string key)
        {
            var settingsDBHelper = new SettingsDBHelper();
            string rst = settingsDBHelper.getSettingValue(key);
            if (String.IsNullOrEmpty(rst)) return "";
            return rst;
        }

        private void RefreshListBox(string keyword = "")
        {
            if (string.IsNullOrEmpty(keyword))
            {
                this.listBox1.Items.Clear();
                var conditions = LoadSearchConditions();
                foreach (var sc in conditions) this.listBox1.Items.Add(sc);
            }
            else
            {
                this.listBox1.Items.Clear();
                var conditions = LoadSearchConditions();
                foreach (var sc in conditions)
                {
                    if (sc.ToString().IndexOf(keyword) >= 0) this.listBox1.Items.Add(sc);
                }
            }
        }
        public static List<SearchConditionClass> LoadSearchConditions()
        {
            List<SearchConditionClass> rst = new List<SearchConditionClass>();
            try
            {
                var settingsDBHelper = new SettingsDBHelper();
                string base64xml = settingsDBHelper.getSettingValue("search_conditions");
                if (string.IsNullOrEmpty(base64xml)) throw new Exception("no search conditions");
                string xml = Common.Base64Decode(base64xml);
                if (string.IsNullOrEmpty(xml)) throw new Exception("base64 decode fail");
                try
                {
                    SearchConditionClass[] array;
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SearchConditionClass[]));
                    using (TextReader reader = new StringReader(xml))
                    {
                        array = (SearchConditionClass[])serializer.Deserialize(reader);
                    }
                    foreach (var sc in array) rst.Add(sc);
                    return rst;
                }
                catch (Exception ex)
                {
                    return rst;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return rst;
            }
        }

        private string SearchConditionsToXml(List<SearchConditionClass> newsclist)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SearchConditionClass[]));
                using (StringWriter textWriter = new StringWriter())
                {
                    serializer.Serialize(textWriter, newsclist.ToArray());
                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";
            }
        }


        private void SaveSearchConditions(List<SearchConditionClass> newsclist)
        {
            string xml = SearchConditionsToXml(newsclist);
            new SettingsDBHelper().updateSettings("search_conditions", Common.Base64Encode(xml));
        }
        private void SaveSettings()
        {
            var settingsDBHelper = new SettingsDBHelper();
            settingsDBHelper.updateSettings(autoscroll, this.autoscrollCheckBox.Checked.ToString());
            settingsDBHelper.updateSettings(photosize, ((int)this.photosizeNumericDown.Value).ToString());
            settingsDBHelper.updateSettings(brandenable, this.useBrandCheckBox.Checked.ToString());

            settingsDBHelper.updateSettings("showprompt", showPromptBeforeBuyCheckbox.Checked.ToString());

            settingsDBHelper.updateSettings("cardnumber", cardNumberTextBox.Text.Trim());
            settingsDBHelper.updateSettings("cardmonth", expireMonthTextBox.Text.Trim());
            settingsDBHelper.updateSettings("cardyear", expireYearTextBox.Text.Trim());
            settingsDBHelper.updateSettings("securitycode", securityCodeTextBox.Text.Trim());
            settingsDBHelper.updateSettings("cardfirstname", cardFirstNameTextBox.Text.Trim());
            settingsDBHelper.updateSettings("cardlastname", cardLastNameTextBox.Text.Trim());
            settingsDBHelper.updateSettings("vpasspassword", vpassPasswordTextbox.Text.Trim());
            settingsDBHelper.updateSettings("usecard", radioButton1.Checked.ToString());
        }
        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void useBrandCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.useBrandCheckBox.Checked) this.brandComboBox.Visible = true;
            else this.brandComboBox.Visible = false;
            if (this.useBrandCheckBox.Checked) this.brandAddButton.Visible = true;
            else this.brandAddButton.Visible = false;
            if (this.useBrandCheckBox.Checked) this.brandDelButton.Visible = true;
            else this.brandDelButton.Visible = false;
            if (this.useBrandCheckBox.Checked) this.listBox1.Visible = true;
            else this.listBox1.Visible = false;
        }


        //選択しているブランドを追加するボタン
        private void brandAddButton_Click(object sender, EventArgs e)
        {
            string selected_brand = brandComboBox.Text;

            var sclist = LoadSearchConditions();
            foreach(var item in sclist)
            {
                if (item.brand_name == selected_brand)
                {
                    MessageBox.Show("すでに追加されているブランドです", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

            if (selected_brand != "未指定")
            {
                //一応これでidはとれる
                //int shop_id = SecondStreetMaster.brandDictionary.First(x => x.Value.name == selected_brand).Key;
                //string branddayo = SecondStreetMaster.brandDictionary[shop_id].name_kana;
                //Console.WriteLine(branddayo);

                SearchConditionClass newsc = getSearchConditionFromGUI();

                //if (newsc == null)
                //{
                //    MessageBox.Show("正しく入力してください", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                if (isEditMode == false)
                {
                    var nowsclist = LoadSearchConditions();
                    nowsclist.Add(newsc);
                    SaveSearchConditions(nowsclist);
                    RefreshListBox();
                    //ResetGUIComponent();

                }
                else
                {
                    isEditMode = false;
                    var nowsclist = LoadSearchConditions();
                    nowsclist[nowedit_index] = newsc;
                    SaveSearchConditions(nowsclist);
                    RefreshListBox();
                    //ResetGUIComponent();
                    //this.addSearchConditionButton.Text = "追加";
                }

            }
        }

        private void brandDelButton_Click(object sender, EventArgs e)
        {

            string selected_brand = listBox1.Text;
            var nowsclist = LoadSearchConditions();
            foreach(var check_list in nowsclist)
            {
               
                Console.WriteLine(check_list.brand_name.Trim()+" "+selected_brand);
                if(check_list.brand_name.Trim() == selected_brand.Trim())
                {
                    nowsclist.Remove(check_list);
                    Console.WriteLine("リムった");
                    break;
                }
            }
            

            SaveSearchConditions(nowsclist);
            RefreshListBox();

        }



        private SearchConditionClass getSearchConditionFromGUI()
        {
            var rst = new SearchConditionClass();
            try
            {
                //rst.condition_name = conditionNameTextBox.Text.Trim();
                //rst.minprice = int.Parse(this.minPriceTextBox.Text.Trim());
                //rst.maxprice = int.Parse(this.maxPriceTextBox.Text.Trim());
                //rst.autobuyoption = autoBuyOptionCheckBox.Checked;
                //rst.keyword_text = keywordTextBox.Text.Trim();
                //rst.ngkeyword_text = ngKeywordTextBox.Text.Trim();
                //rst.seller_text = sellerTextBox.Text.Trim();
                //rst.ngseller_text = ngSellerTextBox.Text.Trim();
                //rst.ischeckdetail_for_ng = ngDetailCheckBox.Checked;
                //rst.usecondition = condition_check_CheckBox.Checked;
                //rst.usecategory = useCategoryCheckBox.Checked;
                rst.usebrand = useBrandCheckBox.Checked;
                //if (categoryComboBox1.SelectedItem != null)
                //{
                //    rst.category_level1 = ((FrilMaster.Category)categoryComboBox1.SelectedItem).id;
                //    rst.category_level1_name = ((FrilMaster.Category)categoryComboBox1.SelectedItem).name;
                //}
                //else rst.category_level1 = -1;
                //if (categoryComboBox2.SelectedItem != null)
                //{
                //    rst.category_level2 = ((FrilMaster.Category)categoryComboBox2.SelectedItem).id;
                //    rst.category_level2_name = ((FrilMaster.Category)categoryComboBox2.SelectedItem).name;
                //}
                //else rst.category_level2 = -1;
                //if (categoryComboBox3.SelectedItem != null)
                //{
                //    rst.category_level3 = ((FrilMaster.Category)categoryComboBox3.SelectedItem).id;
                //    rst.category_level3_name = ((FrilMaster.Category)categoryComboBox3.SelectedItem).name;
                //}
                //else rst.category_level3 = -1;
                if (brandComboBox.SelectedItem != null)
                {

                    //一応これでidはとれる
                    string selected_brand = brandComboBox.Text;
                    int brand_id = SecondStreetMaster.brandDictionary.First(x => x.Value.name == selected_brand).Key;
                    string brand_name = SecondStreetMaster.brandDictionary[brand_id].name;
                    rst.brand_id = brand_id;
                    rst.brand_name = brand_name;

                    //rst.brand_id = ((SecondStreetMaster.Brand)brandComboBox.SelectedItem).id;
                    //rst.brand_name = ((SecondStreetMaster.Brand)brandComboBox.SelectedItem).name;
                }
                //else rst.brand_id = -1;

                rst.item_condition_list = new List<int>();
                //if (checkBox1.Checked) rst.item_condition_list.Add(5);
                //if (checkBox2.Checked) rst.item_condition_list.Add(4);
                //if (checkBox3.Checked) rst.item_condition_list.Add(6);
                //if (checkBox4.Checked) rst.item_condition_list.Add(3);
                //if (checkBox5.Checked) rst.item_condition_list.Add(2);
                //if (checkBox6.Checked) rst.item_condition_list.Add(1);

                //出品者,NG出品者のvalidateを行う
                //FrilAPI api = Common.getFrilAPIFromDB();
                /*if (api == null) throw new Exception("api null error");
                if (string.IsNullOrEmpty(rst.seller_text) == false) {
                    string[] sellers = rst.seller_text.Split(',');
                    bool valid = true;
                    foreach (var seller in sellers) if (api.IsSellerIDValid(seller) == false) valid = false;
                    if (valid == false) throw new Exception("出品者ID不正");
                }
                if (string.IsNullOrEmpty(rst.ngseller_text) == false) {
                    string[] ngsellers = rst.ngseller_text.Split(',');
                    bool valid = true;
                    foreach (var ngseller in ngsellers) if (api.IsSellerIDValid(ngseller) == false) valid = false;
                    if (valid == false) throw new Exception("NG出品者ID不正");
                }*/
                return rst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
