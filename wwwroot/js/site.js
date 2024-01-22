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


$(function () {
    GetTrips();
});

function selectTrip(tripId, from, to, date) {
    $(".alert").remove();

    $("#idTicket").css("display", "block");
    $("#selectedTripId").val(tripId);
    $("#selectedTrip").text(from + ' -> ' + to + ' на ' + date);
}

function GetTrips() {
    $("#trips").html(
        '<div class="d-flex flex-column align-items-center justify-content-center m-2"><div class="row"><div class="spinner-border" role="status" aria-hidden="true"></div></div><div class="row"><span>Загрузка данных...</span></div></div>'
    )

    var selectedFilter = $("#filterData").val();

    $.ajax({
        type: "GET",
        url: "Index?handler=Trips",
        data: { filterDate: selectedFilter },
        success: function (data, textStatus, jqXHR) {
            $("#trips").html(data);
        }
    })
}

function SendBook() {
    $("#sendBook").prop("disabled", true);
    $("#sendBook").html(
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>Подтвердить...'
    );

    $.ajax({
        type: "POST",
        url: "Index?handler=Book",
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            tripId: $("#selectedTripId").val(),
            filterDate: $("#filterData").val()
        },
        success: function (data, textStatus, jqXHR) {
            GetTrips();
            $("#TicketModal").modal('hide');
            AlertShow(data)
        },
        complete: function () {
            $("#sendBook").prop("disabled", false);
            $("#sendBook").html('Подтвердить');
        }
    })
}

function AlertShow(message) {
    $("#AlertModal").on("show.bs.modal", function () {
        $(this).find(".modal-body").html(message);
    });
    $("#AlertModal").modal("show");
}

function ChangeCity(sender) {
    if ($(sender).val() == '0') {
        $('input[name="CityName"]').show().focus();
    } else {
        $('input[name="CityName"]').hide();
    }
}