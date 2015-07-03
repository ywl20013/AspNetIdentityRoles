using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;

namespace AspNetIdentityRoles
{
    /// <summary>
    /// 根据根目录下的ActionRoles.xml的配置授权检查
    /// <remarks>先检查用户名，再检查角色</remarks>
    /// </summary>
    public class PermissionAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
        public new string[] Users { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            //if (Roles == null && Users == null)
            //{
            //    return true;
            //}
            //if (Roles.Length == 0 & Users.Length == 0)
            //{
            //    return true;
            //}

            //if (!httpContext.User.Identity.IsAuthenticated)
            //{
            //    return false;
            //}

            if (Users != null && Users.Length > 0)
            {
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    return false;
                }
                var q = Users.Where(u => u.ToLower().Contains(httpContext.User.Identity.Name.ToLower()));
                if (q.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (Roles != null && Roles.Length > 0)
            {
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    return false;
                }
                if (Roles.Any(httpContext.User.IsInRole))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.AuthorizeCore(httpContext);

        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string roles = GetRoles.GetActionRoles(actionName, controllerName);
            string users = GetRoles.GetActionUsers(actionName, controllerName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                this.Roles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (!string.IsNullOrWhiteSpace(users))
            {
                this.Users = users.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            base.OnAuthorization(filterContext);
        }
    }
    public class GetRoles
    {
        public static string GetActionRoles(string action, string controller)
        {
            XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
            XElement controllerElement = findElementByAttribute(rootElement, "Controller", controller);
            if (controllerElement != null)
            {
                XElement actionElement = findElementByAttribute(controllerElement, "Action", action);
                if (actionElement != null)
                {
                    XElement rolesElement = findElementByTagName(actionElement, "Roles");
                    if (rolesElement != null)
                        return rolesElement.Value;
                }
            }
            return "";
        }

        public static string GetActionUsers(string action, string controller)
        {
            XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
            XElement controllerElement = findElementByAttribute(rootElement, "Controller", controller);
            if (controllerElement != null)
            {
                XElement actionElement = findElementByAttribute(controllerElement, "Action", action);
                if (actionElement != null)
                {
                    XElement usersElement = findElementByTagName(actionElement, "Users");
                    if (usersElement != null)
                        return usersElement.Value;
                }
            }
            return "";
        }

        public static XElement findElementByAttribute(XElement xElement, string tagName, string attribute)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
        }

        public static XElement findElementByTagName(XElement xElement, string tagName)
        {
            return xElement.Elements(tagName).FirstOrDefault(x => x.Name.LocalName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }
    }
}