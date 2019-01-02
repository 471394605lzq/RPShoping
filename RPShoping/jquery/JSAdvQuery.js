/// <reference name="jquery.js"/>
/// <reference name="jquery.form.js"/>
/// <reference name="jquery.validate.min.js"/>

//枚举常量
var advEnum = {
    //高级查询时，当选中操作类型后，控件将要呈现的方式：
    displayValue: 156,   //156:按值呈现
    displayString: 157,   //157:按字符串呈现
    displayDict: 158,   //158:按字典方式呈现
    displayDate: 159,   //159:按日期方式呈现

    //显示高级查询结果时，列的排序类型
    number: 112,
    string: 113,
    date: 114,
    dict: 115,

    //货币类型
    US: 1,
    RMB: 2, 

    //显示网格式，数据库中定义的格式
    FormatDate: "shortdate",
    FormatCurrency: "currency",
    FormatNumber: "number",
    FormatInt: "int",

    //活动类型
    careCustomer: 1, //客户关怀
    dayToDayWork: 2, //日常工作
    followup: 3, //跟进
    complain: 4, //投诉

    //对列筛选时的操作
    contains: 122, //包含数据
    uncontains: 123, //不包含数据

    //活动的状态
    unstart: 76,
    runing: 77,
    canceled: 78,
    completed:79,

    //表ID
    rivalDetail: 84,
    ChanceRivalTab:85,
    actionTab: 10,
    chanceDetailTab: 13,
    quotationTab: 14,
    orderTab: 16,
    linkTab: 4,
    customerTab: 1,
    chanceTab: 9,
    currencyTab: 18,
    dictDetailTab: 2,
    gatheringTab:21,
    gatheringsTab: 22,
    orderplanTab:19,
    orderplansTab: 20,
    ordersTab: 17,
    productTab: 11,
    quotationsTab: 15,
    quotationTmpTab: 44,
    memberTab: 8,
    departmentTab: 23,
    userReportTab: 53,
    costDetailTab: 56,
    importImplateTab: 58,
    lockTab: 59,
    followPolicyDetailTab: 61,
    areaTab: 5,
    knowledgeTab: 73,
    targetTab: 74,
    planTab:76,
    auditModuleStepTab: 77,
    auditTab: 64,
    auditDetailTab: 65,
    planCustomerRelation: 80,
    complainTab: 40,
    complainCallTab: 41,
    subUserTab: 8,
    putonrecord: 100,
    payInAndOutDetailTab: 105,
    productsTab:118,

    //模块ID
    marketchance: 22,
    customer_linkman: 13,
    customer_manager: 12,
    project_manager: 230,
    projectPhase_manager: 231,
    invAll_manager: 236,
    returnpoint: 239,
    waite_event: 131,
    action_manager: 45,
    orderplan_detail: 32,
    contractkey: 84,
    AcceptanceKey: 88,

    //权限ID
    addRight: 1,
    editRight: 2,
    delRight: 3,
    shareRight: 10,
    lockRight: 12,
    unlockRight: 13,

    //列ID
    dictdetailColId: 7,
    areaColId: 37,

    customerModuleMenu:31,


    //字典项
    dictImportantCustomer:921,//重要客户
    dictSuccCustomer:929,//成交客户
    dictHascaseCustomer:930
};


