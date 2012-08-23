$.fn.disable = function () {
    return $(this).each(function () {
        $(this).attr('disabled', 'disabled');
    });
};

$.fn.enable = function () {
    return $(this).each(function () {
        $(this).removeAttr('disabled');
    });
};