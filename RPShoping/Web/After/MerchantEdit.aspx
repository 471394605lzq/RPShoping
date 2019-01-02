<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MerchantEdit.aspx.cs" Inherits="RPShoping.Web.After.MerchantEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../../control/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="../../CSS/AfterCommon.css" rel="stylesheet" />


    <script src="../../control/JS/apicloudsha1.js"></script>
    <script src="../../jquery/jquery.min.js"></script>
    <script src="../../jquery/jquery.query-2.1.7.js"></script>
    <script src="../../jquery/json2.js"></script>
    <script src="../../control/layer/layer.js"></script>
    <script src="../../control/JS/JSCommonForAlter.js"></script>
    <script src="../../control/JS/JSCommon.js"></script>
    <script src="../../jquery/jquery-ui-1.9.2.custom.js"></script>
    <script src="../../jquery/JSAdvQuery.js"></script>





    <title>编辑商户信息</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="datatb_all">
        <div class="datatb_all_mide">
            <div class="datatb_top">
                <span id="back" class="top_back">
                    <i class="fa fa-reply" aria-hidden="true" style="color:#438eb9; margin-right:5px; font-size:22px;"></i>
                </span>
                <span class="top_title">编辑商户信息</span>
            </div>
            <div class="datatb">
                <ul>
                    <li class="lileft">
                        <span class="titlename">商户名称：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="name" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">联系电话：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="tel" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">省份：</span>
                    </li>
                    <li class="liright">
                        <select class="inputlb" id="province" runat="server">
                        </select>
                    </li>
                    <li class="lileft">
                        <span class="titlename">城市：</span>
                    </li>
                    <li class="liright">
                        <select class="inputlb" id="city" name="city" runat="server">
                        </select>
                    </li>
                    <li class="lileft">
                        <span class="titlename">镇区：</span>
                    </li>
                    <li class="liright">
                        <select class="inputlb" id="town" name="town" runat="server">
                        </select>
                    </li>
                     <li class="lileft">
                        <span class="titlename">详细地址：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="address" value="" />
                    </li>
                     <li class="lileft">
                        <span class="titlename">经度：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="longitude" value="" />
                    </li>
                     <li class="lileft">
                        <span class="titlename">纬度：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="dimension" value="" />
                    </li>
                     <li class="lileft">
                        <span class="titlename">合作状态：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input id="isparement" type="radio" checked="checked" name="state" value="合作中" /><label for="mb_female">合作中</label>
                        <input id="noparement" type="radio" name="state" value="停止合作" /><label for="mb_male">停止合作</label>
                    </li>
                     <li class="lileft">
                        <span class="titlename">备注：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="remark" value="" />
                    </li>
                </ul>
            </div>
            <div class="editbottom">
                <div class="btn_div">
                    <input type="button" class="edit_btn" id="cancel" value="关 闭" />
                    <input type="button" class="edit_btn" id="save" value="保 存" style="margin-left:50px;" />
                    <input type="button" class="edit_btn" id="movearea" value="移动区域数据" style="margin-left:50px;" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script src="../../control/JS/JSMerchantEdit.js"></script>

