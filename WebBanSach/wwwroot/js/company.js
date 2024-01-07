var dataTable;
$(document).ready(function () {

    loadDatatable();

});

function loadDatatable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/company/getall"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id"
                , "render": function (data) {
                    return `
                         <div>
                         <a href="/Admin/Company/Upsert?id=${data}"
                                class="btn btn-primary">
                                <i class="bi bi-pencil"></i>
                        </a>

                         <a onClick=Delete('/Admin/Company/DeletePost/${data}') class="btn btn-danger">
                           <i class="bi bi-trash3"></i></a>
                        </div>
                    `
                }
                , "width": "15%"
            }
        ]
    });

}

function Delete(url) {
    Swal.fire({
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

                    location.reload(true);

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