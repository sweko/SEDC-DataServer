$(function() {
    $.get("/listTables", function (response) {
        $.each(response, function(key, value) {
            $('#tables')
                .append($('<option>', { value: value })
                    .text(value));
        });
    });

    $("#display-data").click(displayData);
})

function displayData() {
    var tableName = $('#tables').val();
    $.get("/table/" + tableName, function(response) {

    });
}