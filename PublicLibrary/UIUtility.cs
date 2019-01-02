using DAL;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PublicLibrary
{
    public class UIUtility
    {

        //绑定下拉框
        public void BindSelect(HtmlSelect dpl, string tablename, string bindname, string bindid, string where)
        {
            string sql = string.Format("SELECT {0} as id,{1} as name FROM {2} {3}", bindid, bindname, tablename, where);
            DataTable dt = DBHelper.GetDataTable(sql);
            ListItem item = null;
            dpl.Items.Add(new ListItem("<--请选择-->", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ListItem(dt.Rows[i]["name"].ToString(), dt.Rows[i]["id"].ToString());

                dpl.Items.Add(item);
            }
        }
        public void BindSelectByApicloud(HtmlSelect dpl, string tablename, string bindname, string bindid, string where)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];

            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory(tablename);
            string filter = "{limit: 500000}";
            var temp1 = HttpUtility.UrlDecode(filter);//将明细数据进行解码
            var tempfilter = Newtonsoft.Json.JsonConvert.DeserializeObject(temp1) as JContainer;//转json格式

            var ret = model.Query(tempfilter.ToString());
            var jsonstrtemp = HttpUtility.UrlDecode(ret);//将明细数据进行解码
            var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
            ListItem item = null;
            dpl.Items.Add(new ListItem("<--请选择-->", "0"));
            for (int i = 0; i < jsondataformain.Count; i++)
            {
                item = new ListItem(jsondataformain[i].SelectToken(bindname).ToString(), jsondataformain[i].SelectToken(bindid).ToString());
                dpl.Items.Add(item);
            }
        }
        //绑定area下拉框
        public void BindAreaSelectByApicloud(HtmlSelect dpl, string tablename, string bindname, string bindid, string where)
        {
            string appid = System.Web.Configuration.WebConfigurationManager.AppSettings["appid"];
            string appkey = System.Web.Configuration.WebConfigurationManager.AppSettings["appkey"];

            var resouce = new Resource(appid, appkey);
            var model = resouce.Factory(tablename);
            string filter = "{limit: 500000,where:{Level:1}}";
            var temp1 = HttpUtility.UrlDecode(filter);//将明细数据进行解码
            var tempfilter = Newtonsoft.Json.JsonConvert.DeserializeObject(temp1) as JContainer;//转json格式
            string tempfilter1 = tempfilter.ToString();
            var ret = model.Query(tempfilter1);
            var jsonstrtemp = HttpUtility.UrlDecode(ret);//将明细数据进行解码
            var jsondataformain = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonstrtemp) as JContainer;//转json格式
            ListItem item = null;
            dpl.Items.Add(new ListItem("<--请选择-->", "0"));
            for (int i = 0; i < jsondataformain.Count; i++)
            {
                item = new ListItem(jsondataformain[i].SelectToken(bindname).ToString(), jsondataformain[i].SelectToken("id").ToString()+","+jsondataformain[i].SelectToken(bindid).ToString());
                dpl.Items.Add(item);
            }
        }
    }
}
