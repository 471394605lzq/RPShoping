<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AfterModuleEdit.aspx.cs" Inherits="RPShoping.Web.After.AfterModuleEdit" %>

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
    <script src="../../jquery/JSAdvQuery.js"></script>
    <title>编辑模块</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="datatb_all">
        <div class="datatb_all_mide">
            <div class="datatb_top">
                <span id="back" class="top_back">
                    <i class="fa fa-reply" aria-hidden="true" style="color:#438eb9; margin-right:5px; font-size:22px;"></i>
                </span>
                <span class="top_title">编辑后台模块</span>
            </div>
            <div class="datatb">
                <ul>
                    <li class="lileft">
                        <span class="titlename">模块key：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="module_key" value="" />
                        <input type="hidden" id="parement_id" value="" />
                    </li>
                    <li class="lileft">
                       <span class="titlename">模块名称：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="module_name" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">模块编号：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="module_code" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">页面名称：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="m_editurl" value="" />
                    </li>
                    <li class="lileft">
                        <span class="titlename">是否为父级：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input id="isparement" type="radio" checked="checked" name="isparemnt" value="1" /><label for="mb_female">是</label>
                        <input id="noparement" type="radio" name="isparemnt" value="0" /><label for="mb_male">否</label>
                    </li>
                    <li class="lileft">
                        <span class="titlename">是否显示：</span><span class="requiredstr">*</span>
                    </li>
                    <li class="liright">
                        <input id="yes" type="radio" checked="checked" name="showstate" value="1" /><label for="mb_female">是</label>
                        <input id="no" type="radio" name="showstate" value="0" /><label for="mb_male">否</label>
                    </li>
                     <li class="lileft">
                        <span class="titlename">菜单图标：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="imagecode" value="" />
                    </li>
                     <li class="lileft">
                        <span class="titlename">序号：</span>
                    </li>
                    <li class="liright">
                        <input type="text" class="inputlb" id="reorder" value="" />
                    </li>
                    <li class="allwidth lileft">
                        <span class="commremark titlename">备注说明</span>
                    </li>
                    <li class="commline">
                        <span class="commlinespan"></span>
                    </li>
                    <li class="commreamrktext">
                        <textarea class="input_area" id="module_remark"></textarea>
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
<script src="../../control/JS/JSAfterModuleEdit.js"></script>

