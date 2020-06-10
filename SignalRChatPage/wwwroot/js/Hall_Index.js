$(document).ready(function () {
    InitialPage();
});
//通过ajax进行页面的初始化
var InitialPage = function () {
    $.ajax({
        type: 'GET',
        url: '/Hall/GetInitData',
        dataType: "json",
        success: function (data) {           
            $('#detail_show').empty();
            BuildItemInfo(data);
        },
        error: function (error) {
            console.log(`获得数据出现错误：${error}`);
        }
    });
};

//生成会议详情
function BuildItemInfo(data) {   
    for (let i = 0; i < data.length; i++) {
        let divid = `item_${i}`;
        let htmlstr = BuildHtmlStr(data[i], divid);
        new ClipboardJS('.copy-btn');
        $('#detail_show').append(htmlstr);     
    }
}
function BuildHtmlStr(item, divid) {
    let copydatastr = ` 房间号：${item.roomNo} 主题：${item.theme} 开始时间：${item.startTime} 结束时间：${item.endTime} 房间密码：${item.password}`;   
    let htmlstr = `<div class="item_info" id="${divid}">
                        <span id="text">
                            ${copydatastr}
                        </span>
                        <span>
                            <button type="button" class="btn-secondary " onclick="EditBtnEvent(${item.roomNo})">编辑</button>
                            <button type="button" class="btn-secondary" onclick="DeleteBtnEvent(${item.roomNo},this)">删除</button>
                            <button type="button" class="btn-secondary copy-btn" data-clipboard-text="${copydatastr}" onclick="CopyBtnEvent()">复制</button>
                        </span>
                    </div>`;
    return htmlstr;
}
//按钮事件的监听
$('#joinroom').on('click', function (event) {
    console.log('点击加入房间按钮！');    
});
$('#bookroom').on('click', function (event) {
    console.log('点击预定会议按钮！');
    window.location.href = "/Hall/Book";
});
function CopyBtnEvent() {    
    alert("复制成功");
}
function DeleteBtnEvent(roomNo,dom) {   
    $.ajax({
        type: "GET",
        data: {
            roomno: roomNo
        },
        url:"/Hall/DeleteDataFromMeetInfo",
        dataType: "json",
        success: function (status) {
            console.log('输出返回信息');
            console.log(status);
            if (status.code == 1) {
                $(dom).parent().parent().remove();
            }
            alert(status.codeInfo);
        },
        error: function (e) {
            alert(`请求数据出错，出错信息：${e.responseText}`);
        }
    });
}
function EditBtnEvent(roomno) {

}