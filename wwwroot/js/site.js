// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// registering service worker

//if ('serviceWorker' in navigator) {
//    navigator
//    navigator.serviceWorker.register('serviceworker.js').then(function (res) {
//        console.log('Registration Successful');
//    });
//} else {
//    console.log("Service Worker Registration fails")
//}

function selectTrip(tripId, from, to, date) {
    $(".alert").remove();

    $("#idTicket").css("display", "block");
    $("#selectedTripId").val(tripId);
    $("#selectedTrip").text(from + ' -> ' + to + ' на ' + date);
}

function GetData() {
    var selectedFilter = $("#filterData").val();

    $.ajax({
        type: "GET",
        url: "Index?handler=Filter",
        data: { filterDate: selectedFilter },
        success: function (data, textStatus, jqXHR) {
            $('#trips').html(data);
        }
    })
}