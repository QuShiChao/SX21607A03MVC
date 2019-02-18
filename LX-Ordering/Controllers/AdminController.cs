using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LX_Ordering.Models;

namespace LX_Ordering.Controllers
{
    public class AdminController : Controller
    {
        //管理员主页面
        public ActionResult Index()
        {
            return View();
        }
        //注册
        public ActionResult AdminRegist()
        {
            return View();
        }
        [HttpPost]
        public int AdminRegist(AdministratorInfo admin)
        {
            return 0;
        }
        //登录
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public int AdminLogin(AdministratorInfo admin)
        {
            return 0;
        }
        //添加菜系
        public ActionResult AddCatagery()
        {
            return View();
        }
        [HttpPost]
        public int AddCatagery(Catagery catagery)
        {
            return 0;
        }
        //查看菜系
        public ActionResult GetCatagery()
        {
            return View();
        }
        //编辑菜系
        public ActionResult UpdCatagery(int id)
        {
            return View();
        }
        [HttpPost]
        public int UpdCatagery(Catagery catagery)
        {
            return 0;
        }
        //添加菜色
        public ActionResult AddDish()
        {
            //s
            return View();
        }
        [HttpPost]
        public int AddDish(DishInfo dish)
        {
            return 0;
        }
        //查看菜色
        public ActionResult GetDish()
        {
            return View();
        }
        //编辑菜色信息
        public ActionResult UpdDish(int id)
        {
            return View();
        }
        [HttpPost]
        public int UpdDish(Catagery catagery)
        {
            return 0;
        }
        //订单查看
        public ActionResult GetOrder()
        {
            return View();
        }
        //订单修改
        public ActionResult UpdOrder(int id)
        {
            return View();
        }
        [HttpPost]
        public int UpdOrder(OrderInfo order)
        {
            return 0;
        }
    }
}