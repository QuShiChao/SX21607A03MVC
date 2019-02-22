using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LX_Ordering.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net;
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
        //发送手机短信
        //调用时只需要把拼成的URL传给该函数即可。判断返回值即可
        public string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }
        //获取验证码
        public void GetYz(string Tel = "")
        {
            Random n = new Random();
            int m = n.Next(1000, 10000);
            Session["Random"] = m;
            string url = string.Format("http://utf8.api.smschinese.cn");
            string end = string.Format("/?Uid={0}&Key={1}&smsMob={2}&smsText=验证码:{3}", "pikachu", "d41d8cd98f00b204e980", Tel, m);
            string r = GetHtmlFromUrl(url + end);
        }

        /// <summary>
        /// 客户注册
        /// </summary>
        /// <param name="a">县级PID</param>
        /// <param name="c">市级PID</param>
        /// <param name="p">省级PID</param>
        /// /// <param name="client">客户信息</param>
        /// <param name="pwd">二次密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            
            List<AddressInfo> addressList = CommonGet<AddressInfo>.GetList().Where(y => y.Pid.Equals(0)).ToList();//获取地址
            ViewBag.P = new SelectList(addressList, "Id", "Name");
            ViewBag.C = new SelectList(new List<AddressInfo>(), "Id", "Name");
            ViewBag.A = new SelectList(new List<AddressInfo>(), "Id", "Name");
            return View();
        }
        //获取地址信息
        public string GetDropList(int id)
        {
            List<AddressInfo> addrList = CommonGet<AddressInfo>.GetList().Where(y => y.Pid.Equals(id)).ToList();
            return JsonConvert.SerializeObject(addrList);
        }
        [HttpPost]
        public void Register(ClientInfo client,string Y="")
        {
            int result = 0;
            if (Y != Session["Random"].ToString())//验证码判断 Session["Random"].ToString()
            {
                Response.Write("<script>alert('验证码不正确')</script>");
            }
            else
            {
                List<ClientInfo> clientInfo = CommonGet<ClientInfo>.GetList();//获取用户表信息
                int one = clientInfo.Where(c => c.Tel.Equals(client.Tel)).Count();//查询手机号是否已注册

                if (one > 0)
                {
                    Response.Write("<script>alert('手机号已注册')</script>");
                }
                else
                {
                    string json = JsonConvert.SerializeObject(client);
                    result = Int32.Parse(HttpClientHelper.SendRequest("api/OrderAPI/AddClient", "post", json));
                }
            }
            if (result > 0)
            {
                Session["Random"] = null;
                Response.Write("<script>alert('注册成功,前往登陆');location.href='Login'</script>");

            }
            else
            {
                Response.Write("<script>alert('注册失败')</script>");
            }
        }
        /// <summary>
        /// 客户登录
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string tel, string pwd)
        {
            List<ClientInfo> client = CommonGet<ClientInfo>.GetList();//获取用户表信息
            int one = client.Where(c => c.Tel.Equals(tel)).Count();//查询手机号是否已注册
            int result = 0;
            if (one == 0)//当客户未注册登录时密码为手机号后四位
            {
                try
                {
                    if (pwd == tel.Substring(tel.Length - 4, tel.Length))
                    {
                        result = 1;
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                ClientInfo clientInfo = client.Where(c => c.Tel.Equals(tel) && c.Pwd.Equals(pwd)).FirstOrDefault();
                Session["Clientid"] = clientInfo.Id;//将登陆人的信息存入session
                Session["ClientName"] = clientInfo.Name;
                result = 1;
            }
            if (result > 0)
            {
                Response.Write("<script>location.href='Index'</script>");
            }
            else
            {
                Response.Write("<script>alert('很遗憾登陆失败了,请检查网络连接')</script>");
            }
            return View();
        }
        /// <summary>
        /// 退出当前账号
        /// </summary>
        public void Exit()
        {
            Session["Clientid"] = null;
            Session["ClientName"] = null;
            Response.Write("<script>alert('成功退出该账号');location.href='Login'</script>");
        }
        /// <summary>
        /// 客户注销
        /// </summary>
        public void ClientDel()
        {
            int id = Int32.Parse(Session["Clientid"].ToString());//获取当前账号的ID
            int result = Int32.Parse(HttpClientHelper.SendRequest("api/OrderAPIController/DelClient?id=" + id, "del"));
            if (result > 0)
            {
                Session["Clientid"] = null;
                Session["ClientName"] = null;
                Response.Write("<script>alert('账号已成功注销');location.href='Login'</script>");
            }
            else
            {
                Response.Write("<script>alert('注销失败了')</script>");
            }
        }
        /// <summary>
        /// 客户修改
        /// </summary>
        /// <param name="client">客户信息</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpInfo()
        {
            List<AddressInfo> addressList = CommonGet<AddressInfo>.GetList().Where(y => y.Pid.Equals(0)).ToList();//获取地址
            ViewBag.P = new SelectList(addressList, "Id", "Name");
            ViewBag.C = new SelectList(new List<AddressInfo>(), "Id", "Name");
            ViewBag.A = new SelectList(new List<AddressInfo>(), "Id", "Name");
            int id = Int32.Parse(Session["Clientid"].ToString());//获取当前账号的ID
            ClientInfo client =  CommonGet<ClientInfo>.GetList().Where(c => c.Id.Equals(id)).FirstOrDefault();//获取用户信息
            return View(client);
        }
        [HttpPost]
        public ActionResult UpInfo(ClientInfo client)
        {
            string json = JsonConvert.SerializeObject(client);
            int result = Int32.Parse(HttpClientHelper.SendRequest("api/OrderAPIController/UpdClient", "post", json));
            if (result > 0)
            {
                Response.Write("<script>alert('恭喜您,个人信息修改成功');location.href='Client/Login'</script>");
            }
            else
            {
                Response.Write("<script>alert('修改失败')</script>");
            }
            return View();
        }

        //联系
        public ActionResult Contact(string name="",string tel = "",string city="",string remark="")
        {
            if (name != "" & tel != "" & city != "")
            {
                string str = string.Format("={0}的{1}给你提议:{2}。他的联系方式{3}", city, name, remark, tel);
                string url = string.Format("http://utf8.api.smschinese.cn");
                string end = string.Format("/?Uid=pikachu&Key=d41d8cd98f00b204e980&smsMob=15954563953&smsText={0}", str);
                string r = GetHtmlFromUrl(url + end);
                Response.Write("<script>alert('发送成功')</script>");
            }
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
    }
}