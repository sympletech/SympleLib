/***********************************************************
        Ajax Link / AJAX Form
************************************************************/

$.fn.sympleTech_ajaxlink = function (options) {

    var settings = $.extend({
        'httpMethod': 'GET',
        'showHideLoaderMethod': function (show) {},
        'onComplete': function (data) {}
    }, options);

    return this.each(function () {
        var href = $(this).attr('href');

        $(this).click(function (e) {
            e.preventDefault();
            settings.showHideLoaderMethod(true);
            $.ajax({
                url: href,
                type: settings.httpMethod,
                success: function (data) {
                    settings.onComplete(data);
                    settings.showHideLoaderMethod(false);
                }
            });
        });
    });
};

$.fn.sympleTech_ajaxform = function (options) {
    
    var settings = $.extend({
        'showHideLoaderMethod': function (show) { },
        'onComplete': function (data) { }
    }, options);

    return this.each(function () {
        $(this).submit(function (e) {
            e.preventDefault();
            settings.showHideLoaderMethod(true);

            var href = $(this).attr('action');
            var postData = $(this).serialize();

            $.post(href, postData, function (data) {
                settings.onComplete(data);
                settings.showHideLoaderMethod(false);
            });
        });
    });
};