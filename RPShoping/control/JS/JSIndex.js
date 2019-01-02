var _appid = getappid();
var loadppkey = getshaappkey();
var vm = new Vue({
    el: '#contentdiv',
    data: {
        producttypelist: [],
        likelist: [],
        isload: true,//标识是否可以发送请求，默认是
        PageSize: 20,
        CurrentPageIndex: 1,
    },
    methods: {
        loadtype: function () {
            $.ajax({
                url: "https://d.apicloud.com/mcm/api/tb_ProductSort/",
                method: "get",
                headers: {
                    "Content-type": "application/json",
                    "X-APICloud-AppId": _appid,
                    "X-APICloud-AppKey": loadppkey
                },
                //data:{"id":_id},
                dataType: "json",
                success: function (data, status, header) {
                    //console.log(data);
                    if (data.length>0)
                    {
                        vm.producttypelist = data;
                    }
                },
                error: function (data, status, header) {

                }
            });
        },
        loadlikelist: function (keyword, keywordval) {
            var filter = {
                limit: vm.PageSize,
                include: "P_IDPointer",
                "where": { "I_State": "进行中" },
                skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
            }
            if (keyword != "" && keyword != null && keyword != undefined) {
                if (keyword == "new") {
                    filter = {
                        limit: vm.PageSize,
                        include: "P_IDPointer",
                        "where": { "I_State": "进行中" },
                        order: "createdAt DESC",
                        skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
                    }
                }
                else if (keyword == "fast") {
                    filter = {
                        limit: vm.PageSize,
                        include: "P_IDPointer",
                        "where": { "I_State": "进行中" },
                        order: "SurplusNumber ASC",
                        skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
                    }
                }
                else if (keyword == "hot") {
                    filter = {
                        limit: vm.PageSize,
                        include: "P_IDPointer",
                        "where": { "I_State": "进行中" },
                        order: "I_IssueNumber DESC",
                        skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
                    }
                }
                else if (keyword == "cost") {
                    filter = {
                        limit: vm.PageSize,
                        include: "P_IDPointer",
                        "where": { "I_State": "进行中" },
                        order: "productprice DESC",
                        skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
                    }
                }
                else if (keyword == "five" || keyword == "ten") {
                    filter = {
                        limit: vm.PageSize,
                        include: "P_IDPointer",
                        "where": { "I_State": "进行中", "P_Type": keywordval },
                        skip: vm.CurrentPageIndex * vm.PageSize - vm.PageSize
                    }
                }
            }
            $.ajax({
                url: "https://d.apicloud.com/mcm/api/tb_Issue?filter=" + encodeURIComponent(JSON.stringify(filter)),
                method: "get",
                headers: {
                    "Content-type": "application/json",
                    "X-APICloud-AppId": _appid,
                    "X-APICloud-AppKey": loadppkey
                },
                //data:{"id":_id},
                dataType: "json",
                success: function (data, status, header) {
                    if (data.length > 0) {
                        vm.isload = true;
                        if (vm.CurrentPageIndex == 1) {
                            vm.likelist = [];
                        }
                        for (var i = 0; i < data.length; i++) {
                            // alert(JSON.stringify(ret[i].P_ID));
                            //  alert(JSON.stringify(ret[i].P_ID.point));
                            var temptype = data[i].P_ID.P_Type;
                            if (temptype == "5") {
                                data[i]["ptype"] = "五元商品";
                            }
                            else if (temptype == "10") {
                                data[i]["ptype"] = "十元商品";
                            }
                            data[i]["styleObject"] = 'width : ' + (data[i].AlreadyNumber / data[i].P_ID.P_Price) * 100 + '%;' + 'background : #ff6600'
                            var price = data[i].P_ID.P_Price;
                            var countnumber = parseInt(price) / parseInt(temptype);
                            data[i]["countnumber"] = countnumber;
                            var resultret = data[i];
                            vm.likelist.push(resultret);
                        }
                        vm.CurrentPageIndex += 1;
                        //vm.likelist = data;
                        $("#loadmore").css("display", "block");
                        $("#loadmoreno").css("display", "none");
                    }
                    else {
                        $("#loadmore").css("display", "none");
                        $("#loadmoreno").css("display","block");
                    }
                },
                error: function (data, status, header) {

                }
            });
        }
    }
});
vm.loadtype();
vm.loadlikelist("fast", "");
//选择类型
function selectitem(th) {
    var id = th.id;
    var items = $("#typeitem .selectitem");
    $("#typeitem .selectitem").removeClass("itemaddcolor");
    $(th).addClass("itemaddcolor");
    vm.likelist = [];
    vm.isload = false;
    vm.CurrentPageIndex = 1;
    vm.keyword = id;
    var keywordval = "";
    if (id == "five") {
        keywordval = "5";
    }
    else if (id == "ten") {
        keywordval = "10";
    }
    else {
        keywordval = "";
    }
    vm.keywordval = keywordval;
    vm.loadlikelist(id, keywordval);
}
function loadmore() {
    //vm.isload = false;
    //vm.loadlikelist(vm.keyword, vm.keywordval);
    window.location.href = "goodslist.aspx?id=" + "590f356585d63a837adc13a2";
}
//跳转商品列表
function togoodslist(th) {
    window.location.href = "goodslist.aspx?id=" + th.id;
}