
$(function () {
    var culture = $('html').prop('lang');
    var currencyCulture = $('body').data('currency');
    Globalize.culture(culture);

    $('#showHideDescription').showHideElement('.description', 'em');
    $('#showHideCaregoriesAndParts').showHideElement('.categories-parts-container', 'em');

    $('.categories-parts-container').on('click', '.categoryButton', function (event) {
        event.preventDefault();
        var $target = $(event.target);
        $target.blur();

        $('.parts-container').slideUp(500);
        $('.categoryButton').css('background-color', '');
        $target.css('background-color', 'green');

        var catId = $target.attr('id');
        $.ajax({
            url: '/Ajax/GetCategoryParts',
            data: { catId: catId },
            success: function (parts) {
                $('.parts-container').slideUp(500, function () {
                    $('.parts-container').html(parts);
                });
            },
            complete: function (parts) {
                $('.parts-container').slideDown(500);
            }
        });
    });

    $('.parts-container').on('click', '.addPart', function (event) {
        event.preventDefault();
        var $target = $(event.target);
        $target.blur();

        var id, catalogNumber, name, price, partForAdding, usedPart;
        var $row = $($target.parent().parent());
        $row.css('background-color', 'green');
        $target.css('background-color', 'green');
        setTimeout(function () {
            $row.css('background-color', '');
            $target.css('background-color', '');

            id = $row.data('id');

            catalogNumber = $row.children('.catalogNumber').first().text();
            name = $row.children('.partName').first().text();
            Globalize.culture(currencyCulture)
            price = Globalize.parseFloat($row.children('.price').first().text());
            usedPart = $('#showUsedParts tr[data-id=' + id + ']');

            if (usedPart.length) {
                var qty = parseInt(usedPart.children('td:eq(2)').text());
                qty += 1;
                var qtyTd = usedPart.children('td:eq(2)');
                var qtyInput = $(qtyTd).children('input')[0];
                $(qtyInput).val(qty);
                qtyTd.text(qty).append(qtyInput);
                var currentPrice = Globalize.parseFloat(Globalize.format(usedPart.children('td:eq(3)').text().trim(), 'c'));
                var priceInput = $(usedPart.children('td:eq(3)').children('input')[0]);
                currentPrice += price;
                usedPart.children('td:eq(3)').text(Globalize.format(currentPrice, 'c')).append(priceInput.val(Globalize.format(currentPrice, 'n2')));

            }
            else {
                var count = $('#showUsedParts tr').length - 1;
                partForAdding = '<tr data-id="' + id + '" class="text-muted">' +
                    '<td>' + $row.children('td:eq(0)').text().trim() +
                    '<input type="hidden" name="UsedParts[' + count + '].Id" value="' + id + '" />' + '</td>' +
                    '<td>' + $row.children('td:eq(1)').text().trim() + '</td>' +
                    '<td class="text-center">1<input type="hidden" name="UsedParts[' + count + '].Quantity" value="1" /></td>' + '<td>' + Globalize.format(price, 'c') +
                    '<input type="hidden" name="UsedParts[' + count + '].Id" value="' + Globalize.format(price, 'n2') + '" />' + '</td>' +
                    '<td class="text-center"><button class="removePart btn btn-sm btn-default">Remove</button></td>';
                $('#showUsedParts').append(partForAdding);
            }
            
            var partsPriceString = $('#PartsPrice').val().trim();
            var partsPrice = Globalize.parseFloat(Globalize.format(partsPriceString, 'c')) + price;
            $('#PartsPrice').val(Globalize.format(partsPrice, 'c'));
            if ($('#TotalPrice').val()) {
                console.log($('#TotalPrice').val());
                //Globalize.culture(culture);
                var totalPriceCurrentString = $('#TotalPrice').val().trim();
                var totalPrice = Globalize.parseFloat(totalPriceCurrentString);
                totalPrice += price;
                $('#TotalPrice').val('').val(Globalize.format(totalPrice, 'n2'));
                //Globalize.culture(currencyCulture);
            }
        }, 200);
    });

    $('#showUsedParts').on('click', '.removePart', function (event) {
        event.preventDefault();
        var $target = $(event.target);
        $target.blur();

        var $row = $($target.parent().parent());
        $row.css('background-color', 'red');
        $target.css('background-color', 'red');
        setTimeout(function () {
            $row.css('background-color', '');
            $target.css('background-color', '');
            var qtyTd = $($row.children('td:eq(2)'));
            var qty = parseInt(qtyTd.text());
            Globalize.culture(currencyCulture)
            var currentPrice = Globalize.parseFloat($row.children('td:eq(3)').text().trim());
            var price = currentPrice / qty;
            if (qty === 1) {
                $row.hide(200, function () {
                    $row.remove();
                    applyIndexes(['Id', 'Quantity', 'Price']);
                });
            }
            else {
                currentPrice -= price;
                qty -= 1;
                var qtyInput = $(qtyTd.children('input')[0]);
                var priceInput = $($row.children('td:eq(3)').children('input')[0]);
                qtyTd.text(qty).append(qtyInput.val(qty));
                $($row.children('td:eq(3)').text(Globalize.format(currentPrice, 'c')).append(priceInput.val(Globalize.format(currentPrice, 'n2'))));

            }
            var partsPriceElement = $('#PartsPrice');
            var partsPrice = Globalize.parseFloat(partsPriceElement.val());
            partsPrice -= price;
            partsPriceElement.val(Globalize.format(partsPrice, 'c'));
            var totalPriceElement = $('#TotalPrice');
            if (totalPriceElement.val()) {
                //Globalize.culture(culture);
                var totalPrice = Globalize.parseFloat(totalPriceElement.val());
                totalPrice -= price;
                if (totalPrice > 0) {
                    totalPriceElement.val(Globalize.format(totalPrice, 'n2'));
                }
                else {
                    totalPriceElement.val('');
                }
            }
        }, 200);
    });

    function applyIndexes(array) {
        var trs = $('#showUsedParts tr').not(':first');
        trs.each(function (index, element) {
            var inputs = $(element).children().children('input');
            inputs.each(function (secIndex, secElement) {
                $(secElement).attr('name', 'UsedParts[' + index + '].' + array[secIndex]);
            });
        });
    };
});