using PublicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RPShoping.Web.After
{
    public partial class AfterUserAdminEdit : System.Web.UI.Page
    {
        //UI共用类
        UIUtility _uiu = new UIUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.EnableViewState = false;
            if (!Page.IsPostBack)
                InitSelect();
        }
        private void InitSelect()
        {
            _uiu.BindSelect(rolese, "tb_Role", "R_Name", "R_ID", "");
        }
    }
}