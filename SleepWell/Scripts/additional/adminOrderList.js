$(document).ready(function () {
    $('ul.orderListAdmin > li a.more').click(function () {
        $(this).parent().parent().parent().toggleClass('active');
    });

    var deleteReservations = $('.buttons a.delete');
    var acceptReservations = $('.buttons a.confirm');
    var activateReservations = $('.buttons a.finish');
    var editReservations = $('.buttons a.edit');
    var completeReservations = $('.buttons a.receive');

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
    };

    linkButtons(deleteReservations, 'Na pewno chcesz usunąć tę rezerwację?', '#deleteReservation-');
    linkButtons(acceptReservations, 'Zaakceptować rezerwację?', '#acceptReservation-');
    linkButtons(editReservations, 'Na pewno chcesz edytować tę rezerwację?', '#editReservation-');
    linkButtons(activateReservations, 'Rozpocząć pobyt?', '#activateReservation-');
    linkButtons(completeReservations, 'Zakończyć pobyt?', '#completeReservation-');
});