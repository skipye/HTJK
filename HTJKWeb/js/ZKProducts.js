

function loadZKProductsList() {

    var pageNum = parseInt($("#ZKProductslist").attr("rel"));
    var totalPage = parseInt($("#ZKProductslist").attr("ref"));

  
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        if (!loadFlg) return false;
        if (pageNum > totalPage) {
            $("#loadaimbox").hide();
            $("#myorde_nocart").css({ display: "block" });
            return;
        }
        loadFlg = false;
        $.ajax({
            type: "post",
            data: { "PageSize":10, "PageIndex": pageNum },
            url: "/Home/ZKProductsList",
            success: function (data) {
                totalPage > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                    $("#loadaimbox").hide();
                } else {
                    $('#ZKProductslist').append(data);
                    pageNum = pageNum + 1;
                    $("#ZKProductslist").attr("rel", pageNum);
                    loadFlg = true;
                }
            }
        });
    }
}
function loadTJProductsList() {

    var pageNum = parseInt($("#myOrderlist").attr("rel"));
    var totalPage = parseInt($("#myOrderlist").attr("ref"));
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    $("#loadaimbox").show();
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
       
        if (pageNum > totalPage) {
            $("#loadaimbox").hide();
            $("#myorde_nocart").css({ display: "block" }); 
            return;
        }
        $.ajax({
            type: "post",
            data: { "PageSize":10, "PageIndex": pageNum },
            url: "/Home/TJProductsList",
            success: function (data) {
                totalPage > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                    $("#loadaimbox").hide();
                } else {
                    $("#myOrderlist").append(data);
                    pageNum = pageNum + 1;
                    $("#myOrderlist").attr("rel", pageNum);
                }
            }
        });
    }
}

function loadPMProductsList() {

    var pageNum = parseInt($("#PMProductslist").attr("rel"));
    var totalPage = parseInt($("#PMProductslist").attr("ref"));
    var loadFlg = true;
    // 取消之前绑定的滚动事件，载入数据时重新绑定
    $(window).off("scroll");
    $(window).dropload({ afterDatafun: lowadData });
    function lowadData() {
        if (!loadFlg) return false;
        if (pageNum > totalPage) {
            $("#loadaimbox").hide();
            $("#myorde_nocart").css({ display: "block" });
            return;
        }
        loadFlg = false;
        $.ajax({
            type: "post",
            data: { "PageSize":10, "PageIndex": pageNum },
            url: "/PaiMai/PMProductsList",
            success: function (data) {
                totalPage > 1 ? $("#loadaimbox").show() : $("#loadaimbox").hide();
                if ($.trim(data) == "") {
                    $("#myorde_nocart").css({ display: "block" });
                    $("#loadaimbox").hide();
                } else {
                    $('#PMProductslist').append(data);
                    pageNum = pageNum + 1;
                    $("#PMProductslist").attr("rel", pageNum);
                    loadFlg = true;
                }
            }
        });
    }
}