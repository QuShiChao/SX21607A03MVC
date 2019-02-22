using System;
using System.Collections.Generic;
using System.Dynamic;
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
        //商品浏览
        public ActionResult Index(int pageIndex=1)
        {
            //int pageSize = 3;
            List<OrderInfo> dishList = CommonGet<OrderInfo>.GetList();
            //int count = dishList.Count();
            //int pagecount = Convert.ToInt32(Math.Ceiling(count * 1.0 / pageSize));//多少页
            //ViewBag.pageIndex = pageIndex;//当前页
            //ViewBag.pageSize = pageSize;//每页多少条数据
            //ViewBag.pageCount = pagecount;//多少页
            //ViewBag.count = count;//多少条
            //var pagelist = dishList.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return View(dishList);
            
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
        //订单信息 方法
        public List<OrderInfo> GetOrders()
        {
            var result = from o in CommonGet<OrderInfo>.GetList()
                         join d in CommonGet<DishInfo>.GetList() on o.Did equals d.Id
                         join c in CommonGet<ClientInfo>.GetList() on d.Cid equals c.Id
                         select new 
                         {
                             Id = o.Id,
                             Cid=d.Cid,
                             Did=d.Id,
                             Name = d.Name,
                             Intro=d.Intro,
                             Statuss=d.Status,
                             OrderNums=d.OrderNums,
                             Up=d.Up,
                             Down=d.Down,
                             Num = o.Num,
                             Total = o.Total,
                             Leave = o.Leave,
                             Status = o.Status,
                             OrderTime = o.OrderTime,
                             OrderType=o.OrderType,
                             Addr = c.Addr,
                             HeadImage=c.HeadImage
                         };
            List<OrderInfo> orderList = JsonConvert.DeserializeObject<List<OrderInfo>>(JsonConvert.SerializeObject(result));
            return orderList;
        }

        
        //List<OrderInfo> orderList1 = CommonGet<OrderInfo>.GetList().Where(c => c.Status.Equals(0)).ToList();

        //添加至购物车
        public int AddToCar(OrderInfo order)
        {
           
            string json = JsonConvert.SerializeObject(order);
            string result = HttpClientHelper.SendRequest("api/OrderAPI/AddOrder", "post", json);
            int s = Convert.ToInt32(result);
            if (s > 0)
            {
                Response.Write("<script>alert('添加购物车成功');location.href='/Client/Index'</script>");
            }
            else
            {
                Response.Write("<script>alert('添加购物车失败')</script>");
            }
            return s;
        }
        //购物车订单删除
        public int DeleteOrder(int Id)
        {
            string result = HttpClientHelper.SendRequest("api/OrderAPI/DelOrder/" + Id, "delete");
            int results = Convert.ToInt32(result);
            if (results > 0)
            {
                Response.Write("<script>alert('删除订单成功')</script>");
            }
            else
            {
                Response.Write("<script>alert('删除订单失败')</script>");

            }
            return results;
        }
        //清空购物车
        //public void EmptyCar()
        //{
        //    orderList1.Clear();
            
        //}
        //购物车所有商品
        public ActionResult ShopCar()
         {
            //未付款        
            List<OrderInfo> orderList = CommonGet<OrderInfo>.GetList().Where(c=>c.Status.Equals(0)).ToList();
          
            int orderNum = orderList.Count();//购物车里 总数                             
            //  Session["orderCar"] = orderList;
            int oId = (Session["Order"] as OrderInfo).Id;//订单id
            ViewBag.Ids = oId;
            ViewBag.orderNum = orderNum;
            int Num = 1;
            ViewBag.Num =Num;//商品的数量
            //ViewBag.Total = order.Total;
            return View(orderList);
        }
        //修改购物车中的数量 +
        public void ShopNumAdd(int Id)
        {
         
            OrderInfo order = CommonGet<OrderInfo>.GetList().Where(c => c.Id.Equals(Id)).FirstOrDefault();
            Id=order.Id;
            if (order.Num >= 0)
            {
                order.Num++;
                string jsonstr = JsonConvert.SerializeObject(order);
                string result = HttpClientHelper.SendRequest("api/ClientAPIOrderAPI/UpOrder?Id=" + Id, "put", jsonstr);
                if (int.Parse(result) > 0)
                {
                    Response.Write("<script>alert('')</script>;location.href='/Client/ShopCar'");
                }
            }
        }
        //修改数量 -
        public void ShopNumReduce(int Id)
        {
         
            OrderInfo order = CommonGet<OrderInfo>.GetList().Where(c => c.Id.Equals(Id)).FirstOrDefault();
            Id = order.Id;
            if (order.Num >= 0)
            {
                order.Num--;
                string jsonstr = JsonConvert.SerializeObject(order);
                string result = HttpClientHelper.SendRequest("api/ClientAPIOrderAPI/UpOrder?Id=" + Id, "put", jsonstr);
                if (int.Parse(result) > 0)
                {
                    Response.Write("<script>alert('')</script>;location.href='/Client/ShopCar'");
                }
            }  
        }
        //下单操作 xxxxx
        //public decimal Pay(int Id)
        //{
        //    List<OrderInfo> orderList = CommonGet<OrderInfo>.GetList() ;
        //    List<DishInfo> dishList = CommonGet<DishInfo>.GetList();
        //    OrderInfo order = orderList.Where(c => c.Id == Id).FirstOrDefault();
        //    DishInfo dish = dishList.Where(c => c.Id == Id).FirstOrDefault();

        //    return  order.Total = order.Num * dish.Price;//总价
        //}
        //所有订单信息 +物流信息(已下单)
        public ActionResult Ordering()
        {
            //已付款
            List<OrderInfo> orderList = CommonGet<OrderInfo>.GetList().Where(c=>c.Status==1).ToList();
            return View(orderList);
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