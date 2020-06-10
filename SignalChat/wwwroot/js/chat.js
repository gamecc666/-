/// <reference path="../lib/signalr/dist/browser/signalr.js" />

"use strict"

//实例画signalR包中HubConnectionBuilder一个对象
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//通信管道未连接成功的时候，发送按钮不可用
document.getElementById("sendButton").disabled = true;

//对通信管道调用客户端方法的监听（针对没有房间的情况下使用）
//connection.on("ReceiveMessage", function (user, message) {
//    //转义字符的处理
//    let date = new Date();
//    let dateStr = `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}  `;
//    let msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt").replace(/>/g, "&gt");
//    let combinemsg = dateStr+ user + " says " + msg;
//    let li = document.createElement("li");
//    li.textContent = combinemsg;
//    document.getElementById("messagesList").appendChild(li);
//});
//统一对房间模式下信息的处理
connection.on("ReceiveMessage", function (msg) {    
    let li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});
//开始通信管道的连接
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    console.log('Connection open ...');
}).catch(function (err) {
    console.log('Connection open Expection');
    return console.error(err.toString())
});

////关闭通信管道
//connection.stop().then(function () {
//    document.getElementById("sendButton").disabled = false;
//    console.log('Connection Close ...');
//}).catch(function (err) {
//    console.log('Connection Close Expection');
//    return console.log.error(err.toString());
//});

//代码重构
const InGroupOPeration = 1;
const OutGroupOperation = 2;
const SendGroupOperation = 3;

function Message(roomno, nick, action, msg = null) {
    this.roomno = roomno;
    this.nick = nick;
    this.action = action;
    this.msg = msg;
};

var InvokeServerMethod = (msg) => {
    let methodName = "HandleClientMsg";    
    connection.invoke(methodName, JSON.stringify(msg)).catch(function (err) {
        return console.error(err.toString());
    });
};



//进入按钮添加点击事件
document.getElementById("goChatRoom").addEventListener("click", function () {
    let roomNo = document.getElementById("txtRoomNo").value;
    let nickName = document.getElementById("userInput").value;
    console.warn('进入房间按钮事件');
    if (roomNo && nickName) {
        console.log('房间号：' + roomNo + ' 昵称：' + nickName);
        document.getElementById("outChatRoom").style.display = "block";
        document.getElementById("outChatRoom").style.display = "";
        this.style.display = "none";

        //connection.invoke("AddToGroup", roomNo, nickName).catch (function(err) {
        //    return console.error(err.toString());
        //});
        let msg = new Message(roomNo, nickName, InGroupOPeration);
        InvokeServerMethod(msg);
    }
    else {
        alert("请检查昵称是否为空！");
    }       
});
//退出按钮添加点击事件
document.getElementById("outChatRoom").addEventListener("click", function () {   
    console.warn('离开房间按钮事件');
    this.style.display = "none";
    document.getElementById("goChatRoom").style.display = "block";
    document.getElementById("goChatRoom").style.display = "";
    let roomNo = document.getElementById("txtRoomNo").value;
    let nickName = document.getElementById("userInput").value;
    //connection.invoke("RemoveFromGroup", roomNo, nickName).catch(function (err) {
    //    return console.err(err.toString());
    //});
    let msg = new Message(roomNo, nickName, OutGroupOperation);
    InvokeServerMethod(msg);
});

//发送按钮点击事件处理
document.getElementById("sendButton").addEventListener("click", function (event) {
    let nickName = document.getElementById("userInput").value;
    let roomNo = document.getElementById("txtRoomNo").value;
    let message = document.getElementById("messageInput").value;
    //connection.invoke("SendMessage", user, message).catch(function (err) {
    //    return console.error(err.toString());
    //});
    //connection.invoke("SendMessageToGroup", roomNo ,nickName, message).catch(function (err) {
    //    return console.error(err.toString());
    //});
    let msg = new Message(roomNo, nickName, SendGroupOperation, message);
    InvokeServerMethod(msg);
    //如果此事件没有被显示处理，它默认的动作是不会照常执行的
    event.preventDefault();
});

