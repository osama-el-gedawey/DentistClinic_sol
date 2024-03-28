// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.




$(document).ready(function () {


    //Configure FlatePicker
    $(".patient-birthdate").flatpickr({
        dateFormat: "Y-m-d",
        maxDate: "today"
    });


    //Handle Select2
    $('#Input_Gender').on('select2:select', function (e) {
        let select = $(this);
        $('form').not('#signout').validate().element('#' + select.attr('id'));
    });


});


