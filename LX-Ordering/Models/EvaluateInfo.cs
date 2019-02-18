using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class EvaluateInfo
    {
        //编号
        [Key]
        [Display(Name ="编号")]
        public int Id { get; set; }
        //客户Id
        [Display(Name ="用户")]
        [ForeignKey("Client")]
        public int Cid { get; set; }
        public virtual ClientInfo Client { get; set; }
        //菜色Id
        [Display(Name ="菜肴")]
        [ForeignKey("Dish")]
        public int Did { get; set; }
        public virtual DishInfo Dish { get; set; }
        //评价内容
        [Display(Name ="评价内容")]
        public string Evaluate { get; set; }
        //评价时间
        [Display(Name ="评价时间")]
        public DateTime ETime { get; set; }
    }
}