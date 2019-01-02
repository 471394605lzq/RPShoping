jQuery(document).ready(function () {
    var _m_id = $.query.get("m_id") || "";
    jQuery("#list5").jqGrid({
        url: "/Contract/AfterService.asmx/GetAfterModuleList",
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
        colNames: ['序号', '模块名称', '模块key', '模块编号', '备注', '显示状态值', '是否显示', '页面名称', '父级id', '是否为父级值', '是否为父级','序号'],
        colModel: [
            { name: 'M_ID', index: 'M_ID', width: 55, align: "center", hidden: true },
            { name: 'M_Name', index: 'M_Name', width: 80, align: "center" },
            { name: 'M_ModuleKey', index: 'M_ModuleKey', width: 100, align: "center" },
            { name: 'M_Level_No', index: 'M_Level_No', width: 120, align: "center" },
            { name: 'M_Remark', index: 'M_Remark', width: 150, align: "center" },
            { name: 'M_ShowState', index: 'M_ShowState', width: 120, align: "center", hidden: true },
            { name: 'showstatename', index: 'showstatename', width: 80, align: "center" },
            { name: 'M_EditUrl', index: 'M_EditUrl', width: 80, align: "center" },
            { name: 'parement_id', index: 'parement_id', width: 80, align: "center", hidden: true },
            { name: 'isparement', index: 'isparement', width: 80, align: "center", hidden: true },
            { name: 'isparementname', index: 'isparementname', width: 80, align: "center" },
            { name: 'reorder', index: 'reorder', width: 80, align: "center" }
            
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
        window.location.href = "AfterModuleEdit.aspx?m_id=" + _m_id;
    });
    //新增子菜单
    $(document.body).on("click", "#addchildmenu", function () {
        var ids = $("#list5").jqGrid('getGridParam', 'selarrrow');
                    if (ids.length <= 0 || ids.length > 1) {
                        AlertInfo("请选择一行数据进行编辑！");
                        return false;
                    }
                    else {
                        var temprowData = $("#list5").jqGrid("getRowData", ids[0]);
                        var isparent = temprowData.isparement;
                        if (isparent == "1") {
                            window.location.href = "AfterModuleEdit.aspx?parentid=" + temprowData.M_ID + "&m_id=" + _m_id;
                        }
                        else {
                            AlertInfo("子菜单不可添加下级菜单！");
                            return false;
                        }
                    }
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
            window.location.href = "AfterModuleEdit.aspx?id=" + temprowData.M_ID + "&m_id=" + _m_id;
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
            AlertConfirm("您确定要删除选中的数据吗？", function (c) {
                if (c == "1") {
                    var id = "M_ID";
                    var tablename = "tb_Module";
                    var idvalue = "";
                    for (j = 0; j < ids.length; j++) {
                        var tempid = ids[j];
                        //根据id获取数据行
                        var temprowData = $("#list5").jqGrid("getRowData", tempid);
                        //取得au_id
                        idvalue = idvalue + temprowData.M_ID + ",";
                    }
                    idvalue = idvalue.substring(0, idvalue.length - 1);
                    CommonDelete(tablename, id, idvalue, function (c) {
                        if (c == "1") {
                            Alertsuccess("删除成功！");
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