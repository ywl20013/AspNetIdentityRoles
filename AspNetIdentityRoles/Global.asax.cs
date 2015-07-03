using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;

namespace AspNetIdentityRoles
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MoudleLoader.Load();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public class MoudleLoader
    {
        public static void Load(string filename)
        {
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            try
            {
                if (System.IO.File.Exists(path + filename))
                {
                    //加载指定路径下的程序集
                    System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(path + filename);

                    string infos = "";
                    foreach (System.Reflection.TypeInfo info in asm.DefinedTypes)
                    {
                        infos += "\t" + info.FullName + "\n";
                    }
                    //  Ywl.Data.Console.writeLog("LoadModule \"" + filename + "\":OK\n" + infos);
                }
            }
            catch (Exception ex)
            {
                //  Ywl.Data.Console.writeLog("LoadModule \"" + filename + "\"\n出错：" + ex.Message);
            }
        }
        public static void Load()
        {
            //string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "AreaMoudles.xml");
            foreach (XElement moudleElement in findElementsByTagName(rootElement, "Moudle"))
            {
                XElement filenameElement = findElementByTagName(moudleElement, "FileName");
                if (filenameElement != null)
                {
                    Load(filenameElement.Value);
                }
            }
        }


        public static XElement findElementByAttribute(XElement xElement, string tagName, string attribute)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
        }
        public static XElement findElementByTagName(XElement xElement, string tagName)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Name.LocalName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }
        public static IEnumerable<XElement> findElementsByTagName(XElement xElement, string tagName)
        {
            return xElement.Elements(tagName).Where(x => x.Name.LocalName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
