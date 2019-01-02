jQuery(document).ready(function () {
    var _m_id = $.query.get("m_id") || "";
    var _appid = getappid();
    var _appkey = getshaappkey();
    var imgkeylist = [];//商品图片集合
    jQuery("#list5").jqGrid({
        url: "/Contract/AfterService.asmx/GetUserList",
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
        colNames: ['序号', '昵称', '登陆名', '账户余额', '备注'],
        colModel: [
            { name: 'id', index: 'id', width: 55, align: "center", hidden: true },
            { name: 'nickname', index: 'nickname', width: 80, align: "center" },
            { name: 'username', index: 'username', width: 100, align: "center" },
            { name: 'balance', index: 'balance', width: 100, align: "center" },
            { name: 'issystem', index: 'issystem', width: 80, align: "center", hidden: true }
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

    //新增
    $(document.body).on("click", "#add", function () {
        window.location.href = "systemuseredit.aspx?m_id=" + _m_id;
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
            window.location.href = "systemuseredit.aspx?id=" + temprowData.id + "&m_id=" + _m_id;
        }
    });
    $(document.body).on("click", "#copy", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0 || ids.length > 1) {
            AlertInfo("请选择一行数据进行复制！");
            return false;
        }
        else {
            var temprowData = $("#list5").jqGrid("getRowData", ids[0]);
            window.location.href = "systemuseredit.aspx?id=" + temprowData.id + "&m_id=" + _m_id + "&copy=1";
        }
    });
});


