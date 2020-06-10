$(document).ready(function () {
    console.log("文件结构加载完毕！");    
});

//界面数据初始化

//F5刷新时事件的监听
window.addEventListener("beforeunload", (event) => {
    event.preventDefault();
    event.returnValue = "";
});

//状态初始化
var clickobjid = "";
$("#interaction").css("display", "none");
$(".shadow").css("display", "none");
//弹窗按钮事件处理
$('#close_messagebox').on("click", function () {
    //console.log("点击关闭弹窗按钮");
    $("#interaction").css("display", "none");

    $(".shadow").css("display", "none");
});
$('#ok_messagebox').on('click', function () {
    //console.log('点击了ok按钮');
    let str = $("#pastecontent").val().toString();
    //console.log(`输出textcontent内容：${str}`);
    //$('#testin').val(str);        
    $(`#${clickobjid}`).text(str);
    $("#interaction").css("display", "none");

    $(".shadow").css("display", "none");
});
//点击显示弹窗
function showMessageBox(event) {
    //console.log('点击了textare便签，开始弹出信息输入框||' + event.target.id);
    //console.log(event);
    clickobjid = event.target.id;
    let strs = `#${event.target.id}`;
    let str = $(strs).val().toString();
    //console.log('输出值：' + str);
    $("#pastecontent").val(str);
    $("#interaction").css("display", "");

    $(".shadow").css("display", "");
}

//删除对应textarea
function buttonEvent(node) {
    //console.log("----触发了删除事件！");
    //console.log(node);
    $(node)[0].parentNode.remove();
}

//未分类增加便签的事件处理
var noclassadd_clickcount = 0;
$('#noclassified_add').on('click', function (event) {
    noclassadd_clickcount++;
    var pasteNode = `<div class='item'><input type='image' src='/images/delete_pic.jpg' id='delete' name='name' value='' onclick='buttonEvent(this)'/><textarea class='textarea_unclass' id='textarea_unclass_${noclassadd_clickcount}' onclick='showMessageBox(event)' readonly='readonly'></textarea></div>`;
    console.log('点击了增加按钮');
    $('#noclassified_add').before(pasteNode);
});
//分类增加便签的事件处理
var classadd_clickcount = 0;
$('#classified_add').on('click', function (event) {
    classadd_clickcount++;
    var pasteNode = `<div class='item'><input type='image' src='/images/delete_pic.jpg' id='delete' name='name' value='' onclick='buttonEvent(this)'/><textarea class='textarea_class' id='textarea_class_${classadd_clickcount}' onclick='showMessageBox(event)' readonly='readonly'></textarea></div>`;
    console.log('点击了增加按钮');
    $('#classified_add').before(pasteNode);
});

