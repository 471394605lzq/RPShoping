<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AfterModuleRight.aspx.cs" Inherits="RPShoping.Web.After.AfterModuleRight1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>后台模块权限管理</title>
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />

    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.query-2.1.7.js"></script>
    <script src="../../jquery/json2.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSCommon.js"></script>
    <script src="../../jquery/jquery-ui-1.9.2.custom.js"></script>
    <script src="../../jquery/JSAdvQuery.js"></script>

    <style type="text/css">
        .module_top {
            list-style: none;
            width: 100% !important;
            height: auto;
            min-height: 240px !important;
            padding: 0px;
            margin: 0px;
        }

            .module_top .module_li {
                float: left;
                display: block;
                width: 120px;
                height: 55px;
                border: 1px solid red;
                border-radius: 4px;
                text-align: center;
                line-height: 35px;
                color: #ffffff;
                background-color: #ff6a00;
                border: 1px solid #438eb9;
                font-size: 14px;
                margin-top: 20px;
                margin-left: 20px;
                cursor: pointer;
            }
        #toprolediv {
            width: 100%;
            height: 100px;
            border-bottom: 1px dashed #e8e8e8;
            overflow-y: scroll;
        }
            #toprolediv #toproleul {
                width: 100%;
                height: auto;
                list-style: none;
                padding: 0;
                margin: 0;
                display: block;
            }
                #toprolediv #toproleul li {
                    width: auto;
                    height: 25px;
                    border: 1px solid #ddd;
                    float: left;
                    line-height: 25px;
                    color: #626161;
                    padding: 3px 20px;
                    cursor: pointer;
                    margin-left: 20px;
                    border-radius: 3px;
                    margin-top: 10px;
                }
                    #toprolediv #toproleul li:hover {
                        background:#35C4FD;
                        color:#ffffff;
                    }
        .addcolor {
            background: #35C4FD;
            color: #ffffff !important;
        }
        .addcolor2 {
            background: #35C4FD !important;
            color: #ffffff !important;
            border:1px solid #ff6a00;
        }
        #modulediv {
            width: 250px;
            height: 100%;
            min-height: 600px;
            float: left;
            border-right: 1px dashed #e8e8e8;
        }
            #modulediv .leftmodulediv {
                width: 80%;
                height: 45px;
                margin: 15px auto;
                line-height: 45px;
                cursor: pointer;
                background: #438eb9;
                text-align: center;
                color: #ffffff;
                border-radius: 5px;
            }
        .displaynone {
            display:none;
        }
        .displayblock {
            display:initial;
            margin-left: 10px;
        }

        .moduleright {
            background: #438eb9;
            width: auto;
            min-width: 60px;
            height: 30px;
            float: left;
            color: #ffffff;
            text-align: center;
            line-height: 30px;
            font-size:12px;
            border-radius:3px;
            margin-right:10px;
            margin-bottom:10px;
            cursor:pointer;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <div class="datatb_all">
            <div class="datatb_all_mide">
                <div class="datatb_top">
                    <span class="top_title">后台模块权限设置</span>
                    <%--<i id="addmodule" class="fa fa-plus" style="line-height: 75px; color: #0094ff; font-size: 22px; cursor: pointer;" aria-hidden="true"></i>--%>
                </div>
<%--                <div id="toprolediv">
                    <ul id="toproleul">

                    </ul>
                </div>--%>
                <div id="datatb" class="datatb">
                    <div id="modulediv">
<%--                        <div id="div1" class="leftmodulediv">
                            系统设置
                        </div>
                        <div id="div2" class="leftmodulediv">
                            用户管理
                        </div>
                        <div id="div3" class="leftmodulediv">
                            商品管理
                        </div>--%>
                    </div>
                    <div id="childmodulediv" style="height:auto; min-height:600px;">
                        <div style="height: auto; overflow-y: auto;">
                            <ul class="module_top" id="childmoduleul">
<%--                                <li class="module_li">用户管理
                                    <i class="fa fa-check displaynone" aria-hidden="true" style="color: #0efa45;"></i>
                                </li>
                                <li class="module_li">商品管理
                                    <i class="fa fa-check displaynone" aria-hidden="true" style="color: #0efa45;"></i>
                                </li>
                                <li class="module_li">订单管理
                                    <i class="fa fa-check displaynone" aria-hidden="true" style="color: #0efa45;"></i>
                                </li>
                                <li class="module_li">会员中心
                                    <i class="fa fa-check displaynone" aria-hidden="true" style="color: #0efa45;"></i>
                                    
                                </li>--%>
                            </ul>
                        </div>
                    </div>
                </div>
                <div id="showdiv" class="showtemp" style="width:auto; height:auto; min-height:30px; min-width:150px; max-width:500px;padding:10px; border:1px solid #35C4FD; display:none;">
<%--                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>
                    <div class="moduleright showtemp">
                        <i class='fa fa-check' aria-hidden='true' style='color: #0efa45; margin-right: 5px;'></i>新增
                    </div>--%>
                </div>
                <%--<div class="editbottom" style="position: absolute;bottom: 75px;">
                    <div class="btn_div">
                        <input type="button" class="edit_btn" id="cancel" value="关 闭" />
                        <input type="button" class="edit_btn" id="save" value="保 存" style="margin-left: 50px;" />
                    </div>
                </div>--%>
            </div>
        </div>
    </form>
</body>
</html>
<script src="../../control/JS/JSAfterModuleRight.js"></script>
