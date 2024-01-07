var dataTable;
$(document).ready(function () {

    loadDatatable();

});

function loadDatatable() {
    debugger;
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    debugger;
                    return `
                         <div>
                         <a href="/Admin/Product/Upsert?id=${data}"
                                class="btn btn-primary">
                                <i class="bi bi-pencil"></i>
                        </a>

                         <a onClick=Delete('/Admin/Product/DeletePost/${data}') class="btn btn-danger">
                           <i class="bi bi-trash3"></i></a>
                        </div>
                    `
                }
                , "width": "15%"
            }
        ]
    });

}

function Delete(url) { //api
    Swal.fire({ //cảnh báo
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {

                    //loadDatatable();
                    debugger;
                    if (data.success) {
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message)
                    }
                }
            })
        }
    })
}