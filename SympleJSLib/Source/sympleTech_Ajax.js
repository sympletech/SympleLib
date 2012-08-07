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
        'requiredFields': [],
        'requiredFieldClass': 'field-required',
        'errorMessageTarget': '',
        'fieldErrorClass': 'field-error',
        'beforeSubmit': function (valState) { },
        'postData': $(this).serialize(),
        'action': $(this).attr("action"),
        'onComplete': function (data) { }

    }, options);
    
    var validationState = {
        isValid: true,
        errorMessage: ''
    };
    
    var checkRequiredFields = function (form) {
        for (var i in settings.requiredFields) {
            var rField = $(form).find('*[name="' + settings.requiredFields[i] + '"]');
            if ($(rField).val() == "") {
                validationState.errorMessage += "<li>" + $(rField).attr("name") + " is Required</li>";
                $(rField).addClass(settings.fieldErrorClass);
                validationState.isValid = false;
            } else {
                $(rField).removeClass(settings.fieldErrorClass);
            }
        }
    };

    return this.each(function () {
        for (var i = 0; i < settings.requiredFields.length; i++) {
            $(this).find('*[name="' + settings.requiredFields[i] + '"]').addClass(settings.requiredFieldClass);
        }

        $(this).submit(function (e) {
            e.preventDefault();
            settings.showHideLoaderMethod(true);
            settings.beforeSubmit(validationState);
            checkRequiredFields(this);
            if (validationState.isValid) {
                $.post(settings.action, settings.postData, function (data) {
                    settings.onComplete(data);
                    settings.showHideLoaderMethod(false);
                });
            } else {
                $(settings.errorMessageTarget).html(validationState.errorMessage);
                settings.showHideLoaderMethod(false);
            }
        });
        
    });
};