using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LX_Ordering.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        [MyAuthrization]
        public ActionResult Index()
        {
            return View();
        }
    }
}