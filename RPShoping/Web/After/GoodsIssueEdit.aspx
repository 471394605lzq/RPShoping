<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsIssueEdit.aspx.cs" Inherits="RPShoping.Web.After.GoodsIssueEdit" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
    <script src="../../jquery/qiniu.min.js"></script>
    <script src="../../jquery/plupload.full.min.js"></script>

    <title>商品期数设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="datatb_all">
        <div class="datatb_all_mide">
            <div class="datatb_top">
                <span id="back" class="top_back">
                    <i class="fa fa-reply" aria-hidden="true" style="color:#438eb9; margin-right:5px; font-size:22px;"></i>
                </span>
                <span class="top_title">商品期数设置</span>
            </div>
            <div class="datatb">
                <ul>
                    <li class="lileft">
                        <span class="titlename">期数：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="issuenumber" readonly="true" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">状态：</span>
                    </li>
                    <li class="liright">
                        <input id="state1" type="radio" checked="checked" name="state" value="进行中" /><label for="mb_female">进行中</label>
                        <input id="state2" type="radio" name="state" value="即将揭晓" /><label for="mb_male">即将揭晓</label>
                        <input id="state3" type="radio" name="state" value="已揭晓" /><label for="mb_male">已揭晓</label>
                    </li>
                    <li class="lileft">
                        <span class="titlename">揭晓时间：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="AnnouncedTime" readonly="true" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">已参与人数：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="AlreadyNumber" readonly="true" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">共需人数：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="AltogetherNumber" readonly="true" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">剩余人数：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="SurplusNumber" readonly="true" value="" />
                    </li>
                </ul>
            </div>
            <div class="editbottom">
                <div class="btn_div">
                    <input type="button" class="edit_btn" id="cancel" value="关 闭" />
                    <input type="button" class="edit_btn" id="save" value="保 存" style="margin-left:50px;" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
<script src="../../control/JS/JSGoodsIssueEdit.js"></script>

