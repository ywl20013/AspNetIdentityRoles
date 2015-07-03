using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AspNetIdentityRoles
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //取得当前方法命名空间
            string CurrentNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;

            //使用当前命名空间来注册路由，以防止区域路由冲突
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { CurrentNamespace + ".Controllers" }
            );
        }
    }
}
