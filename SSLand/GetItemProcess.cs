using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSLand
{
    class GetItemProcess
    {
        static public List<SecondStreetListItem> getNewMatchingItems()
        {
           System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
           sw.Start();
           var conditions = SettingForm.LoadSearchConditions();
           var rst = new List<SecondStreetListItem>();
           try
           {
               var searchrst = MainForm.api.postNewItem();
               if (searchrst == null || searchrst.Count == 0) return rst;
                //とりあえず全部返す
                //rst = searchrst;
                //本当は以下でふるいにかける
                
                if (conditions.Count == 0)//ブランド絞りなし
                {
                    rst = searchrst;
                }
                else {//絞りあり


                    //ふるいにかけました
                    foreach (var item in searchrst)
                    {
                        Console.WriteLine(item.brand_name);
                        foreach (var con in conditions)
                        {
                            if (con.brand_name == item.brand_name)
                            {
                                rst.Add(item);
                                Console.WriteLine("追加したもの:" + item.brand_name);
                            }
                        }
                    }
                }

            }
           catch (Exception ex)
           {

               Log.Logger.Error("getNewMatchingItemsでエラー" + ex.Message);
           }
           TimeSpan ts = sw.Elapsed;
           Console.WriteLine("　{ts}");
           return rst;
        }
    }


    //static public bool isMatchCondition(FrilItem item, SearchConditionSettingsForm.SearchConditionClass con){
        //    try {
        //        bool ok = true;
        //        //キーワード
        //        if (string.IsNullOrEmpty(con.keyword_text) == false) ok = isKeyWordMatchCondition(con.keyword_text, item.item_name);
        //        //価格
        //        if (con.minprice > item.s_price || con.maxprice < item.s_price) ok = false;
        //        //NGワード
        //        if (string.IsNullOrEmpty(con.ngkeyword_text) == false && isKeyWordMatchCondition(con.ngkeyword_text, item.item_name)) ok = false;
        //        //NGセラー カンマ区切りでどれか1つでもヒットすればアウト
        //        if (string.IsNullOrEmpty(con.ngseller_text) == false) {
        //            string[] ngseller_splitted = con.ngseller_text.Split(',');
        //            foreach (string seller in ngseller_splitted) {
        //                if (seller.Trim() == item.user_id) ok = false;
        //            }
        //        }
        //        //指定セラー カンマ区切りでどれか1もヒットしなければアウト
        //        if (string.IsNullOrEmpty(con.seller_text) == false) {
        //            string[] seller_splitted = con.seller_text.Split(',');
        //            bool tmpflag = false;
        //            foreach (string seller in seller_splitted) {
        //                if (seller.Trim() == item.user_id) tmpflag = true;
        //            }
        //            if (tmpflag == false) ok = false;
        //        }
        //        //カテゴリ
        //        if (con.usecategory && item.category_id >= 0) {
        //            int item_level1_id = FrilMaster.categoryDictionary[item.category_p_id].grand_parent_id;
        //            int item_level2_id = item.category_p_id;
        //            int item_level3_id = item.category_id;
        //            if (con.category_level1 >= 0) {
        //                //カテゴリ1のチェックを行う
        //                if (isKeyWordMatchCondition(con.ngkeyword_text, con.category_level1_name)) ok = false;
        //                else if (con.category_level1 != item_level1_id) ok = false;
        //            }
        //            if (con.category_level2 >= 0) {
        //                //カテゴリ2のチェックを行う
        //                if (isKeyWordMatchCondition(con.ngkeyword_text, con.category_level2_name)) ok = false;
        //                else if (con.category_level2 != item_level2_id) ok = false;
        //            }
        //            if (con.category_level3 >= 0) {
        //                //カテゴリ3のチェックを行う
        //                if (isKeyWordMatchCondition(con.ngkeyword_text, con.category_level3_name)) ok = false;
        //                else if (con.category_level3 != item_level3_id) ok = false;
        //            }
        //        }
        //        //ブランド
        //        if (con.usebrand && item.brand_id < 0) ok = false;
        //        if (con.usebrand) {
        //            if (con.brand_id >= 0 && item.brand_id >= 0) {
        //                if (con.brand_id != item.brand_id) ok = false;
        //            }
        //        }
        //        //商品の状態
        //        if (con.usecondition && con.item_condition_list != null) {
        //            //どれにも引っかからなかったらアウト
        //            bool tmpflag = false;
        //            foreach (int conditionid in con.item_condition_list) {
        //                if (item.status == conditionid) tmpflag = true;
        //            }
        //            if (tmpflag == false) ok = false;
        //        }
        //        //まだOKでかつNGキーワードを商品説明までみる場合
        //        if (ok && con.ischeckdetail_for_ng) {
        //            if (isKeyWordMatchCondition(con.ngkeyword_text, item.detail)) {
        //                ok = false;
        //            }
        //        }
        //        return ok;
        //    } catch (Exception ex) {
        //        Log.Logger.Error(ex.Message);
        //        return false;
        //    }
        //}
}
