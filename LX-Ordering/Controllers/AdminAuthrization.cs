using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LX_Ordering.Controllers
{
    public class AdminAuthrization: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminName"] == null)
            {
                var context = new ContentResult();
                context.Content = "<script>location.href='/Admin/AdminLogin';</script>";
                filterContext.Result = context;
            }
        }
    }
}