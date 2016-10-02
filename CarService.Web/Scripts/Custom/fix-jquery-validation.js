(function ($) {
    $.validator.setDefaults({
        ignore: []
    });

    $.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".").replace(" ", "");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    };

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    };

    $.validator.addMethod('pricegreaterthan', function (value, element, params) {
        if (!this.optional(element)) {
            Globalize.culture(params.cultureCurrency);
            var allowEquality = params.allowequality;
            var $otherValue = Globalize.parseFloat(Globalize.format($(params.otherpropertyname).val().trim(), 'c'));
            //Globalize.culture(params.culture);
            value = Globalize.parseFloat(Globalize.format(value, 'n2'));
            console.log(value + ' ' + $otherValue);
            if (allowEquality == 'true') {
                return value >= $otherValue;
            }
            return value > $otherValue;
        }
        return true;
    });

    $.validator.unobtrusive.adapters.add("pricegreaterthan", ['otherpropertyname', 'allowequality'], function (options) {
        options.rules['pricegreaterthan'] = {
            otherpropertyname: '#' + options.params.otherpropertyname,
            allowequality: ("" + options.params.allowequality).toLowerCase(),
            cultureCurrency: $('body').data('currency'),
            culture: $('html').prop('lang')
        };
        options.messages["pricegreaterthan"] = options.message;
    });

    $.validator.addMethod('yearrange', function (value, element, params) {
        if (!this.optional(element)) {
            var current = parseInt(value);
            var min = params.min;
            var max = params.max;
            return min <= current && current <= max;
        }
        return true;
    });

    $.validator.unobtrusive.adapters.add('yearrange', ['min', 'max'], function (options) {
        options.rules['yearrange'] = {
            min: options.params.min,
            max: options.params.max
        };
        options.messages['yearrange'] = options.message;
    });

    $.validator.unobtrusive.addValidation = function (selector) {
        //get the relevant form 
        var form = $(selector);
        // delete validator in case someone called form.validate()
        $(form).removeData("validator");
        $.validator.unobtrusive.parse(form);
    };

}(jQuery));
