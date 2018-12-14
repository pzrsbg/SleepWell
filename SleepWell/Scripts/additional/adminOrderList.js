$(document).ready(function () {
    $('ul.orderListAdmin > li a.more').click(function () {
        $(this).parent().parent().parent().toggleClass('active');
    });

    var deleteOrders = $('.buttons a.delete');
    var confirmOrders = $('.buttons a.confirm');
    var finishOrders = $('.buttons a.finish');
    var editOrders = $('.buttons a.edit');
    var receiveOrders = $('.buttons a.receive');

    var linkButtons = function (buttons, info, formId) {
        if (buttons.length > 0) {
            for (var i = 0; i < buttons.length; i++) {
                buttons[i].addEventListener('click', function (e) {
                    var idToMatch = $(this).attr('data-order-id');
                    e.preventDefault();
                    var result = confirm(info);
                    if (result) {
                        $(formId + idToMatch).submit();
                    }
                });
            }
        }
    }

    linkButtons(deleteOrders, 'Na pewno chcesz usunąć to zlecenie', '#deleteOrder-');
    linkButtons(confirmOrders, 'Na pewno chcesz zaakceptować to zlecenie?', '#confirmOrder-');
    linkButtons(editOrders, 'Na pewno chcesz zaakceptować to zlecenie do naprawy?', '#editOrder-');
    linkButtons(finishOrders, 'Na pewno chcesz zakończyć to zlecenie?', '#finishOrder-');
    linkButtons(receiveOrders, 'Klient odebrał zlecenie?', '#receiveOrder-');
});