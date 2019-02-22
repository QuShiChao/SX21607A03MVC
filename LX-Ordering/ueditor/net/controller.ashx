<%@ WebHandler Language="C#" Class="UEditorHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

public class UEditorHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        Handler action = null;
        switch (context.Request["action"])
        {
            case "config":
                action = new ConfigHandler(context);
                break;
            case "uploadimage":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("imageAllowFiles"),
                    PathFormat = Config.GetString("imagePathFormat"),
                    SizeLimit = Config.GetInt("imageMaxSize"),
                    UploadFieldName = Config.GetString("imageFieldName")
                });
                break;
            case "uploadscrawl":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = new string[] { ".png" },
                    PathFormat = Config.GetString("scrawlPathFormat"),
                    SizeLimit = Config.GetInt("scrawlMaxSize"),
                    UploadFieldName = Config.GetString("scrawlFieldName"),
                    Base64 = true,
                    Base64Filename = "scrawl.png"
                });
                break;
            case "uploadvideo":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("videoAllowFiles"),
                    PathFormat = Config.GetString("videoPathFormat"),
                    SizeLimit = Config.GetInt("videoMaxSize"),
                    UploadFieldName = Config.GetString("videoFieldName")
                });
                break;
            case "uploadfile":
                action = new UploadHandler(context, new UploadConfig()
                {
                    AllowExtensions = Config.GetStringList("fileAllowFiles"),
                    PathFormat = Config.GetString("filePathFormat"),
                    SizeLimit = Config.GetInt("fileMaxSize"),
                    UploadFieldName = Config.GetString("fileFieldName")
                });
                break;
            case "listimage":
                action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                break;
            case "listfile":
                action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                break;
            case "catchimage":
                action = new CrawlerHandler(context);
                break;
            default:
                action = new NotSupportedHandler(context);
                break;
        }
        action.Process();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

//  页面使用说明
//  1、  把下列代码复制到 图片路径所在位置
//  2、  修改 FID 为 表单ID 或 form 
//  3、  修改 FILEID 为 保存图片路径的文本框的ID  并把文本框修改为 隐藏域 type='text' 改为 type='hidden'
//       把   path  设置为 ueditor中net的真实路径
//  4、  <form>标记中添加 method="post" enctype="multipart/form-data" 两个属性 如下示例 
//       <form id="f1" method="post" enctype="multipart/form-data">
//  5、  在页面上 添加jquery.form.js 文件 并放置在 JQUERY.js下面

    //<script>
                            //function dd() {
                            //    var fid = "form";
                            //    var fileId = "#txtPhoto";
                            //    var path = "/Content/ueditor/net/";; //这是Controller.ashx 的存放路径  文件域的名字 只能是upfile是不允许修改

                            //    $(fid).ajaxSubmit({    //获取表单ID 执行 异步提交 ajaxSubmit  注意表单 id
                            //        url: path+'controller.ashx',
                            //        type: 'post',
                            //        data: { action: 'uploadimage' },
                            //        dataType: 'json',
                            //        success: function (a) {
                            //            if (a.state == "SUCCESS") {
                            //                var imgpath = path + a.url;
                            //                $("#divImg").append('<img src="' + imgpath + '" style="width:100px;height:100px"/>');
                            //                $(fileId).val(imgpath);
                            //            }
                            //            else {
                            //                alert(a.state);
                            //            }
                            //        }
                            //    });
                            //}
    //</script>

    //// 文件域 和 预览DIV ，放在指定的 form 表单中
    //        <input type="file" name="upfile" onchange="dd()" />
    //        <div id="divImg"></div>