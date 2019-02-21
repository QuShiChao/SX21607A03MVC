using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class OrderInfo
    {
        //编号
        [Key]
        [Display(Name ="订单号")]
        public int Id { get; set; }
        //顾客Id
        [Display(Name = "顾客")]
        [ForeignKey("Client")]
        public int Cid { get; set; }
        public virtual ClientInfo Client { get; set; }
        //菜色Id
        [Display(Name = "菜肴")]
        [ForeignKey("Dish")]
        public int Did { get; set; }
        public virtual DishInfo Dish { get; set; }
        //数量
        [Display(Name = "数量")]
        [Required(ErrorMessage ="不能为空")]
        public int Num { get; set; }
        //订单类型：订餐0、就餐1
        [Display(Name = "数量")]
        [Required(ErrorMessage = "不能为空")]
        public int OrderType { get; set; }
        //总价
        [Display(Name = "总价")]
        [Required(ErrorMessage = "不能为空")]
        public decimal Total { get; set; }
        //留言
        [Display(Name = "备注")]
        [Required(ErrorMessage = "不能为空")]
        public string Leave { get; set; }
        //订单状态：购物车0、待付款1、已付款2、已接单、已发货3、未评价4、已评价5
        [Display(Name = "状态")]
        [Required(ErrorMessage = "不能为空")]
        public int Status { get; set; }
        //订购时间
        [Display(Name = "订购时间")]
        [Required(ErrorMessage = "不能为空")]
        public DateTime OrderTime { get; set; }
    }
}