var color = [
    '#ff6384',
    '#d62728',
    '#36a2eb',
    '#cc65fe',
    '#ffce56',
    '#9bd0f5'
];

$(function () {
    $('input[name="datefilter"]').daterangepicker({
        autoUpdateInput: true,
        startDate: moment().startOf('year'),
        endDate: moment().endOf('year'),
        locale: {
            format: 'DD.MM.YYYY',
            cancelLabel: 'Clear'
        }
    });

    $('input[name="datefilter"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD.MM.YYYY') + ' - ' + picker.endDate.format('DD.MM.YYYY'));

        BuildCharts(picker.startDate, picker.endDate)
    });

    $('input[name="datefilter"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });

    var startDate = $('input[name="datefilter"]').data('daterangepicker').startDate;
    var endDate = $('input[name="datefilter"]').data('daterangepicker').endDate;

    BuildCharts(startDate, endDate)
});

function BuildCharts(startDate, endDate) {
    BuildTicketsByMonth(startDate, endDate);
    BuildTicketsByRoute(startDate, endDate);
}

async function BuildTicketsByMonth(startDate, endDate) {
    SplashScreen("#chartsTicketsByMonth")

    var response = await ajax_chart('/Admin/AdminIndex?handler=TicketsByMonth', { dtStart: startDate.format('YYYY-MM-DD'), dtEnd: endDate.format('YYYY-MM-DD') });

    var labels = response.map(function (e) {
        return e.date;
    });
    var data = response.map(function (e) {
        return e.count;
    });

    $("#chartsTicketsByMonth").html('<canvas></canvas>');

    var chart = new Chart($("#chartsTicketsByMonth").children("canvas"), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Продажи билетов по месецам',
                data: data,
                borderWidth: 1,
                backgroundColor: color
            }]
        }
    });
}

async function BuildTicketsByRoute(startDate, endDate) {
    SplashScreen("#chartsTicketsByRoute")

    var response = await ajax_chart('/Admin/AdminIndex?handler=TicketsByRoute', { dtStart: startDate.format('YYYY-MM-DD'), dtEnd: endDate.format('YYYY-MM-DD') });

    var labels = response.map(function (e) {
        return e.date;
    });
    var data = response.map(function (e) {
        return e.count;
    });

    $("#chartsTicketsByRoute").html('<canvas></canvas>');

    var chart = new Chart($("#chartsTicketsByRoute").children("canvas"), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Продажи билетов по маршрутам',
                data: data,
                borderWidth: 1,
                backgroundColor: color
            }]
        }
    });
}

async function ajax_chart(url, data) {
    var data = data || {};

    return await $.getJSON(url, data);
}

function SplashScreen(element) {
    $(element).html(
        '<div class="d-flex flex-column align-items-center justify-content-center m-2"><div class="row"><div class="spinner-border" role="status" aria-hidden="true"></div></div><div class="row"><span>Загрузка данных...</span></div></div>'
    );
}