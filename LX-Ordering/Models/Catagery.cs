using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class Catagery
    {
        //编号
        [Key]
        public int Id { get; set; }
        //类型名称
        [Display(Name ="菜系名称")]
        [Required(ErrorMessage ="不能为空")]
        public string Name { get; set; }
    }
}