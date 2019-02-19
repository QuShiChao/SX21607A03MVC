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
                    url = "api/OrderAPI/GetCatagery";
                    break;
                case "ClientInfo":
                    url = "api/OrderAPI/GetClient";
                    break;
                case "DishInfo":
                    url = "api/OrderAPI/GetDish";
                    break;
                case "EvaluateInfo":
                    url = "api/OrderAPI/GetEvaluate";
                    break;
                case "Logistics":
                    url = "api/OrderAPI/GetLogistics";
                    break;
                case "OrderInfo":
                    url = "api/OrderAPI/GetOrder";
                    break;
            }
            string json = HttpClientHelper.SendRequest(url, "get");
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }
    }
}