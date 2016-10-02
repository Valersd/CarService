//$(function () {
//    $('.sort').append('<span class="glyphicon glyphicon-sort" aria-hidden="true"></span>');

//    function getParameterByName(name) {
//        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
//        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
//            results = regex.exec(location.search);
//        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
//    };

//    var sortParameter = getParameterByName('sort');

//    switch (sortParameter) {
//        case 'id_desc':
//            $('#number').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet-alt'); break;
//        case 'car':
//            $('#car').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet'); break;
//        case 'car_desc':
//            $('#car').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet-alt'); break;
//        case 'description':
//            $('#description').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet'); break;
//        case 'description_desc':
//            $('#description').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet-alt'); break;
//        case 'created_by':
//            $('#created').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet'); break;
//        case 'created_by_desc':
//            $('#created').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet-alt'); break;
//        case 'mechanic':
//            $('#mechanic').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet'); break;
//        case 'mechanic_desc':
//            $('#mechanic').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet-alt'); break;
//        default:
//            $('#number').children('span').removeClass('glyphicon-sort').addClass('glyphicon-sort-by-alphabet'); break;
//    }
//});