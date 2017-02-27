$(document).ready(function () {
    var recipes = $('#recipe-table').DataTable({
        "info": false,
        "paging": false,
        "searching": false,
        "order": [[0, "asc"]],
        "columnDefs": [
            {
                "targets": [1,2],
                "orderable": false
            },
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [2],
                "searchable": false,
                "orderable": false
            }
        ]
    });

    $('#recipe-table tbody').on('click', 'tr', function () {
        var data = recipes.row(this).data();
        $.ajax({
            url: '/CookBook/Recipe/',
            type: 'GET',
            dataType: 'html',
            data: { id: data[0] },
            success : function(data){
                $('#recipe-pane').html(data);
            }
        });
    });
});