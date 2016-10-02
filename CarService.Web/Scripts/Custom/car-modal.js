$(function () {
    var id;
    if ($('#CarId').val()) {
        id = $('#CarId').val().trim();
    }
    else {
        id = $('#callModal').data('id');
    }
    var url = '/Ajax/CreateCar';
    if ($('#callModal').text().indexOf('Edit car') >= 0) {
        url = '/Ajax/EditCar';
    }

    $('#callModal').click({ id: id, url: url }, carModalClick);

    if ($('#resetCar').text()) {
        $('#CarRegNumber').autocomplete({
            autofocus: true,
            minLength: 3,
            source: "/Ajax/FindCar",
            select: function (event, ui) {
                event.preventDefault();
                $('#CarId').val(ui.item.value);
                $('#CarRegNumber').val(ui.item.label);
                $('#callModal').text('Edit car');
                $('#myModalLabel').text(ui.item.label);
                $('#callModal').unbind().click({ id: ui.item.value, url: '/Ajax/EditCar' }, carModalClick);
                $('#saveCar').unbind().click({ url: '/Ajax/EditCar' }, saveCar);
                $('#resetCar').css('display', 'inline');
                $.validator.unobtrusive.addValidation('form[action="/RepairDocuments/Create"]');
                $('#CarId').valid();
                $('#CarRegNumber').attr('readonly', 'readonly');
            }
        });

        $('#resetCar').click(function (event) {
            event.preventDefault();
            $(this).css('display', 'none');
            $('#CarId').val('');
            $('#CarRegNumber').val('').removeAttr('readonly');
            $('#callModal').text('Add car').unbind().click({ id: '', url: '/Ajax/CreateCar' }, carModalClick);
            $('#saveCar').unbind().click({ url: '/Ajax/CreateCar' }, saveCar);
            $('#myModalLabel').text('Add car data');
        });
    }

    function carModalClick(event) {
        event.preventDefault();
        event.target.blur();

        $('#messageContainer').slideUp(1000);

        if (event.data.url === '/Ajax/CreateCar') {
            $('#CarId').val('');
            $('#CarRegNumber').val('');
        }

        $.ajax({
            url: event.data.url,
            data: { id: event.data.id },
            success: function (result) {
                $('.modal-body').html('').append(result);
                $.validator.unobtrusive.addValidation('#formCar');
                $('#CarModal').modal('show');
            }
        });
    };

    $('#CarModal').on('hidden.bs.modal', function () {
        $('.modal-body').html('');
    });

    $('#saveCar').click({url:url}, saveCar);

    function saveCar(event) {
        event.preventDefault();
        $(this).blur();

        var $form = $('#formCar');
        if ($form.valid()) {
            $.ajax({
                url: event.data.url,
                method: 'POST',
                data: $form.serialize(),
                success: function (result) {
                    $('#message').removeClass().addClass(result.cssClass).text(result.message);
                    $('#CarModal').modal('hide');
                    window.scrollTo(0, 0);
                    if ((result.cssClass + "").indexOf('success') > 0) {
                        if (result.id) {
                            $('#CarId').val(result.id);
                            $('#CarRegNumber').val(result.number);
                            $('#callModal').text('Edit car').attr('data-id', result.id);
                            $('#myModalLabel').text(result.number);
                            $('#callModal').unbind().click({ id: result.id, url: '/Ajax/EditCar' }, carModalClick);
                            $('#saveCar').unbind().click({ url: '/Ajax/EditCar' }, saveCar);
                            $('#resetCar').css('display', 'inline');
                            $.validator.unobtrusive.addValidation('form[action="/RepairDocuments/Create"]');
                            $('#CarId').valid();
                            $('#CarRegNumber').attr('readonly', 'readonly');
                        }
                        $('#messageContainer').show(1000).delay(1000).hide(1000, hiddenMessage);
                    }
                    else {
                        $('#messageContainer').show(1000);
                    }
                }
            });
        }
    };

    function hiddenMessage() {
        if ($('#carContainer')) {
            $.ajax({
                url: '/Ajax/GetCar',
                data: { id: id },
                method: 'GET',
                success: function (result) {
                    $('.well').first().css('border', '1px solid green');
                    setTimeout(function () {
                        $('#carContainer').html(result);
                        $('#showHideInfo').showHideElement('#carInfo', 'em');
                    }, 500);
                }
            });
        }
    };
});
