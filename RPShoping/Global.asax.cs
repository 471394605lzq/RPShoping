using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Optimization;
using PublicLibrary;

namespace RPShoping
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //try {
            //    BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            //    //注册捆绑压缩JS与CSS 
            //    BundleTable.EnableOptimizations = true;//是否开启合并
            //    BundleConfig.RegisterBundles(BundleTable.Bundles); 
            //}
            //catch (Exception ex)
            //{
            //    if (ex.InnerException != null)
            //        ex = ex.InnerException;
            //    var error = string.Format("应用程序出错，详情：{0}.请重新启动网站，并非刷新网页！", ex.Message);
            //    throw new Exception(error);
            //}
        }
    }
}