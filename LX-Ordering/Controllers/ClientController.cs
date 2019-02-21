using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LX_Ordering.Models;
using Newtonsoft.Json;

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

        public ActionResult SingleA(int Did = 0, int PageIndex = 1, int PageSize = 3)
        {
            //获取评价信息
            List<EvaluateInfo> evaluatelist = CommonGet<EvaluateInfo>.GetList();
            //模糊查询
            if (Did > 0)
            {
                evaluatelist = (from a in evaluatelist where a.Did.Equals(Did) select a).ToList();
            }
            //总记录数
            int Count = evaluatelist.Count();
            //总页数
            ViewBag.PageCount = Math.Ceiling(Count * 1.0 / PageSize);
            ViewBag.PageIndex = PageIndex;
            ViewBag.Count = Count;
            ViewBag.PageSize = PageSize;
            return View(evaluatelist.Skip((PageIndex - 1) * PageSize).Take(PageSize));
        }
        [HttpGet]//添加评价
        public ActionResult SingleAdd()
        {          
            return View();
        }
        [HttpPost]//添加评价
        public void SingleAdd(EvaluateInfo evaluate)
        {
            //评价时间
            evaluate.ETime = DateTime.Now;
            var url = "api/OrderAPI/AddEvaluate";
            string data = JsonConvert.SerializeObject(evaluate);
            int result = Convert.ToInt32(HttpClientHelper.SendRequest(url, "post", data));
            if (result > 0)
            {
                Response.Write("<script>alert('评价成功');location.href='/Client/SingleA';</script>");
            }
            else
            {
                Response.Write("<script>alert('评价失败')</script>");
            }
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
        //地图方法
        public ActionResult Map()
        {   
            List<Logistics> logisticslist = CommonGet<Logistics>.GetList();           
            return View(logisticslist);
        }
    }
}