using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LX_Ordering.Controllers
{
    public class MyAuthrizationAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["tel"] == null)
            {
                var context = new ContentResult();
                context.Content = "<script>location.href='/Admin/Index';</script>";
                filterContext.Result = context;
            }
        }
    }
}