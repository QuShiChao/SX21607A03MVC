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
        public void AddCatagery(Catagery catagery)
        {
            string str = "api/OrderAPI/AddCata";
            var cata = JsonConvert.SerializeObject(catagery);
            int result = Convert.ToInt32(HttpClientHelper.SendRequest(str, "post", cata));
            if (result > 0)
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
            string str = "api/OrderAPI/DelCata?id=" + id;
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
            List<Catagery> CataList = ShowCata().Where(s => s.Id.Equals(id)).ToList();
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
            return View();
        }
        //订单修改
        public ActionResult UpdOrder(int id)
        {
            return View();
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
            return View();
        }
        [HttpPost]
        public int UpdOrder(OrderInfo order)
        {
            return 0;
        }
    }
}