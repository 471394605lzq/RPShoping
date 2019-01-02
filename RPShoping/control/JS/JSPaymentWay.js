jQuery(document).ready(function () {
    var _m_id = $.query.get("m_id") || "";

    var _appid = getappid();
    var _appkey = getshaappkey();
    load();

    function load() {
        getproductsort(function (c, data) {
            if (c == "1") {
                //var tempdata = '{"rows"' + ':' + JSON.stringify(data)+"}";
                var tempdata = JSON.stringify(data);
                tempdata = tempdata.substring(1, tempdata.length - 1);
                var t = JSON.parse("[" + tempdata + "]");
                //var resultdata = JSON.parse(tempdata);
                //var t2 = "[" + tempdata + "]";
                //console.log(JSON.parse(t2));
                //console.log(t);
                //console.log(resultdata);
                $("#list5").jqGrid({
                    datatype: "local",
                    data: t,
                    colNames: ['序号', '支付名称', '状态','是否默认'],
                    colModel: [
                        { name: 'id', index: 'id', width: 55, align: "center", hidden: true },
                        { name: 'PA_Name', index: 'PA_Name', width: 120, align: "center" },
                        { name: 'state', index: 'state', width: 120, align: "center" },
                        { name: 'isdefault', index: 'isdefault', width: 120, align: "center" }
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
                getRoleModuleRight();
                //for (var i = 0; i < data.length; i++) {
                //    $("#list5").jqGrid('addRowData', i + 1, data[i]);
                //}
            }
        })
    };


    //获取商品分类信息
    function getproductsort(callback) {
        var filter = {
            limit: 500000
        }
        $.ajax({
            url: "https://d.apicloud.com/mcm/api/tb_PaymentWay?filter=" + encodeURIComponent(JSON.stringify(filter)),
            method: "get",
            headers: {
                "Content-type": "application/json",
                "X-APICloud-AppId": _appid,
                "X-APICloud-AppKey": _appkey
            },
            //data:{"name":"zhangsan","age":"20"},
            dataType: "json",
            success: function (data, status, header) {
                callback("1", data);
            },
            error: function (data, status, header) {

            }
        });
    }



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

    //新增
    $(document.body).on("click", "#add", function () {
        window.location.href = "PaymentWayEdit.aspx?m_id=" + _m_id;
    });
    //刷新
    $(document.body).on("click", "#refresh", function () {
        window.location.href = "PaymentWay.aspx?m_id=" + _m_id;
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
            window.location.href = "PaymentWayEdit.aspx?id=" + temprowData.id + "&m_id=" + _m_id;
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
                    $.ajax({
                        url: "https://d.apicloud.com/mcm/api/tb_PaymentWay/" + id,
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
                                Alertsuccesscallback("删除成功！", function (c) {
                                    if (c == "1") {
                                        window.location.href = "PaymentWay.aspx?m_id=" + _m_id;
                                    }
                                });

                            }
                        },
                        error: function (data, status, header) {

                        }
                    });
                }
                else {
                    return false;
                }
            });
        }
    });

});


