$.extend({
    contextMenuInit: function(gridtbl) {
        //将页面菜单设置放到此处
        var option = {
            //重写onContextMenu和onShowMenu事件
            onContextMenu: function(e) { //显示菜单
                var rowId = $(e.target).closest("tr.jqgrow").attr("id"); //获得RowID 
                if ($(e.target).attr("id") == "dontShow") return false;
                else
                return true;
            },
            onShowMenu: function(e, menu) { //显示菜单
                return menu;
            },
            menuStyle: { //菜单样式
                backgroundColor: '#fcfdfd',
                border: '2px solid #a6c9e2',
                //maxWidth: '600px',
                width: '170px', // to have good width of the menu 
                padding:'1px 1px 5px 1px'
            },
            itemHoverStyle: { //点击菜单上面的样式
                border: '1px solid #79b7e7',
                color: '#1d5987',
                backgroundColor: '#d0e5f5'
            },
            shadow: false
        };

        $('<div class="contextMenu" id="myMenu1"></div>').hide().appendTo('body'); //在页面增加div
        $("#" + gridtbl).contextMenu("myMenu1", option);
    },

    //操作栏字符串拼接,rowid 行id ， actName 操作列名
    contextMenuRowId: function(_option) {
        var act = $("#" + _option.gridtbl).jqGrid("getCell", _option.rowid, _option.actName);
        var act2 = $("#" + _option.gridtbl).jqGrid("getCell", _option.rowid, _option.actName);
        var myDiv = '<ul class="ui-corner-all"><span style="display:block;padding-left:5px;margin-bottom:5px;" class="ui-widget-header ui-corner-top">操作快捷方式</span>';
        var count=0;//统计次数
         while ( act2.indexOf("</a>") > 0) {
        	 act2 = act2.substring(act2.indexOf("</a>") + 5, act2.length - 1);
            count++;
        }
        var count1 = 0;//统计次数
        while ( act.indexOf("</a>") > 0) {
        	count1++;
        	var hr="";//横线
        	if(count1!=count){ 
        		hr="<span style='padding:0px;margin:1px auto;display:block;clean:both;height:1px;width:165px;border-top:1px dotted #B6C0C9;'></span>";
        	}
        	var str = act.substring(act.indexOf("<a"), act.indexOf("</a>") + 4);
        	str = str.replace("<a","<a style='display:block;float:left;padding-left:10px;width:100%;'");
            myDiv = myDiv + '<li style="height:15px;line-height:15px;margin:3px 0"><span style="font-size:10px;">' + str + '</span></li>'+hr;
            act = act.substring(act.indexOf("</a>") + 5, act.length - 1);
        }
        myDiv = myDiv + '</ul>';
        $("#myMenu1").html(myDiv);
    }
});