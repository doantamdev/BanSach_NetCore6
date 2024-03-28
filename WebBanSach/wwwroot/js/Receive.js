"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage", function (user, message, Date) {
    //var li = document.createElement("li");
    var ul = document.getElementById("noti-list");

    // Tạo phần tử li
    var li = document.createElement("li");
    li.className = "noti-item";

    // Tạo phần tử p cho tiêu đề
    var title = document.createElement("p");
    title.className = "title";
    title.textContent = "Thông báo mới từ admin đẹp trai!";

    // Tạo phần tử div cho nhóm nội dung
    var contentGroup = document.createElement("div");
    contentGroup.className = "content-group";

    // Tạo phần tử p cho nội dung
    var content = document.createElement("p");
    content.className = "content";
    content.textContent = `${message}`;

    // Tạo phần tử p cho ngày
    var date = document.createElement("p");
    date.className = "date";
    date.textContent = `${Date}`;

    // Thêm phần tử p nội dung và phần tử p ngày vào nhóm nội dung
    contentGroup.appendChild(content);
    contentGroup.appendChild(date);

    // Thêm phần tử p tiêu đề và nhóm nội dung vào phần tử li
    li.appendChild(title);
    li.appendChild(contentGroup);

    //li.textContent = `${user} says ${message}`;
    ul.appendChild(li);
});