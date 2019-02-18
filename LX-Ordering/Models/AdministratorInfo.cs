using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class AdministratorInfo
    {
        //编号
        [Key]
        public int Id { get; set; }
        //姓名
        [Display(Name="用户名")]
        [Required(ErrorMessage ="用户名不能为空")]
        public string Name { get; set; }
        //密码
        [Display(Name = "密码")]
        [RegularExpression("^.{6}$",ErrorMessage ="密码为6为字符")]
        public string Pwd { get; set; }
    }
}