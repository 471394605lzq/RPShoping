jQuery(document).ready(function () {
    var _m_id = $.query.get("m_id") || "";
    jQuery("#list5").jqGrid({
        url: "/Contract/AfterService.asmx/GetAfterUserRoleList",
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
            //page: function (obj) {
            //    var data = eval("(" + obj.d + ")");
            //    return data.page;
            //},
            //total: function (obj) {
            //    var data = eval("(" + obj.d + ")");
            //    return data.total;
            //},
            //records: function (obj) {
            //    var data = eval("(" + obj.d + ")");
            //    return data.records;
            //},
            repeatitems: false
        },
        colNames: ['序号', '角色名称', '备注', '角色key'],
        colModel: [
            { name: 'R_ID', index: 'R_ID', width: 55, align: "center", hidden: true },
            { name: 'R_Name', index: 'R_Name', width: 80, align: "center" },
            { name: 'R_Remark', index: 'R_Remark', width: 155, align: "center" },
            { name: 'R_Key', index: 'R_Key', width: 120, align: "center" }
        ],
        page: 1,
        rowNum: 30,
        rowList: [30, 50, 100],
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
        height: 650,
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
                    var tempmenustr = "";
                    for (i = 0; i < list.length; i++) {
                        tempmenustr = tempmenustr + "<span id='" + list[i].right_code + "' style='display: block; position: relative; top: 20px; font-size: 11px; margin-right:15px; cursor: pointer; float: left;'>" +
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
        window.location.href = "RoleEdit.aspx?m_id=" + _m_id;
    });
    //刷新
    $(document.body).on("click", "#refresh", function () {
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
            if (temprowData.R_Key == "admin") {
                AlertInfo("管理员角色不能修改！");
                return;
            }
            window.location.href = "RoleEdit.aspx?id=" + temprowData.R_ID + "&m_id=" + _m_id;
        }
    });
    //删除
    $(document.body).on("click", "#delete", function () {
        //获取到选中的网格中的id
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
        if (ids.length <= 0) {
            AlertInfo("请选择要删除的数据行！");
            return;
        }
        else {
            var idvalue = "";
            for (j = 0; j < ids.length; j++) {
                var tempid = ids[j];
                //根据id获取数据行
                var temprowData = $("#list5").jqGrid("getRowData", tempid);
                if (temprowData.R_Key == "admin") {
                    AlertInfo("管理员角色不能删除！");
                    return;
                }
                //取得au_id
                idvalue = idvalue + temprowData.R_ID + ",";
            }
            AlertConfirm("您确定要删除选中的数据吗？", function (c) {
                if (c == "1") {
                    var id = "R_ID";
                    var tablename = "tb_Role";
                    idvalue = idvalue.substring(0, idvalue.length - 1);
                    CommonDelete(tablename, id, idvalue, function (c) {
                        if (c == "1") {
                            Alertsuccess("删除成功！");
                            $("#topmenu").html("");
                            $("#list5").jqGrid('setGridParam', {
                                datatype: 'json',
                                //postData: { 'keyword': encodeURI(encodeURI(keyword)) }, //发送数据  
                                page: 1
                            }).trigger("reloadGrid"); //重新载入  
                        }
                        else {
                            AlertError(c);
                            return false;
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