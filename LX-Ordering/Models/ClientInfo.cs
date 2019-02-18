using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LX_Ordering.Models
{
    public class ClientInfo
    {
        //编号
        [Key]
        [Display(Name="客户编号")]
        public int Id { get; set; }
        //昵称
        [Display(Name="昵称")]
        [Required(ErrorMessage ="不能为空")]
        public string Name { get; set; }
        //手机号码
        [Display(Name ="手机号码")]
        [Required(ErrorMessage ="不能为空")]
        [RegularExpression("^1{1}[0-9]{10}$", ErrorMessage = "手机号不正确")]
        public string Tel { get; set; }
        //密码默认为手机号码后四位
        [Display(Name = "密码")]
        [Required(ErrorMessage = "不能为空")]
        [RegularExpression("^.{4}$", ErrorMessage = "密码为4位字符")]
        public string Pwd { get; set; }
        //省份Id
        [Display(Name ="地址")]
        [ForeignKey("P")]
        public int Pid { get; set; }
        public virtual AddressInfo P { get; set; }
        //市级Id
        [ForeignKey("C")]
        public int Cid { get; set; }
        public virtual AddressInfo C { get; set; }
        //县级Id
        [ForeignKey("A")]
        public int Aid { get; set; }
        public virtual AddressInfo A { get; set; }
        //详细地址
        [Display(Name = "详细地址")]
        [Required(ErrorMessage = "不能为空")]
        public string Addr { get; set; }
        //头像
        [Display(Name = "头像")]
        [Required(ErrorMessage = "不能为空")]
        public string HeadImage { get; set; }
    }
}