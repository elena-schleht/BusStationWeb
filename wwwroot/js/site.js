// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function selectTrip(tripId, from, to) {
    $(".alert").remove();

    $("#idTicket").css("display", "block");
    $("#selectedTripId").val(tripId);
    $("#selectedTrip").text(from + ' -> ' + to);
}