//--Easy helper for enabling and disabling objects
jQuery.fn.disable = function () {
    return this.each(function () {
        jQuery(this).attr('disabled', 'disabled');
    });
};

jQuery.fn.enable = function () {
    return this.each(function () {
        jQuery(this).removeAttr('disabled');
    });
};

//Set's up a watch to monitor for DOM Mutations
//-- http://james.padolsey.com/javascript/monitoring-dom-properties/
jQuery.fn.watch = function (id, fn) {
    return this.each(function () {
        var self = this;
        var oldVal;
        if (id === 'html') {
            oldVal = jQuery(this).html();
        } else {
            oldVal = self[id];
        }

        $(self).data(
            'watch_timer',
            setInterval(function () {
                var newVal;
                if (id === 'html') {
                    newVal = jQuery(self).html();
                } else {
                    newVal = self[id];
                }
                if (newVal !== oldVal) {
                    fn.call(self, id, oldVal, self[id]);
                    oldVal = newVal;
                }
            }, 100)
        );
    });
    return self;
};

jQuery.fn.unwatch = function (id) {
    return this.each(function () {
        clearInterval($(this).data('watch_timer'));
    });
};