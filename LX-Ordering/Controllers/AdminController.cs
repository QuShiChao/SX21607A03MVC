﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LX_Ordering.Models;
using LX_Ordering;
using Newtonsoft.Json;

namespace LX_Ordering.Controllers
{
    public class AdminController : Controller
    {
        List<OrderInfo> OrderList = CommonGet<OrderInfo>.GetList();//订餐系统
        List<EvaluateInfo> EvalList = CommonGet<EvaluateInfo>.GetList();//评价系统
        //管理员主页面
        //[AdminAuthrization]
        public ActionResult Index()
        {
            ViewBag.count=OrderList.Count();
            string total = (from a in OrderList
                        group a by a.Id into t
                        select new
                        {
                            Sum = t.Sum(a => a.Total)
                        }).ToString();
            ViewBag.sum = total;
            return View(OrderList);
        }
        //注册
        [AdminAuthrization]
        public ActionResult AdminRegist()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminRegist(AdministratorInfo admin,string Zid,string Key)
        {
            int result = 0;
            if (Zid == "123456" && Key == "123456")
            {
                string json = JsonConvert.SerializeObject(admin);
                result = Int32.Parse(HttpClientHelper.SendRequest("api/OrderAPI/AddAdmin", "post", json));
            }
            if (result > 0)
            {
                Response.Write("<script>alert('注册成功');location.href='AdminLogin'</script>");
            }
            else
            {
                Response.Write("<script>alert('注册失败')</script>");
            }
            return View();
        }
        //登录
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(string name,string pwd)
        {
            List<AdministratorInfo> client = CommonGet<AdministratorInfo>.GetList();
            int result = client.Where(c => c.Name.Equals(name) && c.Pwd.Equals(pwd)).Count();//查询是否存在
            if (result > 0)
            {
                Session["AdminName"] = client.FirstOrDefault().Name;
                Response.Write("<script>alert('登陆成功');location.href='Index'</script>");
            }
            else
            {
                Response.Write("<script>alert('登陆失败')</script>");
            }
            return View();
        }
        //菜肴操作页面
        public ActionResult ShowCatagery()
        {
            return View();
        }
        //添加菜系
        [AdminAuthrization]
        public ActionResult AddCatagery()
        {
            return View();
        }
        [HttpPost]
        public void AddCatagery(Catagery catagery)
        {
            string str = "api/OrderAPI/AddCata";
            var cata = JsonConvert.SerializeObject(catagery);
            int result=Convert.ToInt32(HttpClientHelper.SendRequest(str,"post",cata));
            if (result>0)
            {
                Response.Write("<script>alert('添加成功！');location.href='/Admin/';</script>");
            }
            else
            {
                Response.Write("<script>alert('添加失败！');location.href='/Admin/';</script>");
            }
        }
        //查看菜系
        public ActionResult GetCatagery()
        {
            List<Catagery> CataList = ShowCata();
            return View(CataList);
        }
        //查询所有菜系
        private static List<Catagery> ShowCata()
        {
            string str = "api/OrderAPI/GetCatagery";
            List<Catagery> CataList = JsonConvert.DeserializeObject<List<Catagery>>(HttpClientHelper.SendRequest(str, "get"));
            return CataList;
        }
        public void DelCatagery(int id)
        {
            string str = "api/OrderAPI/DelCata?id="+id;
            int result = Convert.ToInt32(HttpClientHelper.SendRequest(str, "delete"));
            if (result > 0)
            {
                Response.Write("<script>alert('删除成功！');location.href='/Admin/';</script>");
            }
            else
            {
                Response.Write("<script>alert('删除失败！');location.href='/Admin/';</script>");
            }
        }
        //编辑菜系
        public ActionResult UpdCatagery(int id)
        {
            List<Catagery> CataList = ShowCata().Where(s=>s.Id.Equals(id)).ToList();
            return View(CataList);
        }
        [HttpPost]
        public void UpdCatagery(Catagery catagery)
        {
            string str = "api/OrderAPI/UpdCata";
            var cata = JsonConvert.SerializeObject(catagery);
            int result = Convert.ToInt32(HttpClientHelper.SendRequest(str, "put", cata));
            if (result > 0)
            {
                Response.Write("<script>alert('修改成功！');location.href='/Admin/';</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失败！');location.href='/Admin/';</script>");
            }
        }
        //添加菜色
        [AdminAuthrization]
        public ActionResult AddDish(int id = 0)
        {
            ViewBag.id = id;
            //获取菜系集合
            List<Catagery> cataList = CommonGet<Catagery>.GetList();
            ViewBag.Cata = cataList;// new SelectList(cataList, "Id", "Name");
            return View();
        }
        public string UpdDish(int id)
        {
            DishInfo dish = dishList.FirstOrDefault(d => d.Id.Equals(id));
            string json = JsonConvert.SerializeObject(dish);
            return json;
        }
        [HttpPost]
        public void AddDish(DishInfo dish)
        {
            string json = JsonConvert.SerializeObject(dish);
            string result = null;
            if (dish.Id == 0)
            {

                result = HttpClientHelper.SendRequest("api/OrderAPI/AddDish", "post", json);

            }
            else
            {
                result = HttpClientHelper.SendRequest("api/OrderAPI/UpdDish", "put", json);
            }
            if (int.Parse(result) > 0)
            {
                Response.Write("<script>location.href='/Admin/';</script>");
                //Response.Write("<script>layer.msg('OK');</script>");
            }
            else
            {
                Response.Write("<script>alert('×');location.href='/Admin/AddDish';</script>");
            }
        }
        List<DishInfo> dishList = CommonGet<DishInfo>.GetList();
        //查看菜肴信息
        [AdminAuthrization]
        public ActionResult ShowDish(int pageIndex = 1, int pageSize = 8, string name = "")
        {
            if (name != "")
            {
                dishList = dishList.Where(d => d.Name.Contains(name)).ToList();
            }
            int count = dishList.Count;
            ViewBag.pageCount = Math.Ceiling(count * 1.0 / pageSize);
            ViewBag.pageIndex = pageIndex;
            return View(dishList.Skip((pageIndex - 1) * pageSize).Take(pageSize * pageIndex));
        }
        //菜肴信息删除
        public int DelDish(int id)
        {
            string result = HttpClientHelper.SendRequest("api/OrderAPI/DelDish?id=" + id, "delete");
            if (result != "")
            {
                return int.Parse(result);
            }
            else
            {
                return 0;
            }
        }
        //订单查看
        public ActionResult GetOrder()
        {
           OrderList = OrderList.Where(s => s.Status != 0).ToList();
            return View(OrderList);
        }
        //订单修改
        public void UpdOrder(int id)
        {
            OrderInfo order = OrderList.Where(s => s.Id.Equals(id)).FirstOrDefault();
            order.Status = 3;
            Logistics logis = new Logistics();
            logis.Oid = id;
            logis.Status = 2;
            logis.Ltime = DateTime.Now;
            Upd(order,logis);
        }
        public void UpdOrder1(int id)
        {
            OrderInfo order = OrderList.Where(s => s.Id.Equals(id)).FirstOrDefault();
            order.Status = 2;
            Logistics logis = new Logistics();
            logis.Oid = id;
            logis.Status = 1;
            logis.Ltime = DateTime.Now;
            Upd(order,logis);
        }
        //修改订单和物流信息方法
        private void Upd(OrderInfo order,Logistics logis)
        {
            string orderstr = JsonConvert.SerializeObject(order);
            string logisstr = JsonConvert.SerializeObject(logis);
            int result = Convert.ToInt32(HttpClientHelper.SendRequest("api/OrderAPI/UpdOrder", "put", orderstr));
            int result1 = Convert.ToInt32(HttpClientHelper.SendRequest("api/OrderAPI/AddLogistics", "post", logisstr));
            if (result > 0 && result1 > 0)
            {
                Response.Write("<script>alert('修改成功！');location.href='/Admin/';</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失败！');location.href='/Admin/';</script>");
            }
        }

