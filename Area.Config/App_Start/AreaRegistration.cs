﻿using System.Web.Mvc;

namespace AspNetIdentityRoles
{
    public class ConfigAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Config";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Config_default",
                "Config/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "AspNetIdentityRoles.Area.Config.Controllers" }
            );
        }
    }
}