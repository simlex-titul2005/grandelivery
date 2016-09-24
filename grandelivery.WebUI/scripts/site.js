/// <reference path="../bower_components/jquery/dist/jquery.min.js" />

$(function () {
    $('#grid-orders').on('click', '.take-btn', function () {
        var orderId = $(this).closest('tr').attr('data-row-id');
        takeCargo(this, orderId);
        return false;
    });
});

$(function () {
    $('#grid-orders').on('click', '.untake-btn', function () {
        var orderId = $(this).closest('tr').attr('data-row-id');
        untakeCargo(this, orderId);
        return false;
    });
});

function takeCargo(element, orderId)
{
    var $grid = $(element).closest('.sx-gv');
    var page = $grid.find('.sx-gv__pager li.active a').data('page');

    $.ajax({
        method: 'post',
        url: '/orders/takecargo',
        data: { orderId: orderId },
        beforeSend: function () {
            $(element).addClass('fa-spinner').addClass('fa-spin');
        },
        success: function (result) {
            clickGridViewPager($grid, page);
        }
    });
}

function untakeCargo(element, orderId) {
    var $grid = $(element).closest('.sx-gv');
    var page = $grid.find('.sx-gv__pager li.active a').data('page');

    $.ajax({
        method: 'post',
        url: '/orders/untakecargo',
        data: { orderId: orderId },
        beforeSend: function () {
            $(element).addClass('fa-spinner').addClass('fa-spin');
        },
        success: function (result) {
            clickGridViewPager($grid, page);
        }
    });
}