//查询对象
var advFind = {
    //methodName:服务器端方法名
    //args:用服务器端参数名与客户端对应的值串连接字符串：
    //     GET方法格式：tabID=1&keyword="公司"
    //     POST方法格式："{"customer":{"id":0,"name":"jk","age":18},"link":{"name":"zs","id":"1"}}"，其中customer,link为参数名。
    //callback:调用完成后执行的方法
    //isAsync:是否采用异步调用，默认为异步调用
    //submitType:提交数据的类型：POST,GET。默认为GET
    //progressBar:显示进度条的div的ID，需带"#"号。如"#divId"
    //url:请求的URL，主要用于路径不符合默认的URL配置与请求不同的URL时，url与methodName只需指定一个即可
    //noEncode:是否不需要对参数编码
    GetServerMethodValue: function (methodName, args, callback, isAsync, submitType, progressBar, url, noEncode, hideErrorWindow, dtType) {
        var sdargs = "";
        var subType = "GET";
        var dateType = "json";
        var useAsync = true;
        var submitData = args;
        
        if (isAsync) { useAsync = isAsync; }
        if (submitType) { subType = submitType; }
        if (subType == "GET") {
            if (!noEncode) {
                //编码
                //在值中包含"&"时会出错,这时最好在外部编码
                //此处必须用encodeURIComponent，不用能escape,否则不能对'+'编码，导致出错.
                if (submitData.length > 0) {
                    var sds = submitData.split("&");
                    $.each(sds, function (i, iv) {
                        var didx = iv.indexOf("=");
                        if (didx < 0) {
                            sdargs += encodeURIComponent("&" + iv);
                        }
                        else {
                            var key = iv.substr(0, didx);
                            var val = "";
                            if (didx < (iv.length - 1))
                                val = encodeURIComponent(iv.substr(didx + 1, iv.length - didx - 1));
                            sdargs += key + "=" + val + "&";
                        }
                    });
                }
                if (sdargs.length > 0) {
                    sdargs = sdargs.substr(0, sdargs.length - 1);
                }
                submitData = sdargs;
            }
        }
        if ($(progressBar).length == 0) {
            progressBar = $(window.parent.document).find(progressBar);
        }
        else {
            progressBar = $(progressBar);
        }
        progressBar.addClass("crm_progressbar");
        if (!url) { url = "../Crm_Server.svc/" + methodName; }
        if (dtType) { dateType = dtType; }
        if (typeof submitData == "object") { submitData = $.toJSON(submitData); }
        $.ajax({
            type: subType,
            contentType: "application/json;charset=UTF-8",
            url: url,
            data: submitData,
            dataType: dateType,
            processData: false,
            cache: true,
            async: useAsync, //是否采用异步加载
            success: function (msg, rs) {
                //停止滚动条
                progressBar.removeClass("crm_progressbar");
                //回调
                callback(msg, rs);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //停止滚动条
                progressBar.removeClass("crm_progressbar");

                //输出错误提示
                var errInfo = "call ajax method fail!";
                errInfo += "detail:" + XMLHttpRequest.responseText;
                var errurl = url;
                if (methodName) errurl += "/" + methodName;
                if (args) errurl += "/" + args;
                if (!hideErrorWindow) {
                    //$.showErrorWindow(errInfo, errurl);

                    if ($.log != undefined) {
                      
                    }
                }
                else {
                    callback("{opResult:'RequestFailed}'", false, XMLHttpRequest);
                }
            }
        });
    }
}
/*
jQuery.fn.extend({
    editVal: function() {
        if (this.data("relVal") && this.data("relVal").length > 0) {
            var dataVal = $.evalJSON(this.data("relVal"));
            if ($.isArray(dataVal) && dataVal.length > 0) {
                var rs = "";
                $.each(dataVal, function(idx, item) {
                    rs += item.priKey + ",";
                });
                if (rs.length > 0) {
                    rs = rs.substr(0, rs.length - 1);
                }
                return rs;
            }
            else {
                return dataVal.toString();
            }
        }
        else {
            return this.val();
        }
    } 
});*/ 
jQuery.extend({
    evalJSON: function (strJson) {
        ///<summary>将json字符串转换为对象</summary>
        ///内容中不能包含\t\r\n"字符

        try {
            var str = "(" + strJson + ")";
            return eval(str);
        } catch (e) {
            return null;
        }
    },
    toJSON: function (object) {//返回json字符串
        if (object == null || object == undefined) return "null";
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor)
                type = 'array';
            else if (RegExp == object.constructor)
                type = 'regexp';
            else
                type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g,
       function () {
           var a = arguments[0];
           return (a == '\n') ? '\\n' :
                       (a == '\r') ? '\\r' :
                       (a == '\t') ? '\\t' : ""
       }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSON(object[property]);
                    if (value !== undefined)
                        results.push(jQuery.toJSON(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSON(object[i]);
                    if (value !== undefined) results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    },
    shieldMouseRightKey: function () {//屏蔽右键
        var event = null;

        if (window.Event) {
            document.captureEvents(Event.MOUSEUP);
        }
        function nocontextmenu(e) {
            e.stopPropagation();
            e.preventDefault();
        }
        function norightclick(e) {
            if (window.Event) {
                if (e.which == 2 || e.which == 3) {
                    e.stopPropagation();
                    e.preventDefault();
                }
            }
            else {
                if (event.button == 2 || event.button == 3) {
                    e.stopPropagation();
                    e.preventDefault();
                }
            }
        }
        document.oncontextmenu = nocontextmenu; // for IE5+ 
        document.onmousedown = norightclick; // for all others
    }
});

/*
* 高级查询控件v1.0
*
* Copyright 2010, afaneti
*
* Date: 2010-08-14
*/
var advMenu = null;
var changeState = null;
var tabMaxId = 0;
jQuery.advQuery = function() { }
if (jQuery) (
    function (jQuery) {
        jQuery.extend(jQuery.advQuery, {
            init: function (option) {//载入时初始化
                settings = jQuery.extend({
                    divId: 'divId', //条件DIV
                    id: 0, //表ID
                    onChange: function () { },
                    onTabChange: function () { },
                    formId: "form", //form的ID，一定要带上"#"，如："#mainFrm"
                    view: {//视图
                        name: "",
                        id: 0,
                        attribute: "",
                        queryStatement: "",
                        mainTabId: 0
                    },
                    conditions: [{//条件
                        id: 0,
                        viewId: 0,
                        alias: "",
                        colId: 0,
                        operator: 0,
                        value: ""
                    }],
                    relations: [{//条件的关系
                        id: 0,
                        viewId: 0,
                        condition: "",
                        relation: 0
                    }],
                    tables: [{//表关系
                        id: 0,
                        viewId: 0,
                        tabId: 0,
                        tabAli: "",
                        colId: 0,
                        tabId1: 0,
                        tabAli1: "",
                        colId1: 0
                    }],
                    viewColumns: [{//将要显示的列
                        id: 0,
                        viewId: 0,
                        alias: "",
                        colId: 0,
                        format: "",
                        sequence: 0,
                        sort: -1,
                        width: -1, //空值;将使用默认宽度
                        visible: 1,
                        displayName: "",
                        tabDisplayName: "",
                        tabId: 0,
                        isFix: 0
                    }]
                }, option);
                jQuery.advQuery.setting = settings;
                jQuery.advQuery.setInitContent();

                //设置验证
                $(settings.formId).validate({
                    errorPlacement: function (error, element) {
                        var spid = element.attr("id").replace("val", "span");
                        if (element.parent("span:first").length > 0 && spid == element.parent("span:first").attr("id")) {
                            var tp = element.parent().parent();
                            error.appendTo(tp);
                            var td = element.parent().parent();
                        }
                        else {
                            error.appendTo(element.parent());
                        }
                    }
                });
            },
            loadViewInfo: function (viewId) {
                if (!$.advQuery.clearCondition()) { return; }
                if (!$.isNumber(viewId) || parseInt(viewId) <= 0) { return; }
                advFind.GetServerMethodValue("QueryViewInfo", "viewID=" + viewId, function (msg, rs) {
                    if (rs) {
                        var data = msg.d;
                        if (data) {
                            jQuery.advQuery.conditions = new Array();
                            jQuery.advQuery.setting.viewColumns = new Array();
                            jQuery.advQuery.relations = new Array();
                            jQuery.advQuery.tables = new Array();

                            //加载视图信息
                            jQuery.advQuery.setting.view.id = data.view.id;
                            jQuery.advQuery.setting.view.name = data.view.name;
                            jQuery.advQuery.setting.view.attribute = data.view.attribute;
                            jQuery.advQuery.setting.view.isysview = data.view.isysview;

                            $.each(data.condition, function (idx, item) {
                                //填充条件
                                jQuery.advQuery.conditions.push({
                                    id: item.id,
                                    viewId: item.viewId,
                                    alias: item.alias,
                                    colId: item.colId,
                                    operator: item.operator,
                                    value: item.value
                                });
                            });
                            $.each(data.viewColumn, function (idx, item) {
                                //填充查询列
                                jQuery.advQuery.setting.viewColumns.push({
                                    id: item.id,
                                    viewId: item.viewId,
                                    alias: item.alias,
                                    colId: item.colId,
                                    format: item.format,
                                    sequence: item.sequence,
                                    sort: item.sort,
                                    width: item.width, //空值;将使用默认宽度
                                    visible: item.visible,
                                    displayName: item.column.colDisplayName,
                                    tabDisplayName: item.column.table.tabDisplayName,
                                    tabId: item.column.table.tabId,
                                    isFix: item.isFix
                                });
                            });
                            $.each(data.relation, function (idx, item) {
                                //填充列关系
                                jQuery.advQuery.relations.push({
                                    id: item.id,
                                    viewId: item.viewId,
                                    condition: item.condition,
                                    relation: item.relation
                                });
                            });
                            $.each(data.tabRelation, function (idx, item) {
                                //填充表关系
                                jQuery.advQuery.tables.push({
                                    id: item.id,
                                    viewId: item.viewId,
                                    tabId: item.tabId,
                                    tabAli: item.tabAli,
                                    colId: item.colId,
                                    tabId1: item.tabId1,
                                    tabAli1: item.tabAli1,
                                    colId1: item.colId1
                                });
                            });

                            //检查表关系是否存在，并赋值给alias
                            if (data.tabRelation.length > 0) {
                                $("#" + jQuery.advQuery.setting.divId).attr("alias", data.tabRelation[0].tabAli);
                            }
                            var getTableParentContainer = function (alias) {
                                var targetAlias = "";
                                for (var i = 0; i < data.tabRelation.length; i++) {
                                    if (data.tabRelation[i].tabAli1 == alias) {
                                        targetAlias = data.tabRelation[i].tabAli;
                                    }
                                }
                                return targetAlias;
                            };

                            //加载条件(使用递归依次加载条件)
                            var loadedcdi = 0;
                            var loadCdi = function (idx, callback) {
                                var item = null;
                                if (idx >= data.condition.length) {
                                    if (callback) callback();
                                    return;
                                }
                                item = data.condition[idx];
                                var select = jQuery.advQuery.getColSelect(jQuery.advQuery.setting.divId);
                                var children = select.children();
                                var opts = children.eq(1).children();
                                changeState = "loading";
                                var loadCondition = function () {
                                    if (opts.length > 0) {
                                        changeState = "loading";
                                        opts.parents("table:first").attr("colcdi", item.id).attr("type", "item");
                                        opts.removeAttr("selected");
                                        var opt = opts.filter("[value='" + item.colId + "'][type='col']");
                                        opt.attr("selected", "true");
                                        select.change();
                                        $.handlePage.waitfor(function () { return changeState != "loading"; }, 1000, 10000, function (istimeout) {
                                            opt.parents("tr:first").find("[role=ope]").val(item.operator);
                                            opt.parents("tr:first").find("[role=ope]").change();
                                            item.value = item.value || "";
                                            if (opt.parents("tr:first").find("[role=ope] option:selected").attr("showType") != advEnum.displayDict.toString()) {
                                                opt.parents("tr:first").find("[role=val]").val(item.value);
                                                loadedcdi++;
                                                loadCdi(idx + 1, callback);
                                            }
                                            else {
                                                if ($.isArray($.evalJSON(item.column.conditionPlus))) {
                                                    var cdiPlus = $.evalJSON(item.column.conditionPlus);
                                                    var text = "";
                                                    $.each(cdiPlus, function (i, iv) {
                                                        if (iv.value.toString() == item.value.toString()) {
                                                            text = iv.key;
                                                        }
                                                    });
                                                    opt.parents("tr:first").find("[role=val]").val(text);
                                                    opt.parents("tr:first").find("[role=val]").data("relVal", item.value);
                                                    loadedcdi++;
                                                    loadCdi(idx + 1, callback);
                                                }
                                                else {
                                                    $.handlePage.getModule(item.column.relationColTable.tabId, function (d, rs) {
                                                        if (rs) {
                                                            var src_data = d;
                                                            var pk_name = "rec_no";
                                                            if (item.column.relationColTable.tabId == advEnum.areaTab) {
                                                                pk_name = "areaid";
                                                            }
                                                            var context = "qcondition:" + src_data.moduleTable + "." + pk_name + " in (" + item.value + ")";
                                                            $.handlePage.operatorData(context, "query", src_data.moduleKey, function (d, rs) {
                                                                src_data = $.evalJSON(d);
                                                                var cval = [];
                                                                $.each(src_data, function (i, iv) {
                                                                    if (iv.isMainCol) {
                                                                        cval = iv.value;
                                                                    }
                                                                });
                                                                opt.parents("tr:first").find("[role=val]").val(cval.join(","));
                                                                opt.parents("tr:first").find("[role=val]").data("relVal", item.value);
                                                                loadedcdi++;
                                                                loadCdi(idx + 1, callback);
                                                            }, "", "", true);
                                                        }
                                                    });
                                                }
                                            }
                                        });
                                    }
                                };

                                if (item.alias && $("#" + jQuery.advQuery.setting.divId).attr("alias") != item.alias) {
                                    var targetAlias = getTableParentContainer(item.alias);
                                    var parentContainer = $("[alias='" + targetAlias + "']");
                                    var hastab = parentContainer.find("[type=tpitem][tab=" + item.column.table.tabId + "]");
                                    if (hastab.length == 0) {
                                        var select = jQuery.advQuery.getColSelect(parentContainer.attr("id")).first();
                                        select.find("option[value=" + item.column.table.tabId + "][type=tab]").attr("selected", "true");
                                        select.change();
                                        $.handlePage.waitfor(function () { return changeState != "loading"; }, 1000, 10000, function (istimeout) {
                                            var parentTab = select.parents("table:first").next("table");
                                            parentTab.attr("alias", item.alias);
                                            var nextTab = parentTab.attr("id");
                                            select = jQuery.advQuery.getColSelect(nextTab);
                                            children = select.children();
                                            opts = children.eq(1).children();
                                            loadCondition();
                                        });
                                    }
                                    else {
                                        select = jQuery.advQuery.getColSelect(hastab.attr("id"));
                                        children = select.children();
                                        opts = children.eq(1).children();
                                        loadCondition();
                                    }
                                }
                                else {
                                    loadCondition();
                                }
                            };

                            //加载条件
                            loadCdi(0, function () {
                                loadedcdi = data.condition.length;
                            });

                            //检测条件加载是否完成……
                            var runcount = 0;
                            var checkCdiLoadState = function () {
                                if (loadedcdi != data.condition.length && runcount < 10) {
                                    setTimeout(checkCdiLoadState, 1000);
                                    runcount++;
                                }
                                else {
                                    loadRelation();
                                }
                            };


                            //加载列关系
                            var loadRelation = function () {
                                $.each(jQuery.advQuery.relations, function (i, iv) {
                                    if (iv) {
                                        var colcdis = iv.condition.split(",");
                                        var colcdi = iv.id;
                                        var colcdirel = iv.relation;
                                        var seltab = null;
                                        $.each(colcdis, function (ii, iiv) {
                                            var cdiitem = iiv.replace("][", ",").replace("[", "").replace("]", "").split(",");
                                            var colcdiid = cdiitem[0];
                                            var colcditype = cdiitem[1];
                                            if (colcditype == 0) {
                                                seltab = $("#" + jQuery.advQuery.setting.divId).find("table[type=item][colcdi=" + colcdiid + "]:first");
                                            }
                                            else {
                                                seltab = $("#" + jQuery.advQuery.setting.divId).find("table[type=pitem][colcdi=" + colcdiid + "]:first");
                                            }
                                            seltab.css({ 'background-color': '#A7CDF0' });
                                            seltab.attr("selected", 'true');
                                        });
                                        $.advQuery.operateCondition(colcdirel);
                                        seltab.parents("table:first").attr("colcdi", colcdi).attr("type", "pitem");
                                    }
                                });
                            };

                            //开始执行加载关系
                            jQuery.advQuery.sortRelation();
                            checkCdiLoadState();
                        }
                    }
                });
            },
            sortRelation: function () {
                var getmaxval = function (item) {
                    var max = 0;
                    var childitems = item.split(",");
                    $.each(childitems, function (i, iv) {
                        var childMax = 0;
                        var iitem = iv.replace("][", ",").replace("[", "").replace("]", "").split(",");
                        if (iitem[1] == "0") childMax = 0;
                        else childMax = parseInt(iitem[0]);
                        if (max < childMax) max = childMax;
                    });
                    return max;
                };
                for (var i = 0; i < jQuery.advQuery.relations.length; i++) {
                    if ((i + 1) < jQuery.advQuery.relations.length && getmaxval(jQuery.advQuery.relations[i].condition) > getmaxval(jQuery.advQuery.relations[i + 1].condition)) {
                        var temp = jQuery.advQuery.relations[i];
                        jQuery.advQuery.relations[i] = jQuery.advQuery.relations[i + 1];
                        jQuery.advQuery.relations[i + 1] = temp;
                    }
                }
            },
            getColSelect: function (ctlid) {
                return $("#" + ctlid + " select").filter(function (idx) {
                    var rs = false;
                    $.each($(this).children(), function (idx, item) {
                        if (item.innerHTML == "选择") {
                            rs = true;
                        }
                    });
                    return rs;
                });
            },
            setInitContent: function () {//初始化内容
                //开始初始化
                $("#" + settings.divId).addClass('adv_ui');
                $("#" + settings.divId).attr("alias", "ali" + jQuery.advQuery.getNowTimeString());
                //初始布局
                advFind.GetServerMethodValue(
                    'GetAdvQueryColumns',
                    "tabID=" + settings.id,
                    function (msg, e) {
                        var dataObj = msg;
                        if (dataObj.d != undefined && dataObj.d.length > 0) {
                            jQuery.advQuery.colList = null;
                            jQuery.advQuery.colList = new Array();
                            var columns = dataObj.d;
                            if (columns.length == 0 || columns[0].columns.length == 0) {
                                throw ("初始化表时未找到指定列！");
                            }
                            jQuery.advQuery.colList.push(columns[0]);
                            jQuery.advQuery.cdi_Id = 0;
                            jQuery.advQuery.buildColumnList(jQuery.advQuery.colList[0], settings.divId, jQuery.advQuery.cdi_Id);
                        }
                        //载入完成
                        if (jQuery.advQuery.loadCompleted == null || jQuery.advQuery.loadCompleted == undefined) {
                            jQuery.advQuery.loadCompleted = true;
                        }
                    });
            },
            adjustViewOrder: function () {
                var tabId = $("#tabList").val();
                var url = "/crm/controls/viewSortDlg.aspx?tb=" + tabId;
                $.advQuery.showDialog(url, null, 450, 430);
            },
            refreshLayout: function (option) {//刷新布局
                jQuery.extend(jQuery.advQuery.setting, option, {
                    view: {//视图
                        name: "",
                        id: 0,
                        attribute: "",
                        queryStatement: "",
                        mainTabId: 0
                    },
                    conditions: [{//条件
                        id: 0,
                        viewId: 0,
                        alias: "",
                        colId: 0,
                        operator: 0,
                        value: ""
                    }],
                    relations: [{//条件的关系
                        id: 0,
                        viewId: 0,
                        condition: "",
                        relation: 0
                    }],
                    tables: [{//表关系
                        id: 0,
                        viewId: 0,
                        tabId: 0,
                        tabAli: "",
                        colId: 0,
                        tabId1: 0,
                        tabAli1: "",
                        colId1: 0
                    }],
                    viewColumns: [{//将要显示的列
                        id: 0,
                        viewId: 0,
                        alias: "",
                        colId: 0,
                        format: "",
                        sequence: 0,
                        sort: -1,
                        width: -1, //空值;将使用默认宽度
                        visible: 1,
                        displayName: "",
                        tabDisplayName: "",
                        tabId: 0,
                        isFix: 0
                    }]
                });
                $("#" + jQuery.advQuery.setting.divId).children().remove();
                jQuery.advQuery.setInitContent();
            },
            getViewColumns: function () {//增加列时，获取当前视图的列属性
                if (jQuery.advQuery.setting.viewColumns == null || (jQuery.advQuery.setting.viewColumns.length == 1 && jQuery.advQuery.setting.viewColumns[0].colId == 0)) {//未设置视图列
                    var cols = $.grep(jQuery.advQuery.colList[0].columns, function (item, idx) {
                        return item.isDefaultColumn;
                    });
                    return $.map(cols, function (item) {
                        return {//增加列时的列属性
                            colId: item.colId,
                            format: "",
                            sequence: 0,
                            sort: -1,
                            width: item.defaultWidth,
                            visible: 1,
                            displayName: item.colDisplayName,
                            tabDisplayName: item.table.tabDisplayName,
                            tabId: item.table.tabId,
                            isFix: item.isFix
                        };
                    });
                }
                else {
                    return $.map(jQuery.advQuery.setting.viewColumns, function (item) {
                        return {//增加列时的列属性
                            colId: item.colId,
                            format: item.format,
                            sequence: item.sequence,
                            sort: item.sort,
                            width: item.width,
                            visible: item.visible,
                            displayName: item.displayName,
                            tabDisplayName: item.tabDisplayName,
                            tabId: item.tabId,
                            isFix: item.isFix
                        };
                    });
                }
            },
            showColumnsDialog: function () {//显示列属性对话框
                var srcColumns = jQuery.advQuery.getViewColumns();
                var tle = "请选择将要在此视图中显示的列";
                var url = "ColumnDlg.aspx?t=" + encodeURI(tle) + "&tabId=" + jQuery.advQuery.setting.id + "&r=" + Math.random();
                var dlgHeight = 415;
                var dlgWidth = 690;
                var retVal = null;
                $.advQuery.showDialog(url, srcColumns, dlgWidth, dlgHeight, function (rs) {
                    retVal = rs;
                });
                if (retVal && retVal.length > 0) {
                    var idx = 0;
                    var colArray = $.map(retVal, function (item) {
                        idx++;
                        return {//用于显示的视图列
                            id: idx,
                            viewId: jQuery.advQuery.setting.view.id,
                            alias: "",
                            colId: item.colId,
                            format: item.format,
                            sequence: item.sequence,
                            sort: item.sort,
                            width: item.width,
                            visible: item.visible,
                            displayName: item.displayName,
                            tabDisplayName: item.tabDisplayName,
                            tabId: item.tabId,
                            isFix: item.isFix
                        };
                    });
                    jQuery.advQuery.setting.viewColumns = colArray;
                }
            },
            showDialog: function (url, winArgs, dlgWidth, dlgHeight, callback) {//模式对话框无法在页内部调整其窗口大小,使用时须指定其宽度和高度
                var winfeature = "help:no;scrollbars:no;";
                if ($.browser && $.browser.msie && $.browser.version == "6.0") {
                    dlgHeight = (parseInt(dlgHeight) + 50).toString() + "px";
                }
                if (dlgHeight && dlgWidth) {
                    var dlgLeft = $(document.body).outerWidth(true) / 2 - dlgWidth / 2;
                    var dlgTop = $(document.body).outerHeight(true) / 2 - dlgHeight / 2;
                    winfeature += "dialogHeight:" + dlgHeight + "px;dialogWidth:" + dlgWidth + "px;dialogLeft:" + dlgLeft + ";dialogTop:" + dlgTop + ";"
                }
                try {
                    var option = { height: dlgHeight, width: dlgWidth, left: dlgLeft, top: dlgTop, pathurl: url, parameter: winArgs };
                    if (window.top.MyshowModalDialog && !window.top.IsShowModalDialog()) {

                        window.top.MyshowModalDialog(option, callback);
                    }
                    else if (window.top.MyshowModalDialog && window.top.IsShowModalDialog()) {

                        window.top.MyTwoshowModalDialog(option, callback);
                    }
                    else {
                        if (window.showModalDialog != undefined) {
                            var retVal = window.showModalDialog(url, winArgs, winfeature);
                            if (retVal == undefined) retVal = window.returnValue || window.opener.returnValue;
                            if (callback) {
                                callback(retVal);
                            }
                            return retVal;
                        }
                        else {
                            Alertinformation("请在系统里面调用");
                        }
                    }
                    //MyModalDialog
                    //showModalDialog的替换方法：使用window.open打开窗口，用window.postMessage发送消息到子页面.
                    //详见:https://developer.mozilla.org/zh-CN/docs/DOM/window.postMessage 
                    //chrome浏览器下，请将子页的window.opener.returnValue赋值
                    //                                        if (retVal == undefined) retVal = window.returnValue || window.opener.returnValue;

                    //                                        if (callback) {
                    //                                            callback(retVal);
                    //                                        }
                    //                                        return retVal;
                } catch (e) {

                }
            },
            getNowTimeString: function () {//获取当前时间的字符串形式
                var now = new Date();
                return $.validator.format("{0}{1}", $.timeToString(now).replace(/-/g, "").replace(/ /g, "").replace(/\//g, "").replace(/:/g, ""), now.getMilliseconds());
            },
            getColListById: function (ctlId) {//通过控件ID获取相应表的所有列
                var tabId = 0;
                var rs = null;
                var p = $("#" + ctlId + ":first");
                if (p && p.length > 0 && p.attr("id").toLowerCase() == jQuery.advQuery.setting.divId.toLowerCase()) {//传入的参数为jQuery.advQuery.setting.divId
                    return jQuery.advQuery.colList[0];
                }
                while (p && p.length > 0) {
                    if (p.get(0).tagName.toLowerCase() == "table") {
                        var attr = p.attr("tab");
                        try {
                            if (attr && parseInt(attr) > 0) {
                                tabId = parseInt(attr);
                                break;
                            }
                        } catch (e) { }
                    }
                    p = p.parent();
                    if (p.length == 0) break; //没有父节点
                    if (p.attr("id") && p.attr("id").toLowerCase() == jQuery.advQuery.setting.divId.toLowerCase()) {
                        break;
                    }
                }
                if (tabId > 0) {
                    jQuery.each(jQuery.advQuery.colList, function (idx, item) {
                        if (item.columns[0].table.tabId == tabId) {
                            rs = item;
                        }
                    });
                }
                else {
                    return jQuery.advQuery.colList[0];
                }
                return rs;
            },
            getSubTabContainer: function (ctlId) {//通过控件ID获取相应父级表的容器ID
                var cid = jQuery.advQuery.setting.divId;
                if (ctlId.toLowerCase() == cid.toLowerCase()) return cid; //传入的参数为jQuery.advQuery.setting.divId
                var p = $("#" + ctlId + ":first");
                while (p && p.length > 0) {
                    if (p.get(0).tagName.toLowerCase() == "table") {
                        var attr = p.attr("subTabContainer");
                        try {
                            if (attr && attr == 'true') {
                                cid = p.attr("id");
                                cid = "tpcol" + cid.substr(3, cid.length - 3);
                                break;
                            }
                        } catch (e) { }
                    }
                    p = p.parent();
                    if (p.length == 0) break; //没有父节点
                    if (p.attr("id") && p.attr("id").toLowerCase() == jQuery.advQuery.setting.divId.toLowerCase()) {
                        break;
                    }
                }
                return cid;
            },
            getSubTab: function (ctlId) {
                var cid = jQuery.advQuery.getSubTabContainer(ctlId);
                if (cid == jQuery.advQuery.setting.divId) return cid;
                var id = cid.substr(5, cid.length - 5);
                return "tab" + id;
            },
            //colList:父级列的列表
            //childColList:跟据选中的父级获取到的子级列的列表,
            //currId:当前生成的唯一id
            //pCtlId;邻近控件的ID
            buildRelationQuery: function (colList, childColList, currId, pCtlId) {//选中的列发生更改后，选中的是表,构造其内容
                //重复检查
                var allTabs = $("#" + pCtlId).parent().children();
                var rs = jQuery.grep(allTabs, function (item, idx) {
                    if ($(item).attr("tab") == childColList.columns[0].table.tabId &&
                            $(item).attr("type") == "tpitem") {
                        return true;
                    }
                    return false;
                });
                if (rs.length > 0) {
                    Alertinformation("查询中已存在要添加的关系!"); return;
                }

                var alias = jQuery.advQuery.getNowTimeString();
                var tabContainer = "<table id='tab" + currId + "' tab='" + childColList.columns[0].table.tabId + "' subTabContainer='true' style='margin:2px 2px 0px 0xp;' type='tpitem' alias='ali" + alias + "'><tr id='row_0_" + currId + "' style='background-color:#A7CDF0;'>";
                tabContainer += "<td style='padding:0px 10px;width:10px;cursor:pointer;border:solid #000064; border-width:1px 0px 1px 1px;' onclick='jQuery.advQuery.showMenu(" + currId + ")' id='menu" + currId + "'><img src='../../images/CRM_Images/dropdown.gif' alt='select' /></td>";
                tabContainer += "<td style='border:solid #000064;border-width:1px 1px 1px 0px;'>";
                var select = "<select Id='tpsel" + currId + "' role='tab' onchange='jQuery.advQuery.selectTableChanged(" + currId + ")' style='width:150px;'>";
                jQuery.each(colList.relationTables, function (idx, item) {
                    if (item.tagTable.tabId == childColList.columns[0].table.tabId) {//选中的表
                        select += "<option value='" + item.tagTable.tabId + "' selected='true'>" + item.tagTable.tabDisplayName + "</option>";
                    }
                    else {
                        select += "<option value='" + item.tagTable.tabId + "'>" + item.tagTable.tabDisplayName + "</option>";
                    }
                });
                select += "</select>";
                tabContainer += select;
                tabContainer += "</td>";
                tabContainer += "</tr>";
                tabContainer += "<tr id='row_1_" + currId + "'>";
                tabContainer += "<td>&nbsp;";
                tabContainer += "</td>";
                tabContainer += "<td id='tpcol" + currId + "'>";
                tabContainer += "</td>";
                tabContainer += "</tr>";
                tabContainer += "</table>";
                $("#" + pCtlId).after(tabContainer);
                jQuery.advQuery.cdi_Id++;
                jQuery.advQuery.buildColumnList(childColList, "tpcol" + currId, jQuery.advQuery.cdi_Id);
            },
            //colList:当前表中所有列的列表
            //parentID:内容的父容器ID
            //id:生成控件时将要使用的id值,唯一值，一般调用函数前要生成一个id
            //sender:触发更改的控件ID,sender存在的情况下，将内容插入到sender的外部，否则插入到parentID的内部
            buildColumnList: function (colList, parentID, id, sender) {//构造一个条件选择表
                var select = "<select style='width:300px;' role='col' Id='sel" + id + "' onchange='jQuery.advQuery.selectColumnChanged(" + id + ",this)'><option selected>选择</option>";
                select += "<optgroup label='字段' type='pcol'>";
                jQuery.each(colList.columns, function (idx, item) {
                    select += "<option value='" + item.colId + "' type='col'>" + item.colDisplayName + "</option>"
                });
                select += "</optgroup>";
                if (colList.relationTables && colList.relationTables.length > 0) {
                    select += "<optgroup label='相关' type='ptab'>";
                    jQuery.each(colList.relationTables, function (idx, item) {
                        select += "<option value='" + item.tagTable.tabId + "' type='tab'>" + item.tagTable.tabDisplayName + "</option>"
                    });
                    select += "</optgroup>";
                }
                select += "</select>";
                var content = "<table Id='tab" + id + "' tab='" + colList.columns[0].table.tabId + "' type='item' style='margin-top:2px;' selectList='true'><tr Id='row" + id + "'><td style='width:150px;'>" + select + "</td></tr></table>";
                var ctlPrt = $("#" + parentID);
                if (sender && $("#" + sender).length > 0) {
                    $("#" + sender).after(content);
                }
                else {
                    ctlPrt.append(content);
                }
            },
            buildContent: function (colType, colId, id) {//选中的列发生更改后，修改其内容
                var tr = $("#row" + id);
                var colList = jQuery.advQuery.getColListById("row" + id);
                var children = tr.children();
                var select = "";
                if (colType == "col") {//选中列时
                    select += "<select Id='sel" + id.toString() + "_1' role='ope' onchange='jQuery.advQuery.selectOperatorChanged(" + id.toString() + ",this)' style='width:150px;'>";
                    var colArray = jQuery.grep(colList.columns, function (item, idx) {
                        return item.colId == colId;
                    });
                    var opts = jQuery.grep(jQuery.advQuery.operatorList, function (item, idx) {
                        return item.OperatorType == colArray[0].OperatorType;
                    });
                    jQuery.each(opts, function (idx, item) {
                        select += "<option value='" + item.OperatorID + "' showType='" + item.ShowType + "' colId='" + colId + "'>" + item.OperatorValue + "</option>";
                    });
                    select += "</select>";

                    if (children.length == 1) {
                        select = "<td style='width:150px;'>" + select;
                        select += "</td>";
                        select += "<td>&nbsp;</td>";
                        tr.append(select);
                        tr.prepend("<td style='padding:0px 10px;width:10px;cursor:pointer;' onclick='jQuery.advQuery.showMenu(" + id + ")' id='menu" + id + "'><img src='../../images/CRM_Images/dropdown.gif' alt='select' /></td>");
                        var tb = $("#tab" + id);
                        tb.css({ 'background-color': '#f9f9f9', 'border': 'solid #A6BADA', 'border-width': '0px 0px 1px 1px' });
                        jQuery.advQuery.selectOperatorChanged(id, $("#sel" + id + "_1"));
                    }
                    else {
                        children[children.length - 2].innerHTML = select;
                        jQuery.advQuery.selectOperatorChanged(id, $("#sel" + id + "_1"));
                    }
                    $("#tab" + id).css({ "margin": "0px" });

                    //增加一个条件选择行
                    var hasSelectList = false;
                    var tabContainer = jQuery.advQuery.getSubTabContainer("row" + id);
                    var tbs = $("#" + tabContainer + ">table");
                    var hasItem = tbs.filter("[type='item']");
                    if (hasItem.length > 0) {
                        hasItem.each(function (idx, item) {
                            if ($(item).children().children().children().length == 1) {
                                hasSelectList = true;
                            }
                        });
                    }
                    if ($(tbs.get(tbs.length - 1)).children().children().children().length != 1 && !hasSelectList) {
                        jQuery.advQuery.cdi_Id++;
                        var colList = jQuery.advQuery.getColListById(tabContainer);
                        jQuery.advQuery.buildColumnList(colList, tabContainer, jQuery.advQuery.cdi_Id, "tab" + id);
                    }
                    changeState = "loaded"; //选中状态已更改完成
                }
                else if (colType == "tab") {//选中表时
                    //增加一个表，表增加一个subTabContainer='true'的属性，用来标识为相关实体的父级容器
                    var childColList = null;
                    var tbs = jQuery.grep(jQuery.advQuery.colList, function (item, idx) {
                        return item.columns[0].table.tabId == colId;
                    });
                    jQuery.advQuery.cdi_Id++;
                    if (tbs.length == 0) {
                        advFind.GetServerMethodValue(
                            'GetAdvQueryColumns',
                            "tabID=" + colId,
                            function (msg, e) {
                                var dataObj = msg;
                                if (dataObj.d != undefined && dataObj.d.length > 0) {
                                    if (!jQuery.advQuery.colList) {
                                        Alertinformation("please call init method on document load completed!");
                                        return;
                                    }
                                    var columns = dataObj.d;
                                    childColList = columns[0];
                                    jQuery.advQuery.colList.push(childColList);
                                    jQuery.advQuery.buildRelationQuery(colList, childColList, jQuery.advQuery.cdi_Id, "tab" + id);
                                }
                                changeState = "loaded"; //选中状态已更改完成
                            });
                    }
                    else {
                        childColList = tbs[0];
                        jQuery.advQuery.buildRelationQuery(colList, childColList, jQuery.advQuery.cdi_Id, "tab" + id);
                        changeState = "loaded"; //选中状态已更改完成
                    }
                }

                //当滚动条原本就在底部附近时滚动到底部
                var ij = $("#" + jQuery.advQuery.setting.divId).parent(); //最外部容器
                if (ij.length == 1 && ((ij.get(0).scrollHeight - ij.get(0).scrollTop) < 50))
                    ij.get(0).scrollTop = ij.get(0).scrollHeight;
            },
            selectTableChanged: function (id) {

            },
            selectColumnChanged: function (id, obj) {//选中的列发生更改时
                var cknode = $(obj).find("option:selected");
                if (cknode.html() == "选择") return;
                obj.style.width = '150px';
                var tr = $("#row" + id);
                var colId = obj.value;
                var colType = $(obj).find("option:selected").attr("type"); //标识为列，还是表
                var children = tr.children();
                if (children.length == 1 && typeof (jQuery.advQuery.operatorList) == "undefined") {
                    advFind.GetServerMethodValue(
                        "GetOperators",
                        "",
                        function (msg, e) {
                            var dataObj = msg;
                            if (dataObj.d && dataObj.d.length > 0) {
                                var opts = dataObj.d;
                                jQuery.advQuery.operatorList = opts;
                                jQuery.advQuery.buildContent(colType, colId, id);
                            }
                        });
                }
                else {
                    jQuery.advQuery.buildContent(colType, colId, id);
                }
                if (obj[0].innerHTML == "选择") {//第一次选择处理
                    if (colType == "col") {
                        obj.remove(0);
                        var tb = $(obj).children("[type='ptab']"); //移除相关选择
                        tb.remove();
                        $("#tab" + id).removeAttr("selectList");
                    }
                    else {
                        $(obj).find("option:selected").removeAttr("selected");
                        $($(obj).children().get(0)).attr("selected", "true");
                        $(obj).css({ "width": "300px" });
                    }
                }
            },
            selectOperatorChanged: function (id, obj) {//选中的操作发生更改时
                if (obj.jquery != undefined) {//obj为jquery对象
                    obj = obj.get(0);
                }
                var sv = $("#" + obj.id).find("option:selected");
                var st = sv.attr("showType");
                var cid = sv.attr("colId");
                var col = $("#sel" + id).find("option:selected").text();
                var ctlHTML = "";
                var validStr = "";
                //移除已存在的规则
                if ($("#val" + id).length > 0) {
                    $("#val" + id).rules("remove");
                }
                switch (parseInt(st)) {
                    case advEnum.displayValue:
                        ctlHTML = "<input type='text' role='val'  id='val" + id + "' name='val" + id + "' vtype='" + st + "' onblur='jQuery.advQuery.ckinput(this," + id + ")' mask='\\d+' class='advq_input1'  />";
                        validStr = $.validator.format("$(\"#val\"+id).rules(\"add\",{required:true,number:true,messages:{required:\"\\\"{0}\\\"不能为空\",number:\"\\\"{0}\\\"只能为数字\"}})", col);
                        break;
                    case advEnum.displayString:
                        ctlHTML = "<input type='text' role='val'  id='val" + id + "' name='val" + id + "' vtype='" + st + "' onblur='jQuery.advQuery.ckinput(this," + id + ")' mask='.+' class='advq_input1' />";
                        validStr = $.validator.format("$(\"#val\"+id).rules(\"add\",{required:true,messages:{required:\"\\\"{0}\\\"不能为空\"}})", col);
                        break;
                    case advEnum.displayDict:
                        ctlHTML = "<span id='span" + id + "'><input type='text' role='val' id='val" + id + "' name='val" + id + "' vtype='" + st + "' onblur='jQuery.advQuery.ckinput(this," + id + ")' style='vertical-align:middle;' class='advq_input2' readonly='true' /><input id='dict" + id + "' type='button' class='nockdict' style='vertical-align:middle;' hidefocus='true' /></span>";
                        validStr = $.validator.format("$(\"#val\"+id).rules(\"add\",{required:true,messages:{required:\"\\\"{0}\\\"不能为空\"}})", col);
                        break;
                    case advEnum.displayDate:
                        ctlHTML = "<input type='text' role='val'  id='val" + id + "' name='val" + id + "' vtype='" + st + "' onfocus=\" WdatePicker({dateFmt:'yyyy-MM-dd'});\" onblur='jQuery.advQuery.ckinput(this," + id + ")' class='advq_input1' />";
                        validStr = $.validator.format("$(\"#val\"+id).rules(\"add\",{required:true,date:true,messages:{required:\"\\\"{0}\\\"不能为空\",date:\"\\\"{0}\\\"只能为日期型数据\"}})", col);
                        break;
                }
                var tr = $("#row" + id);
                var children = tr.children();
                if (children.length > 3) {
                    children[children.length - 1].innerHTML = ctlHTML;
                    eval(validStr);
                }
                //为字典选择按钮增加事件
                if ($("#dict" + id).length > 0) {
                    $("#dict" + id).hover(function () {
                        $(this).removeClass();
                        $(this).addClass("ckdict");
                    },
                            function () {
                                $(this).removeClass();
                                $(this).addClass("nockdict");
                            });
                    $("#dict" + id).click(function () {
                        var colList = jQuery.advQuery.getColListById("val" + id);
                        var srcd = $("#val" + id).data("relVal");
                        var col = $("#sel" + id).find("option:selected");
                        var colVal = col.val();
                        var colTxt = col.text();
                        var rscols = jQuery.grep(colList.columns, function (item, idx) {
                            return item.colId == colVal;
                        });
                        var relColId = rscols[0].relationCol; //相关列的ID
                        var cdiPlus = rscols[0].conditionPlus; //列额外条件
                        var tle = $.validator.format("请选择[{0}]", colTxt);
                        var dlgHeight = 540;
                        var dlgWidth = 500;
                        var retVal = null;
                        var url = "FindDlg.aspx?cid=" + relColId + "&cps=" + encodeURIComponent(cdiPlus) + "&t=" + encodeURI(tle) + "&r=" + Math.random();
                        $.advQuery.showDialog(url, srcd, dlgWidth, dlgHeight, function (rs) {
                            retVal = rs;
                        });
                        if (retVal) {
                            var ret = $.evalJSON(retVal);
                            if (ret) {
                                var tabIcon = "";
                                $("#val" + id).data("relVal", retVal);
                                var retjoin = "";
                                $(ret).each(function (idx, item) {
                                    retjoin += item.mainColVal + ",";
                                    if (tabIcon == "") {
                                        tabIcon = item.tabIcon;
                                    }
                                });
                                if (retjoin.length > 0) {
                                    retjoin = retjoin.substr(0, retjoin.length - 1);
                                }
                                $("#val" + id).val(retjoin);
                                if (tabIcon) {
                                    $("#val" + id).css({ "background": "url(../../images/crm_images/" + tabIcon + ") no-repeat 2px center", "padding-left": "20px", "width": "180px" });
                                }
                            }
                        }
                        else if (retVal == "") {//未选中任何选项
                            $("#val" + id).data("relVal", retVal);
                            $("#val" + id).val("");
                            $("#val" + id).css({ "background": "" });
                        }
                    });
                    $("#val" + id).dblclick(function () {
                        $("#dict" + id).click();
                    });
                }
            },

            ckinput: function (obj, id) {
            },
            validSubmit: function () {
            },
            getId: function (str) {
                return str.substr(3, str.length - 3);
            },
            getItem: function (id) {
                var colId = $("#sel" + id).val();
                var colList = jQuery.advQuery.getColListById("sel" + id);
                var col = jQuery.grep(colList.columns, function (item, idx) {
                    return item.colId == colId;
                });
                var optId = $("#sel" + id + "_1").val();
                var opt = jQuery.grep(jQuery.advQuery.operatorList, function (item, idx) {
                    return item.OperatorID == optId && col[0].OperatorType == item.OperatorType
                });
                var val = $("#val" + id).val();
                return ({
                    column: {
                        id: 'sel' + id,
                        value: col[0]
                    },
                    operator: {
                        id: 'sel' + id + "_1",
                        value: opt[0]
                    },
                    value: {
                        id: 'val' + id,
                        value: val
                    }
                });
            },
            showMenu: function (id) {//显示菜单
                var options = {
                    minWidth: 120,
                    showDelay: 0,
                    hideDelay: 0,
                    arrowSrc: '../JQuery/images/arrow_right.gif'
                };
                var ckstr = "";
                var ntinfo = "确定要删除该条子句吗？";
                var tbtype = $("#tab" + id).attr("type");
                if (advMenu == null) {
                    advMenu = new $.Menu('#menu' + id, null, options);
                }
                else {
                    if ($.browser.mozilla) {
                        if ($("#root-menu-div").length > 0) {
                            $("#root-menu-div").children().remove();
                        }
                    }
                    else {
                        advMenu.destroy();
                    }
                    advMenu = new $.Menu('#menu' + id, null, options);
                }
                if (tbtype != "tpitem") {//非相关菜单
                    var sel = $("#tab" + id).attr("selected");
                    if (sel == null || sel == undefined) {
                        sel = 'false';
                    }
                    if (sel == 'true')
                        ckstr = tbtype == "item" ? "取消选择行" : "取消选择组";
                    else
                        ckstr = tbtype == "item" ? "选中行" : "选择组";
                    advMenu.addItem(
                                new $.MenuItem({ src: ckstr, url: "" }, $.extend({}, options, { onClick: function () {
                                    if (sel == 'true') {
                                        $("#tab" + id).css({ 'background-color': '#f9f9f9' });
                                        $("#tab" + id).attr("selected", 'false');
                                    }
                                    else {
                                        $("#tab" + id).attr("selected", 'true');
                                        $("#tab" + id).css({ 'background-color': '#A7CDF0' });
                                    }
                                }
                                })));
                    if (tbtype == "pitem") {
                        ntinfo = "确定删除该组及组下的所有子句吗？";
                        advMenu.addItem(
                            new $.MenuItem({ src: "取消组合", url: "" }, $.extend({}, options, { onClick: function () {
                                var tbprev = $("#tab" + id).prev();
                                var tbs = $("#tab" + id + ">tbody>tr>td>table");
                                var lasttlb = $(tbs.get(tbs.length - 1));
                                if (lasttlb.attr("type") == "item")
                                    lasttlb.css({ "border-bottom": "solid 1px #A6BADA" }); //只有是行时，才还原底边框
                                if (tbprev.length > 0) {
                                    tbprev.after(tbs.clone(true));
                                }
                                else {
                                    var tbparent = $("#tab" + id).parent();
                                    tbparent.prepend(tbs.clone(true));
                                }
                                $("#tab" + id).remove();
                            }
                            })));
                        var s = $("#menu" + id).attr("state");
                        var uns = s == 0 ? "和" : "或"; //相反的
                        advMenu.addItem(
                                new $.MenuItem({ src: "更改为 " + uns, url: "" }, $.extend({}, options, { onClick: function () {
                                    $("#menu" + id).attr("state", Math.abs(parseInt(s) - 1).toString());
                                    $("#menu" + id).children().get(0).nextSibling.nodeValue = uns;
                                }
                                })));
                        advMenu.addItem(
                                new $.MenuItem({ src: "添加子句", url: "" }, $.extend({}, options, { onClick: function () {
                                    var ctlbs = $("#tab" + id + ">tbody>tr>td[id=pcol" + id + "]>table");
                                    var lasttb = $(ctlbs.get(ctlbs.length - 1));
                                    if (lasttb.attr("type") == "item") {
                                        lasttb.css({ "border-bottom": "solid 1px #A6BADA" });
                                    }
                                    jQuery.advQuery.cdi_Id++;
                                    var colList = jQuery.advQuery.getColListById("tab" + id);
                                    jQuery.advQuery.buildColumnList(colList, "pcol" + id, jQuery.advQuery.cdi_Id);
                                }
                                })));
                    }
                }
                if (tbtype == "tpitem") {//相关菜单
                    ntinfo = "确定删除该相关实体及实体下的所有子句吗？";
                    var oped = $("#tab" + id).attr("opened");
                    if (oped == null || oped == undefined) {
                        oped = 'true';
                    }
                    if (oped == 'false') {
                        ckstr = "展开";
                    }
                    else {
                        ckstr = "折叠";
                    }
                    advMenu.addItem(
                                new $.MenuItem({ src: ckstr, url: "" }, $.extend({}, options, { onClick: function () {
                                    if (oped == 'true') {
                                        $("#tab" + id).attr("opened", 'false');
                                        $("#row_1_" + id).css({ "display": "none" });
                                    }
                                    else {
                                        $("#tab" + id).attr("opened", 'true');
                                        if (jQuery.browser.msie) {
                                            $("#row_1_" + id).css({ "display": "block" });
                                        }
                                        else {
                                            $("#row_1_" + id).removeAttr("style");
                                        }
                                    }
                                }
                                }))
                                );
                }

                advMenu.addItem(
                new $.MenuItem({ src: "删除", url: "", icon: "../JQuery/images/cancel.png" }, {
                    onClick: function () {
                        Alertconfirm(ntinfo, function () {
                            $("#tab" + id).remove();
                        });
                    }
                }));
                $('.menu-div').bgiframe({ opacity: true });
                //$('#menu'+id).click();
            },
            clearCondition: function (state) {//清空所有条件。state=0为不提示
                if ($("#" + jQuery.advQuery.setting.divId + " table").length > 1) {
                    if (state == undefined || state == null || state) {
                        Alertconfirm("确定清空所有子句吗?", function () {
                            var v = $("#" + $.advQuery.setting.divId);
                            var vt = v.children("table");
                            vt.each(function (idx, item) {
                                if (item.children[0].children[0].children.length != 1) {
                                    $("#" + item.id).remove();
                                }
                            });
                            return true;
                        });

                    }
                }

            },
            //state:0:或操作;1:与操作
            operateCondition: function (state) {//对条件进行与或操作时
                var errInfo = "若要对条件进行分组，请选择同一实体下的一个或多个条件或组合，然后单击组\"和\"，或组\"或\"。\n新组中不能包含已分组的各个条件，并且相关实体条件或组不能与其父实体条件或组分在一起。";
                var cktb = $("#" + $.advQuery.setting.divId + " table[selected=true]");
                var prt = null, newPrt = null;
                var rs = true;
                if (cktb.length < 2) {
                    Alerterror(errInfo);
                    return;
                }
                //判断父元素是否为同一元素
                cktb.each(function (idx, item) {
                    if (prt == null) {
                        prt = $("#" + item.id).parent();
                    }
                    else {
                        newPrt = $("#" + item.id).parent();
                        if (prt[0].id != newPrt[0].id) {
                            rs = false;
                        }
                    }
                });
                if (rs) {
                    //判断是否选中同一父元素下的所有子元素,只检查已分组的实体
                    if (prt.get(0).tagName.toLowerCase() == 'td') {
                        if (prt.children().length == cktb.length) {
                            prt.children().css({ "background-color": "#f9f9f9" });
                            prt.children().attr("selected", "false");
                            prt = $(prt.parent().children().get(0));
                            var src_sta = prt.attr("state");
                            if (parseInt(src_sta) == state) return; //无须更改
                            prt.attr("state", state);
                            prt.children().get(0).nextSibling.nodeValue = (state == 1 ? "和" : "或");
                            return;
                        }
                    }
                    jQuery.advQuery.cdi_Id++;
                    var ftlb = $(cktb.get(0));
                    var opt = state == 0 ? "或" : "和";
                    cktb.wrapAll("<table id='tab" + jQuery.advQuery.cdi_Id + "' type='pitem' style='margin:3px 0px; ' cellpadding=0 cellspacing=0><tr id='row" + jQuery.advQuery.cdi_Id + "' style='padding:0px 0px 0px 5px; border-left:solid 1px #A6BADA;margin:0;'><td id='pcol" + jQuery.advQuery.cdi_Id + "' style='background-color:#f9f9f9;border-bottom:solid 1px #f9f9f9;'></td></tr></table>");
                    $("#tab" + jQuery.advQuery.cdi_Id).css({ "background-color": "#f9f9f9" });
                    $("#tab" + jQuery.advQuery.cdi_Id).children("tbody").css({ "width": "100%" });
                    $("#row" + jQuery.advQuery.cdi_Id).prepend("<td style='width:30px;cursor:pointer;border:solid 1px #A6BADA;' onclick='jQuery.advQuery.showMenu(" + jQuery.advQuery.cdi_Id + ")' id='menu" + jQuery.advQuery.cdi_Id + "' state='" + state + "'><img src='../../images/CRM_Images/dropdown.gif' alt='select' />" + opt + "</td>")
                    $("#row" + jQuery.advQuery.cdi_Id + ">td>table").css({ "background-color": "#f9f9f9", "border-left": "solid 1px #A6BADA" });
                    var tbs = $("#row" + jQuery.advQuery.cdi_Id + ">td>table");
                    var lasttb = $(tbs.get(tbs.length - 1));
                    if (lasttb.attr("type") == "item") {
                        lasttb.css({ "border-bottom": "solid 1px #f9f9f9" }); //只有是行时，才隐藏底边框
                    }
                    tbs.attr("selected", "false");
                }
                else {
                    Alerterror(errInfo);
                }
            },
            saveView: function (saveAs, callback) {//保存视图(saveAs：0);另存为视图(saveAs：1)
                if (!$(jQuery.advQuery.setting.formId).valid()) return;
                if ($("#" + jQuery.advQuery.setting.divId + ">table").length <= 1) {
                    Alerterror("请指定条件后再保存！");
                    return;
                }
                jQuery.advQuery.setting.conditions = null;
                jQuery.advQuery.setting.relations = null;
                jQuery.advQuery.setting.tables = null;
                if (jQuery.advQuery.setting.viewColumns != null && jQuery.advQuery.setting.viewColumns.length == 1 && jQuery.advQuery.setting.viewColumns[0].colId == 0) {//未设置视图列
                    jQuery.advQuery.setting.viewColumns = null;
                }
                if (saveAs) {
                    jQuery.advQuery.setting.view.id = 0;
                }
                if (jQuery.advQuery.setting.view.name == "") {
                    var retVal = jQuery.advQuery.showViewDlg();
                    if (retVal) {
                        jQuery.advQuery.setting.view.name = retVal.name;
                        jQuery.advQuery.setting.view.attribute = retVal.attrInfo;
                        jQuery.advQuery.setting.view.isysview = retVal.isysview;
                    }
                    else {
                        return;
                    }
                }
                jQuery.advQuery.setting.view.ower_rec = $.subUserRec();
                jQuery.advQuery.setting.view.account_rec = $.accountRec();

                window.status = "正在保存视图……";
                jQuery.advQuery.setting.view.mainTabId = jQuery.advQuery.setting.id;
                jQuery.advQuery.setting.conditions = new Array();
                advFind.GetServerMethodValue("SaveView", "view=" + $.toJSON(jQuery.advQuery.setting.view), function (msg, result) {
                    if (msg.d > 0) {
                        //获取到视图的唯一ID
                        jQuery.advQuery.setting.view.id = msg.d;
                        var viewId = jQuery.advQuery.setting.view.id;

                        var divId = jQuery.advQuery.setting.divId;

                        //组合条件与关系
                        var topTabs = $("#" + divId + ">table");
                        var topTabs = jQuery.grep(topTabs, function (item, idx) {
                            if (!$(item).attr("selectList")) {
                                return true;
                            }
                            return false;
                        });
                        if (topTabs.length > 0) {
                            var ctlId = 0, cdiMaxId = 1, relMaxId = 1;
                            var childArray = new Array();
                            var rels = new Array();
                            jQuery.each(topTabs, function (idx, item) {
                                ctlId = item.id.substr(3, item.id.length - 3);
                                switch ($(item).attr("type")) {
                                    case "item":
                                        childArray.push({
                                            id: cdiMaxId,
                                            viewId: viewId,
                                            alias: $("#" + divId).attr("alias"),
                                            colId: $("#sel" + ctlId).val(),
                                            operator: $("#sel" + ctlId + "_1").val(),
                                            value: $("#val" + ctlId).editVal()
                                        });
                                        cdiMaxId++;
                                        break;
                                    case "pitem":
                                        var relation = $("#menu" + ctlId).attr("state");
                                        var rs = jQuery.advQuery.visitPitem(ctlId, cdiMaxId, "pcol" + ctlId, viewId, relation, relMaxId);
                                        //条件
                                        jQuery.merge(childArray, rs.cdiArray);
                                        cdiMaxId += rs.cdiArray.length;

                                        //关系
                                        jQuery.merge(rels, rs.cdiRelationArray);
                                        relMaxId += rs.cdiRelationArray.length;
                                        break;
                                    case "tpitem":
                                        var rs = jQuery.advQuery.visitPitem(ctlId, cdiMaxId, "tpcol" + ctlId, viewId, null, relMaxId);
                                        //条件
                                        jQuery.merge(childArray, rs.cdiArray);
                                        cdiMaxId += rs.cdiArray.length;

                                        //关系
                                        jQuery.merge(rels, rs.cdiRelationArray);
                                        relMaxId += rs.cdiRelationArray.length;
                                        break;
                                }
                            });
                            jQuery.advQuery.setting.conditions = childArray;
                            jQuery.advQuery.setting.relations = rels;
                        }
                        /*
                        对于类似[a][b],[c][d]的说明：
                        a,c代表CRM_AdvQuery_CONDITION_Relation或CRM_AdvQuery_Condition中的主键
                        b,d用来标识a,c属于哪张表中的主键:
                        (1)当b或d为0时，表示a或c为CRM_AdvQuery_Condition表中的主键
                        (2)当b或d为1时,表示a或c为CRM_AdvQuery_CONDITION_Relation表中的主键
                        */

                        //组合表
                        tabMaxId = 1;
                        var mainAli = $("#" + divId).attr("alias");
                        var tabRelations = new Array();
                        var relTabs = $("#" + divId + ">table[type='tpitem']");
                        jQuery.each(relTabs, function (idx, item) {
                            var tagTabId = parseInt($(item).attr("tab"));
                            var tabRels = jQuery.advQuery.getColRelation(jQuery.advQuery.setting.id, tagTabId).relationColumns;
                            var tagTabAli = $("#" + jQuery.advQuery.getSubTab(item.id)).attr("alias");
                            tabRelations.push({
                                id: tabMaxId,
                                viewId: viewId,
                                tabId: jQuery.advQuery.setting.id,
                                tabAli: mainAli,
                                colId: tabRels[0].srcColumn.colId,
                                tabId1: tagTabId,
                                tabAli1: tagTabAli,
                                colId1: tabRels[0].tagColumn.colId
                            });
                            tabMaxId++;
                            var childArray = jQuery.advQuery.visitChildTabItem(tagTabId, tagTabAli, item.id, viewId);
                            if (childArray.length > 0) {
                                jQuery.merge(tabRelations, childArray);
                            }
                        });
                    }
                    else {
                        Alerterror("保存视图失败!");
                    }
                    jQuery.advQuery.setting.tables = tabRelations;

                    //处理视图列
                    if (jQuery.advQuery.setting.viewColumns != null) {
                        $.each(jQuery.advQuery.setting.viewColumns, function (idx, item) {
                            item.viewId = viewId;
                        });
                        if (tabRelations && tabRelations.length > 0) {
                            $.each(tabRelations, function (idx, titem) {
                                $.each(jQuery.advQuery.setting.viewColumns, function (idx, citem) {
                                    citem.alias = "";
                                    if (titem.tabId == citem.tabId) {
                                        citem.alias = titem.tabAli;
                                    }
                                    else if (titem.tabId1 == citem.tabId) {
                                        citem.alias = titem.tabAli1;
                                    }
                                })
                            });
                        }
                    }

                    //特殊处理(当不存在表关系时，无须设置表别名，将条件与视图列的别名清除)
                    if (tabRelations == null || tabRelations == undefined || tabRelations.length == 0) {
                        jQuery.each(jQuery.advQuery.setting.conditions, function (idx, item) {
                            item.alias = "";
                        })
                        if (jQuery.advQuery.setting.viewColumns) {
                            jQuery.each(jQuery.advQuery.setting.viewColumns, function (idx, item) {
                                item.alias = "";
                            })
                        }
                    }

                    //没有条件的情况下无须保存
                    if (jQuery.advQuery.setting.conditions.length == 0) {
                        return;
                        window.status = "未设置条件，视图保存完成。";
                    }

                    //保存数据
                    var args = jQuery.toJSON({
                        condition: jQuery.advQuery.setting.conditions,
                        relation: jQuery.advQuery.setting.relations,
                        tabRelation: jQuery.advQuery.setting.tables,
                        viewColumn: jQuery.advQuery.setting.viewColumns
                    });
                    var arg = { cdi: args };
                    var parg = jQuery.toJSON(arg);
                    advFind.GetServerMethodValue("SaveCondition", parg, function (msg, result) {
                        window.status = "视图保存完成。";
                        if (!msg.d) {
                            Alerterror("保存失败！");
                        }
                        else {
                            if (callback && $.isFunction(callback)) {
                                callback({
                                    viewId: viewId,
                                    viewName: jQuery.advQuery.setting.view.name
                                });
                            }
                        }
                    }, true, "POST");
                },
                true, "GET");
            },
            saveAsView: function () {
                var args = null;
                retVal = jQuery.advQuery.showViewDlg(args);
                if (retVal) {
                    jQuery.advQuery.setting.view = {
                        name: retVal.name,
                        id: 0,
                        attribute: retVal.attrInfo,
                        queryStatement: "",
                        mainTabId: jQuery.advQuery.setting.id,
                        isysview: retVal.isysview
                    }
                    jQuery.advQuery.saveView(1);
                }
            },
            updateView: function () {
                var args = {
                    name: jQuery.advQuery.setting.view.name,
                    attrInfo: jQuery.advQuery.setting.view.attribute,
                    isysview: jQuery.advQuery.setting.view.isysview
                };
                retVal = jQuery.advQuery.showViewDlg(args);
                if (retVal) {
                    jQuery.advQuery.setting.view.name = retVal.name;
                    jQuery.advQuery.setting.view.attribute = retVal.attrInfo;
                    jQuery.advQuery.setting.view.isysview = retVal.isysview;
                    jQuery.advQuery.saveView(0);
                }
            },
            newBuildView: function () {
                if ($("#" + jQuery.advQuery.setting.divId + " table").length > 1) {
                    if (!window.confirm("确定放弃未保存过的记录？")) {
                        return false;
                    }
                }

                jQuery.advQuery.clearCondition(0);
                jQuery.advQuery.setting.conditions = null;
                jQuery.advQuery.setting.relations = null;
                jQuery.advQuery.setting.tables = null;
                jQuery.advQuery.setting.viewColumns = null;
                jQuery.advQuery.setting.view = {
                    name: "",
                    id: 0,
                    attribute: "",
                    queryStatement: "",
                    mainTabId: jQuery.advQuery.setting.id,
                    isysview: false
                };
                return true;
            },
            showViewDlg: function (args) {//视图属性对话框
                var tle = "视图属性";
                var dlgHeight = 500;
                var dlgWidth = 600;
                var retVal = null;
                var url = "ViewDlg.aspx?t=" + encodeURI(tle) + "&r=" + Math.random();
                $.advQuery.showDialog(url, args, dlgWidth, dlgHeight, function (rs) {
                    retVal = rs;
                });
                return retVal;
            },
            getColRelation: function (srcTabId, tagTabId) {//通过指定的源表与目标表获取表关系
                var rs = null;
                jQuery.each(jQuery.advQuery.colList, function (idx, item) {
                    if (item.columns[0].table.tabId == srcTabId) {
                        var rels = jQuery.grep(item.relationTables, function (citem, idx) {
                            return citem.tagTable.tabId == tagTabId;
                        });
                        if (rels.length > 0) {
                            rs = rels[0];
                        }
                    }
                });
                return rs;
            },
            visitChildTabItem: function (srcTabId, srcTabAli, ctlId, viewId) {//获取表关系的递归
                var rs = new Array();
                var childRelTabs = $("#" + ctlId + ">tbody>tr>td>table[type='tpitem']");
                if (childRelTabs.length > 0) {
                    childRelTabs.each(function (idx, item) {
                        var tagTabId = parseInt($(item).attr("tab"));
                        var tabRels = jQuery.advQuery.getColRelation(srcTabId, tagTabId).relationColumns;
                        var tagTabAli = $("#" + jQuery.advQuery.getSubTab(item.id)).attr("alias");
                        rs.push({
                            id: tabMaxId,
                            viewId: viewId,
                            tabId: srcTabId,
                            tabAli: srcTabAli,
                            colId: tabRels[0].srcColumn.colId,
                            tabId1: tagTabId,
                            tabAli1: tagTabAli,
                            colId1: tabRels[0].tagColumn.colId
                        });
                        tabMaxId++;
                        var childArray = jQuery.advQuery.visitChildTabItem(tagTabId, tagTabAli, item.id, viewId);
                        if (childArray.length > 0) {
                            jQuery.merge(rs, childArray);
                        }
                    });
                }
                return rs;
            },
            visitPitem: function (id, cdiMaxId, containerId, viewId, relation, relMaxId) {//获取列关系的递归
                var retVal = {};
                jQuery.extend(retVal, {
                    cdiArray: null,
                    cdiRelationArray: null
                });
                var ctlId = 0;
                var lastItemType = "", lastPRelation = null;
                var cds = new Array();
                var rels = new Array();
                var cldTabs = $("#" + containerId).children();
                cldTabs = jQuery.grep(cldTabs, function (item, idx) {
                    if (!$(item).attr("selectList")) {
                        return true;
                    }
                    return false;
                });
                jQuery.each(cldTabs, function (idx, item) {
                    ctlId = item.id.substr(3, item.id.length - 3);
                    switch ($(item).attr("type")) {
                        case "pitem":
                            var itemRelation = $("#menu" + ctlId).attr("state");
                            var ars = jQuery.advQuery.visitPitem(ctlId, cdiMaxId, "pcol" + ctlId, viewId, itemRelation, relMaxId);
                            //条件
                            jQuery.merge(cds, ars.cdiArray);
                            cdiMaxId += ars.cdiArray.length;
                            relMaxId += ars.cdiRelationArray.length;

                            //关系
                            if (relation != null && relation != undefined && ars.cdiRelationArray.length > 0) {
                                if (rels.length == 0) {
                                    rels.push({
                                        id: relMaxId,
                                        viewId: viewId,
                                        condition: jQuery.format("[{0}][{1}]", ars.cdiRelationArray[0].id, 1),
                                        relation: relation
                                    });
                                }
                                else {
                                    rels[0].condition += "," + jQuery.format("[{0}][{1}]", ars.cdiRelationArray[0].id, 1);
                                }
                                relMaxId++;
                            }
                            jQuery.merge(rels, ars.cdiRelationArray);
                            break;
                        case "item":
                            //条件
                            cds.push({
                                id: cdiMaxId,
                                viewId: viewId,
                                alias: $("#" + jQuery.advQuery.getSubTab(item.id)).attr("alias"),
                                colId: $("#sel" + ctlId).val(),
                                operator: $("#sel" + ctlId + "_1").val(),
                                value: $("#val" + ctlId).editVal()
                            });

                            //关系
                            if (relation != null && relation != undefined) {
                                if (rels.length == 0) {
                                    rels.push({
                                        id: relMaxId,
                                        viewId: viewId,
                                        condition: jQuery.format("[{0}][{1}]", cdiMaxId, 0), //0:item;1:pitem
                                        relation: relation
                                    });
                                }
                                else {
                                    rels[0].condition += "," + jQuery.format("[{0}][{1}]", cdiMaxId, 0);
                                }
                                relMaxId++;
                            }
                            cdiMaxId++; //放在语句最后
                            break;
                        case "tpitem":
                            var ars = jQuery.advQuery.visitPitem(ctlId, cdiMaxId, "tpcol" + ctlId, viewId, null, relMaxId);
                            //条件
                            jQuery.merge(cds, ars.cdiArray);
                            cdiMaxId += ars.cdiArray.length;
                            relMaxId += ars.cdiRelationArray.length;

                            //关系
                            if (relation != null && relation != undefined && ars.cdiRelationArray.length > 0) {
                                if (rels.length == 0) {
                                    rels.push({
                                        id: relMaxId,
                                        viewId: viewId,
                                        condition: jQuery.format("[{0}][{1}]", ars.cdiRelationArray[0].id, 1),
                                        relation: relation
                                    });
                                }
                                else {
                                    rels[0].condition += "," + jQuery.format("[{0}][{1}]", ars.cdiRelationArray[0].id, 1);
                                }
                                relMaxId++;
                            }
                            jQuery.merge(rels, ars.cdiRelationArray);
                            break;
                    }
                });
                retVal.cdiArray = cds;
                retVal.cdiRelationArray = rels;
                return retVal;
            }

        });
    })(jQuery);

