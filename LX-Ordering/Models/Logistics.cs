using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class Logistics
    {
        [Key]
        public int Id { get; set; }
        //菜色Id
        [Display(Name ="订单")]
        [ForeignKey("Order")]
        public int Oid { get; set; }
        public virtual OrderInfo Order { get; set; }
        //状态 待接单0 待配送1 配送中2 已送达2
        [Display(Name ="状态")]
        public int Status { get; set; }
        //时间
        public DateTime Ltime { get; set; }
    }
}