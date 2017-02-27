$(document).ready(function () {
    let current = $('#current-ingredients');
    loadCurrent(current);
    loadAdd(current);

    $('#add-form').submit(function (e) {
        e.preventDefault();
        loadCurrent(current);
        $('#add-ingredient').html(e);
    })
});

var loadCurrent = function(current) {
    $.ajax({
        url: '/CookBook/CurrentIngredients/',
        type: 'GET',
        dataType: 'html',
        data: { id: current.data('id') },
        success: function (data) {
            current.html(data);
        }
    });
}

var loadAdd = function(current) {
    $.ajax({
        url: '/CookBook/Add/',
        type: 'GET',
        dataType: 'html',
        data: { id: current.data('id') },
        success: function (data) {
            $('#add-ingredient').html(data);
        }
    });
}