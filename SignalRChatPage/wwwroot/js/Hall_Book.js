$(document).ready(function () {
    console.log("文档加载完毕");
});
//按钮事件的监听
$('#cancle').on('click', function (event) {
    console.log('点击取消按钮，返回上一界面！');
    window.location.href = "/Hall/Index";
});
$('#book').on('click', function (event) {
    console.log('点击预定按钮！');
    $('form:first').submit();    
});
