var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDatatable("inprocess");
    } else {
        if (url.includes("completed")) {
            loadDatatable("completed");
        } else {
            if (url.includes("pending")) {
                loadDatatable("pending");
            } else if (url.includes("approved")) {
                loadDatatable("approved");
            } else {
                loadDatatable("all");
            }
        }
    }

});

function loadDatatable(status) {
    debugger;
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/order/getall?status=" + status
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "25%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "25%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "5%" },
            {
                "data": "id"
                , "render": function (data) {
                    debugger;
                    return `
                         <div>
                             <a href="/Admin/Order/Details?orderId=${data}"
                                    class="btn btn-primary">
                                    <i class="bi bi-pencil"></i>
                             </a>                       
                        </div>
                    `
                }
                , "width": "5%"
            }
        ]
    });

}
