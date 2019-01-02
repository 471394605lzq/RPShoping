<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systemuseredit.aspx.cs" Inherits="RPShoping.Web.After.systemuseredit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />

    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.query-2.1.7.js"></script>
    <script src="../../jquery/json2.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSCommon.js"></script>
    <script src="../../jquery/jquery-ui-1.9.2.custom.js"></script>
    <script src="../../control/JS/apicloudsha1.js"></script>
    <script src="../../jquery/JSAdvQuery.js"></script>
    <style type="text/css">
        .imagediv {
            width: 100px;
            height: 120px;
            margin-left: 10px;
            margin-top: 10px;
            float:left;
        }
        .image {
            width: 100px;
            height: 100px;
            float: left;
            border: none;
        }
        .closebtn {
            color: red;
            position: absolute;
            margin-left: -15px;
            cursor:pointer;
        }
        .setslt {
            width: 100px;
            height: 25px;
            border: 1px solid #666666;
            color: #ffffff;
            position: absolute;
            text-align:center;
            line-height:25px;
            font-size:10px;
            background:#666666;
            margin-top:75px;
            cursor:pointer;
            display:none;
            opacity:0.9;
        }
        #mainimagecontent {
            width: 100%;
            height: 120px;
            border: 1px solid #e8e8e8;
            margin-top: 10px;
            line-height: 120px;
        }
        .imagetitle {
            width: 100%;
            height: 25px;
            border-bottom: 1px dashed #e8e8e8;
            line-height: 25px;
            text-indent: 1em;
        }
        .mainimage {
            width: 80%;
            margin: 0 auto;
            height: 280px;
        }
    </style>
    <title>编辑客户信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="datatb_all">
            <div class="datatb_all_mide">
                <div class="datatb_top">
                    <span id="back" class="top_back">
                        <i class="fa fa-reply" aria-hidden="true" style="color: #438eb9; margin-right: 5px; font-size: 22px;"></i>
                    </span>
                    <span class="top_title">编辑客户信息</span>
                </div>
                <div class="datatb">
                    <ul style="height: 550px; min-height: 550px; margin-bottom:25px;">
                        <li class="lileft">
                            <span class="titlename">登陆名：</span><span class="requiredstr">*</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="username" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">昵称：</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="nickname" value="" />
                        </li>
                        <li class="lileft">
                            <span class="titlename">账户余额：</span>
                        </li>
                        <li class="liright">
                            <input type="text" class="inputlb" id="balance" value="" />
                        </li>
                    </ul>
                </div>
            </div>
            <div class="editbottom">
                <div class="btn_div">
                    <input type="button" class="edit_btn" id="cancel" value="关 闭" />
                    <input type="button" class="edit_btn" id="save" value="保 存" style="margin-left: 50px;" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script src="../../control/JS/JSsystemuseredit.js"></script>