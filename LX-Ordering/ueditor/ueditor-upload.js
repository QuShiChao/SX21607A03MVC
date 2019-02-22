$.fn.upload = function (path,img) {
    var me = this;
    $(me).before('<input class="fup" type="file" name="upfile"/>');
    $(me).before('<div id="divImg"></div>');
    if(img!=undefined){
       $(me).prev().append('<img src="' + img + '" style="width:100px;height:100px"/>');
       $(me).val(img);
    }
    $(this).hide();

    var Form = $(this).parents().filter("form");
    if (path == undefined) {
        path = "/ueditor/net/";
    }
    //var path = "/ueditor/net/";; //这是Controller.ashx 的存放路径  文件域的名字 只能是upfile是不允许修改

    $(".fup").change(function () {
        Form.ajaxSubmit({    //获取表单ID 执行 异步提交 ajaxSubmit  注意表单 id
            url: path + 'controller.ashx',
            type: 'post',
            data: { action: 'uploadimage' },
            dataType: 'json',
            success: function (a) {
                if (a.state == "SUCCESS") {
                    var imgpath = path + a.url;
                    $(me).prev().empty();
                    $(me).prev().append('<img src="' + imgpath + '" style="width:100px;height:100px"/>');
                    $(me).val(imgpath);
                }
                else {
                    alert(a.state);
                }
            }
        });
    });
}