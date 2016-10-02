jQuery.fn.extend({
    showHideElement: function (targetElementSelector, childrenSelector) {
        var $caller = $(this);
        $caller.click(function (event) {
            event.preventDefault;
            var children = $caller.children(childrenSelector);
            $(targetElementSelector).slideToggle('slow', function () {
                children.toggle();
            });
        });
    }
});