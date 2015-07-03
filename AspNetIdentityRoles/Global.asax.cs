using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                    //System.Console.SetOut(HttpContext.Current.Response.Output);
                    //System.Console.WriteLine("LoadModule \"" + filename + "\":OK\n" + infos);
                    Console.writeLog("LoadModule \"" + filename + "\":OK\n" + infos);
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

    public class Console
    {
        #region 检查是否为IP地址
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 获得当前页面客户端的IP
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = "";
            try
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                //GetDnsRealHost();
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(result) || !IsIP(result))
                    return "127.0.0.1";
            }
            catch { }
            return result;
        }
        #endregion

        public static string getLogFileName()
        {
            string VirtualPath = System.Web.HttpRuntime.AppDomainAppVirtualPath + "/logs/" + DateTime.Now.ToString("yyyy-MM");
            string MapPath = GetMapPath(VirtualPath); //物理路径
            //检查上传的物理路径是否存在，不存在则创建
            if (!Directory.Exists(MapPath))
            {
                try
                {
                    Directory.CreateDirectory(MapPath);
                }
                catch
                {
                    return "";
                    //return "{\"status\": 0, \"msg\": \"创建上传路径" + MapPath.Replace("\\", "\\\\") + "时出错了啦！\"}";
                }
            }

            return MapPath + "/log " + DateTime.Now.ToString("yyyy-MM-dd HH") + ".txt";
        }

        public static void writeLog(string msg)
        {
            FileInfo fi = new FileInfo(getLogFileName());
            StreamWriter writer = null;
            if (!fi.Exists)
            {
                writer = fi.CreateText();
            }
            else
                writer = fi.AppendText();

            writer.WriteLine("[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffzzz") + " ]");
            writer.WriteLine("\tIP:" + GetIP());
            writer.WriteLine("\tMSG:");
            writer.WriteLine("\t" + msg);

            writer.WriteLine("=================================================================");
            writer.WriteLine("");
            writer.WriteLine("");

            writer.Close();
        }
    }
}
