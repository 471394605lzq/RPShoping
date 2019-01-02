jQuery(document).ready(function () {
    var _m_id = $.query.get("m_id") || "";
    var _appid = getappid();
    var _appkey = getshaappkey();
    var imgkeylist = [];//商品图片集合
    //load();

    //function load() {
    //getproductsort(function (c, data) {
    //    if (c == "1") {
    //        var tempdata = JSON.stringify(data);
    //        tempdata = tempdata.substring(1, tempdata.length - 1);
    //        var t = JSON.parse("[" + tempdata + "]");
    //        $("#list5").jqGrid({
    //            datatype: "local",
    //            data: t,
    //            colNames: ['序号', '商品名称', '上架状态', '主图', '商品描述', '价格', '详情', '备注'],
    //            colModel: [
    //                { name: 'id', index: 'id', width: 55, align: "center", hidden: true },
    //                { name: 'P_Name', index: 'P_Name', width: 80, align: "center" },
    //                { name: 'p_state', index: 'p_state', width: 100, align: "center" },
    //                { name: 'P_Image', index: 'P_Image', width: 80, align: "center", hidden: true },
    //                { name: 'P_Explain', index: 'P_Explain', width: 155, align: "center" },
    //                { name: 'P_Price', index: 'P_Price', width: 120, align: "center" },
    //                { name: 'P_Details', index: 'P_Details', width: 120, align: "center", hidden: true },
    //                { name: 'P_Remark', index: 'P_Remark', width: 120, align: "center" }
    //            ],
    //            page: 1,
    //            rowNum: 30,
    //            rowList: [30, 50, 100],
    //            viewrecords: true,
    //            loadonce: true,
    //            pager: '#pager5',
    //            sortname: 'id',
    //            viewrecords: true,
    //            sortorder: "asc",
    //            //caption: "用户列表",
    //            editurl: "",
    //            shrinkToFit: true,
    //            autowidth: true,
    //            multiselect: true,
    //            refresh: true,
    //            //width:"100%",
    //            height: 650
    //        });
    //        getRoleModuleRight();
    //        //for (var i = 0; i < data.length; i++) {
    //        //    $("#list5").jqGrid('addRowData', i + 1, data[i]);
    //        //}
    //    }
    //})
    //};
    jQuery("#list5").jqGrid({
        url: "/Contract/AfterService.asmx/GetQRCodeList",
        datatype: "json",
        mtype: 'post',
        postData: { where: "" },
        ajaxGridOptions: { contentType: 'application/json; charset=utf-8' },
        serializeGridData: function (postData) {
            getRoleModuleRight();
            return JSON.stringify(postData);
        },
        jsonReader: {
            root: function (obj) {
                var data = eval("(" + obj.d + ")");
                return data.rows;
            },
            repeatitems: false
        },
        colNames: ['序号', '安卓二维码地址', '苹果二维码地址', '安卓下载地址', '苹果下载地址', '安卓是否上线', '苹果是否上线'],
        colModel: [
            { name: 'id', index: 'id', width: 55, align: "center", hidden: true },
            { name: 'androidqrcodepath', index: 'androidqrcodepath', width: 120, align: "center" },
            { name: 'iosqrcodepath', index: 'iosqrcodepath', width: 120, align: "center" },
            { name: 'androidurl', index: 'androidurl', width: 120, align: "center" },
            { name: 'iosurl', index: 'iosurl', width: 120, align: "center" },
            { name: 'androidisup', index: 'androidisup', width: 50, align: "center" },
            { name: 'iosisup', index: 'iosisup', width: 50, align: "center" }
        ],
        page: 1,
        rowNum: 30,
        rowList: [30, 50, 100],
        viewrecords: true,
        loadonce: true,
        pager: '#pager5',
        sortname: 'id',
        viewrecords: true,
        sortorder: "asc",
        //caption: "用户列表",
        editurl: "",
        shrinkToFit: true,
        autowidth: true,
        multiselect: true,
        refresh: true,
        //width:"100%",
        height: 650
    });



    //获取角色模块权限
    function getRoleModuleRight() {
        var role_id = $.cookie("r_id");
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: '/Contract/AfterService.asmx/GetAfterRoleModuleRight',
            data: '{"where":" where tb_RoleModuleRight.module_id=' + _m_id + ' and role_id=' + role_id + ' "}',
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                if (data != "0" || data != "") {
                    var s = eval("(" + data + ")");
                    var list = s.rows;
                    console.log(list);
                    var tempmenustr = "";
                    for (i = 0; i < list.length; i++) {
                        tempmenustr = tempmenustr + "<span id='" + list[i].right_code + "' style='display: block; position: relative; top: 20px;margin-right: 15px; font-size: 11px; cursor: pointer; float: left;'>" +
              "<i class='fa " + list[i].right_imagecode + "' style='color: #ff6a00; margin-right: 5px;'></i>" +
              "<em style='font-size: 11px; font-style: normal; color: #1c94c4;'>" + list[i].right_name + "</em></span>"
                    }
                    $("#topmenu").append(tempmenustr);
                }
            }
        });
    }

    //设置产品期数
    $(document.body).on("click", "#setgoodsissue", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0 || ids.length > 1) {
            AlertInfo("请选择一行数据进行编辑！");
            return false;
        }
        else {
            var temprowData = $("#list5").jqGrid("getRowData", ids[0]);
            AlertBackText(function (c) {
                if (parseInt(c) == 1) {
                    $.ajax({
                        type: "post",
                        contentType: "application/json",
                        url: "/Contract/AfterService.asmx/SetProductIssue",
                        data: '{"id":"' + temprowData.id + '","productprice":"' + temprowData.P_Price + '","number":"' + c + '","productname":"' + temprowData.P_Name + '","productsort":"' + temprowData.PS_ID + '","type":"' + temprowData.P_Type + '"}',
                        dataType: 'json',
                        success: function (result) {
                            var data = result.d;
                            if (parseInt(data) == 1) {
                                Alertsuccess("设置成功");
                            }
                            else if (parseInt(data) == 2) {
                                AlertInfo("该产品已有未开始期数！");
                            }
                            else {
                                AlertError("设置失败" + data);
                            }
                        }
                    });
                }
                else {
                    AlertInfo("最大产品期数设置数量为'1'！");
                    return;
                }
            });
        }
    });
    //新增
    $(document.body).on("click", "#add", function () {
        window.location.href = "QRCodeEdit.aspx?m_id=" + _m_id;
    });
    //刷新
    $(document.body).on("click", "#refresh", function () {
        //window.location.href = "Goods.aspx?m_id=" + _m_id;
        //$("#topmenu").html("");
        $("#topmenu").html("");
        $("#list5").jqGrid('setGridParam', {
            datatype: 'json',
            //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
            page: 1
        }).trigger("reloadGrid"); //重新载入
    });
    //编辑
    $(document.body).on("click", "#edit", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0 || ids.length > 1) {
            AlertInfo("请选择一行数据进行编辑！");
            return false;
        }
        else {
            var temprowData = $("#list5").jqGrid("getRowData", ids[0]);
            window.location.href = "QRCodeEdit.aspx?id=" + temprowData.id + "&m_id=" + _m_id;
        }
    });
    //删除  "data": {"_method":"DELETE"}
    $(document.body).on("click", "#delete", function () {
        //获取到选中的网格中的id
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0) {
            AlertInfo("请选择要删除的数据行！");
            return;
        }
        else {
            AlertConfirm("您确定要删除选中的数据吗？", function (c) {
                if (c == "1") {
                    var rows = ids[0];
                    //根据id获取数据行
                    var temprowData = $("#list5").jqGrid("getRowData", rows);
                    var id = temprowData.id;
                    //如果有已经开始或者已揭晓的产品期数则只改变产品的上架状态
                    if (getissuebyproductid(id)) {
                        $.ajax({
                            type: "post",
                            contentType: "application/json",
                            url: "/Contract/AfterService.asmx/updatestatebyid",
                            data: '{"productid":"' + id + '","state":"下架"}',
                            async: false,
                            dataType: 'json',
                            success: function (result) {
                                var data = result.d;
                                if (data == "1") {
                                    Alertsuccess("删除成功！");
                                    $("#topmenu").html("");
                                    $("#list5").jqGrid('setGridParam', {
                                        datatype: 'json',
                                        //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
                                        page: 1
                                    }).trigger("reloadGrid"); //重新载入
                                }
                                else {
                                    AlertError("删除失败");
                                }
                            }
                        });
                    }
                    else {
                        var p_details = temprowData.P_Details;
                        if (p_details == "") {
                            $.ajax({
                                url: "https://d.apicloud.com/mcm/api/tb_Product/" + id,
                                method: "post",
                                headers: {
                                    //"Content-type": "application/json",
                                    "X-APICloud-AppId": _appid,
                                    "X-APICloud-AppKey": _appkey
                                },
                                "data": {
                                    "_method": "DELETE"
                                },
                                //data:{"name":"zhangsan","age":"20"},
                                dataType: "json",
                                success: function (data, status, header) {
                                    if (status == "success") {
                                        Alertsuccess("删除成功！");
                                        $("#topmenu").html("");
                                        $("#list5").jqGrid('setGridParam', {
                                            datatype: 'json',
                                            //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
                                            page: 1
                                        }).trigger("reloadGrid"); //重新载入

                                        //根据产品id删除状态为未开始的产品期数
                                        $.ajax({
                                            type: "post",
                                            contentType: "application/json",
                                            url: "/Contract/AfterService.asmx/DeleteIssueByProduct",
                                            data: '{"productid":"' + id + ' "}',
                                            dataType: 'json',
                                            success: function (result) {
                                                var data = result.d;
                                                if (parseInt(data) == 1) {
                                                    //Alertsuccess("删除成功");
                                                }
                                                else {
                                                    //AlertError("删除失败" + data);
                                                }
                                            }
                                        });


                                        var jsonlist = imgkeylist;
                                        //判断加载时存储在imgkeylist中的产品图片信息
                                        if (jsonlist.length > 0) {
                                            //console.log(jsonlist);
                                            //循环获取产品图片信息
                                            for (i = 0; i < jsonlist.length; i++) {
                                                if (jsonlist[i].id == id) {
                                                    //取出当前产品的产品主图
                                                    var img = jsonlist[i].imgsrc;
                                                    //分割主图
                                                    var imgspllist = img.split(',');
                                                    //alert(imgspllist.length);
                                                    if (imgspllist.length > 0) {
                                                        //循环删除产品主图
                                                        for (j = 0; j < imgspllist.length; j++) {
                                                            var tempimg = imgspllist[j];
                                                            var spllist = tempimg.split('/');
                                                            var imgkey = spllist[spllist.length - 1];
                                                            //var tempkeyspl = imgkey.split('.');
                                                            //var tempimgstr = tempkeyspl[0];
                                                            $.ajax({
                                                                type: "post",
                                                                contentType: "application/json",
                                                                url: "/Contract/AfterCommonService.asmx/DeleteQiNiuImage",
                                                                data: '{"key":"' + imgkey + ' "}',
                                                                async: false,
                                                                dataType: 'json',
                                                                success: function (result) {
                                                                    var data = result.d;
                                                                    if (data == "200") {
                                                                    }
                                                                    else {
                                                                        //AlertError("删除失败");
                                                                    }
                                                                }
                                                            });
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                },
                                error: function (data, status, header) {

                                }
                            });
                        }
                        else {
                            AlertInfo("请先进行编辑并把产品详情清空！");
                            return;
                        }
                    }
                }
                else {
                    return false;
                }
            });
        }
    });
    //上架
    $(document.body).on("click", "#toup", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0) {
            AlertInfo("请选择要删除的数据行！");
            return;
        }
        else {
            var rows = ids[0];
            //根据id获取数据行
            var temprowData = $("#list5").jqGrid("getRowData", rows);
            var id = temprowData.id;
            var statestr = temprowData.p_state;
            if (statestr == "下架" || statestr == "" || statestr == null || statestr == undefined) {
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    url: "/Contract/AfterService.asmx/updatestatebyid",
                    data: '{"productid":"' + id + '","state":"上架"}',
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        var data = result.d;
                        if (data == "1") {
                            Alertsuccess("上架成功！");
                            $("#topmenu").html("");
                            $("#list5").jqGrid('setGridParam', {
                                datatype: 'json',
                                //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
                                page: 1
                            }).trigger("reloadGrid"); //重新载入
                        }
                        else {
                            AlertError("上架失败");
                        }
                    }
                });
            }
            else {
                AlertInfo("产品已上架！");
                return;
            }
        }
    });
    //下架
    $(document.body).on("click", "#todown", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0) {
            AlertInfo("请选择要删除的数据行！");
            return;
        }
        else {
            var rows = ids[0];
            //根据id获取数据行
            var temprowData = $("#list5").jqGrid("getRowData", rows);
            var id = temprowData.id;
            var statestr = temprowData.p_state;
            if (statestr == "上架" || statestr == "" || statestr == null || statestr == undefined) {
                $.ajax({
                    type: "post",
                    contentType: "application/json",
                    url: "/Contract/AfterService.asmx/updatestatebyid",
                    data: '{"productid":"' + id + '","state":"下架"}',
                    async: false,
                    dataType: 'json',
                    success: function (result) {
                        var data = result.d;
                        if (data == "1") {
                            Alertsuccess("下架成功！");
                            $("#topmenu").html("");
                            $("#list5").jqGrid('setGridParam', {
                                datatype: 'json',
                                //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
                                page: 1
                            }).trigger("reloadGrid"); //重新载入
                        }
                        else {
                            AlertError("下架失败");
                        }
                    }
                });
            }
            else {
                AlertInfo("产品已下架！");
                return;
            }
        }
    });
    //复制
    $(document.body).on("click", "#copy", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0 || ids.length > 1) {
            AlertInfo("请选择一行数据进行复制！");
            return false;
        }
        else {
            var temprowData = $("#list5").jqGrid("getRowData", ids[0]);
            window.location.href = "GoodsEdit.aspx?id=" + temprowData.id + "&m_id=" + _m_id + "&copy=1";
        }
    });
    //删除前先根据该产品id获取是否有已开始或者已揭晓的期数
    function getissuebyproductid(pid) {
        var ishavedata = false;//是否有数据
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/Contract/AfterService.asmx/getissuebyproductid",
            data: '{"productid":"' + pid + ' "}',
            async: false,
            dataType: 'json',
            success: function (result) {
                var data = result.d;
                if (data == "1") {
                    ishavedata = true;
                }
                else {
                    ishavedata = false;
                    //AlertError("删除失败");
                }
            }
        });
        return ishavedata;
    }
});


