using System;
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
            return View();
        }
        //注册
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
        public ActionResult AddDish()
        {
            //获取菜系集合
            List<Catagery> cataList = CommonGet<Catagery>.GetList();
            ViewBag.Cata = new SelectList(cataList, "Id", "Name");
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