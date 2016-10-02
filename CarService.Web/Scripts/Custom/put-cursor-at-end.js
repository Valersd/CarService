jQuery.fn.extend({
    setSelection: function (selectionStart, selectionEnd) {
        if (this.length == 0) return this;
        input = this[0];

        if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', selectionEnd);
            range.moveStart('character', selectionStart);
            range.select();
        } else if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(selectionStart, selectionEnd);
        }

        return this;
    }
})

jQuery.fn.extend({
    setCursorPosition: function (position) {
        if (this.length == 0) return this;
        return $(this).setSelection(position, position);
    }
})

jQuery.fn.extend({
    focusEnd: function () {
        var that = $(this);
        that.setCursorPosition(that.val().length);
        window.setTimeout(function () {
            that.setCursorPosition(that.val().length);
            $(that).scrollTop(9999);
            $(window).scrollTop($(that).offset().top - 300);
        }, 1);

        return that;
    }
});