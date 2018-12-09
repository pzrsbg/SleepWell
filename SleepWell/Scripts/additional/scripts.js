$(document).ready(function () {
    $('input').each(function () {
        if ($(this).val())
            $(this).addClass('filled');
    });
    $('textarea').each(function () {
        if ($(this).val())
            $(this).addClass('filled');
    });
    if ($('.isCompanyCheckbox').is(':checked')) {
        $(".isCompanyBox").show();
    } else {
        $(".isCompanyBox").hide();
    }
});

$('input').blur(function () {
    var inputValue = $(this).val;
    if (inputValue === "") {
        $(this).removeClass('filled');
    } else {
        $(this).addClass('filled');
    }
});
$('textarea').blur(function () {
    var inputValue = $(this).val;
    if (inputValue === "") {
        $(this).removeClass('filled');
    } else {
        $(this).addClass('filled');
    }
});

$('.isCompanyCheckbox').click(function () {
    if ($(this).is(':checked')) {
        $(".isCompanyBox").show();
    } else {
        $(".isCompanyBox").hide();
    }
}); 