        //经济收入页面
        public ActionResult GetIncome()
        {
            return View();
        }
        //菜肴销量页面
        public ActionResult GetSales()
        {
            return View();
        }
        public JsonResult ShowMoney()
        {
            OrderList = OrderList.Where(s => s.Status >= 2 && s.OrderTime > s.OrderTime.AddDays(-6)).ToList();
            var result = (from a in OrderList
                          group a by a.OrderTime.ToString("yyyy/mm/dd") into time
                          select new
                          {
                              Time = time.Key,
                              Sum = time.Sum(a => a.Total)
                          }).ToList();
            return Json(result);
        }
        //菜色评价
        public ActionResult LookEvaluate()
        {
            return View(EvalList);
        }
        //删除评价
        public void DelEvaluate(int id)
        {
            var result =Convert.ToInt32(HttpClientHelper.ReferenceEquals("api/OrderAPI/DelEvaluate?id=" + id, "delete"));
            if (result>0)
            {
                Response.Write("<script>alert('删除成功')</script>");
                Response.Redirect("/Admin/LookEvaluate");
            }
            else
            {
                Response.Write("<script>alert('删除成功')</script>");
                Response.Redirect("/Admin/LookEvaluate");
            }
        }
        [HttpPost]
        public int UpdOrder(OrderInfo order)//测试
        {
            return 0;
        }
    }
}