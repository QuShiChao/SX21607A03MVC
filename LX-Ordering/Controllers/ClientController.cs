using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LX_Ordering.Models;

namespace LX_Ordering.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client

        public ActionResult Index()
        {
            return View();
        }

        //用户登陆
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //登陆
        [HttpPost]
        public ActionResult Login(string name, string pwd)
        {
            return View();
        }
        //用户注册
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //注册
        [HttpPost]
        public ActionResult Register(ClientInfo client)
        {
            return View();
        }
        //联系
        public ActionResult Contact()
        {
            return View();
        }
        //购物车
        public ActionResult ShopCar()
        {
            return View();
        }
        //修改购物车中的数量(无视图)
        public ActionResult ShopNum(int Id)
        {
            return View();
        }
        //所有订单信息 +物流信息
        public ActionResult Ordering()
        {
            return View();
        }
        //查看商品的所有评价（）

        public ActionResult SingleA()
        {
            return View();
        }
        [HttpGet]//添加评价
        public ActionResult SingleAdd()
        {
            return View();
        }
        [HttpPost]//添加评价
        public ActionResult SingleAdd(DishInfo dish)
        {
            return View();
        }
        //反填
        [HttpGet]
        public ActionResult UpInfo(string Id)//参数应是int类型  用到改回int类型
        {
            return View();
        }
        //修改信息
        [HttpPost]
        public ActionResult UpInfo(ClientInfo client)
        {
            return View();
        }
    }
}