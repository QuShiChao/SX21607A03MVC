using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LX_Ordering.Controllers
{
    public class CommonGet<T>
    {
        //获取指定集合信息
        public static List<T> GetList()
        {
            Type type = typeof(T);
            string url = null;
            switch (type.Name)
            {
                case "AddressInfo":
                    url = "api/OrderAPI/GetAddr";
                    break;
                case "AdministratorInfo":
                    url = "api/OrderAPI/GetAdmin";
                    break;
                case "Catagery":
                    url = "api/OrderAPI/GetCatagery";//查看菜系信息
                    break;
                case "ClientInfo":
                    url = "api/OrderAPI/GetClient";//查看客户信息
                    break;
                case "DishInfo":
                    url = "api/OrderAPI/GetDish";//查看菜色信息
                    break;
                case "EvaluateInfo":
                    url = "api/OrderAPI/GetEvaluate";//查看评价地址
                    break;
                case "Logistics":
                    url = "api/OrderAPI/GetLogistics";//查看物流信息
                    break;
                case "OrderInfo":
                    url = "api/OrderAPI/GetOrder";//查看订单信息
                    break;
            }
            string json = HttpClientHelper.SendRequest(url, "get");
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }
    }
}