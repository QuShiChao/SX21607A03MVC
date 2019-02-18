using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class DishInfo
    {
        //编号
        [Key]
        public int Id { get; set; }
        //名称
        [Display(Name ="菜肴名称")]
        [Required(ErrorMessage ="不能为空")]
        public string Name { get; set; }
        //价格
        [Display(Name = "菜肴名称")]
        [Required(ErrorMessage = "不能为空")]
        public decimal Price { get; set; }
        //菜系ID
        [Display(Name = "菜肴类型")]
        [Required(ErrorMessage = "不能为空")]
        [ForeignKey("Cata")]
        public int Cid { get; set; }
        public virtual Catagery Cata { get; set; }
        //图片路径
        [Display(Name = "图片")]
        [Required(ErrorMessage = "不能为空")]
        public string Image { get; set; }
        //菜色介绍
        [Display(Name = "详细介绍")]
        [Required(ErrorMessage = "不能为空")]
        public string Intro { get; set; }
        //菜色状态：0有，1无，2，特色菜
        [Display(Name = "状态")]
        [Required(ErrorMessage = "不能为空")]
        public int Status { get; set; }
        //订购次数
        [Display(Name = "订购次数")]
        [Required(ErrorMessage = "不能为空")]
        public int OrderNums { get; set; }
        //被顶次数
        [Display(Name = "好评")]
        [Required(ErrorMessage = "不能为空")]
        public int Up { get; set; }
        //被踩次数
        [Display(Name = "差评")]
        [Required(ErrorMessage = "不能为空")]
        public int Down { get; set; }
    }
}