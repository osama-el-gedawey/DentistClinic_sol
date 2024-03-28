//constants
let table;
let datatable;
let datatablePayments;
let datatablePlans;
let datatableAppoinments;
let exportColumns = [];
let updatedRaw = undefined;
let documentations = [];
let updatedBox = undefined;

//document ready
$(document).ready(function () {

    //handle bootstrap modal
    $("body").delegate(".js-render-modal", "click", function () {


        const renderModelBtn = $(this);

        const modal = $("#Modal");

        updatedRaw = undefined;
        updatedBox = undefined;
        //set modal title
        modal.find("#ModalLabel").text(renderModelBtn.data("title"));

        if (renderModelBtn.data("box") != undefined) {

            if (renderModelBtn.data("update") !== undefined) {
                updatedRaw = renderModelBtn.closest(".medical-report");
            }

            //set modal body (ajax call)
            $.get({

                url: renderModelBtn.data("url"),
                success: function (result) {

                    modal.find(".modal-body").html(result);


                    if ($('.js-select2').length) {

                        $('.js-select2').select2({
                            dropdownParent: $('#Modal'),
                            tags: true,
                        });

                    }

                    if ($(".report-sdate").length) {
                        //handle flatepicker
                        $(".report-sdate").flatpickr({
                            altInput: true,
                            altFormat: "F j, Y",
                            maxDate: "today"
                        });
                    }

                    if ($(".report-edate").length) {

                        $(".report-edate").flatpickr({
                            altInput: true,
                            altFormat: "F j, Y",
                            maxDate: "today"
                        });

                    }
                   
                    if ($(".form-upload").length) {
                        uploaddocumentationsDragAndDrop();
                    }

                    $.validator.unobtrusive.parse(modal);

                    //show modal
                    modal.modal("show");
                },
                error: function () {
                    console.log("Error");
                }

            });
        }
        else {

            if (renderModelBtn.data("update") !== undefined) {
                updatedRaw = renderModelBtn.closest("tr");
            }

            //set modal body (ajax call)
            $.get({

                url: renderModelBtn.data("url"),
                success: function (result) {

                    modal.find(".modal-body").html(result);

                    if ($('.js-select2').length) {

                        $('.js-select2').select2({
                            dropdownParent: $('#Modal'),
                            tags: true,
                        });

                    }

                    if ($(".payment-date").length) {
                        //libraries handling
                        $(".payment-date").flatpickr({
                            altInput: true,
                            altFormat: "F j, Y",
                            defaultDate: "today",
                        });
                    }

                    if ($(".tplan-sdate").length) {

                        if (updatedRaw != null) {
                            $(".tplan-sdate").flatpickr({
                                altInput: true,
                                altFormat: "F j, Y",
                            });
                        }
                        else {
                            $(".tplan-sdate").flatpickr({
                                altInput: true,
                                altFormat: "F j, Y",
                                defaultDate: "today"
                            });
                        }


                    }

                    if ($(".tplan-edate").length) {
                        if (updatedRaw != null) {

                            $(".tplan-edate").flatpickr({
                                altInput: true,
                                altFormat: "F j, Y",

                            });
                        }
                        else {

                            $(".tplan-edate").flatpickr({
                                altInput: true,
                                altFormat: "F j, Y",
                                defaultDate: "today"
                            });
                        }
                    }



                    $.validator.unobtrusive.parse(modal);

                    //show modal
                    modal.modal("show");
                },
                error: function () {
                    console.log("Error");
                }

            });

            
        }

    });
    //hande toggle status
    $("body").delegate(".js-toggle-status", "click", function () {

        var toggleBtn = $(this);
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure that you need to toggle this item status?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, iam sure!',
            customClass: {
                confirmButton: "btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary",
                cancelButton: "btn btn-outline btn-outline-dashed btn-outline-danger btn-active-light-danger",
            }
        }).then((result) => {
            if (result.isConfirmed) {
                //call ajax
                $.post({

                    url: toggleBtn.data("url"),

                    data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

                    success: function (response) {
                        let vmodel = response;
                        let targetRaw = toggleBtn.parents("tr");

                        targetRaw.find(".js-status").text((vmodel.isDeleted) ? "Deleted" : "Available")
                            .toggleClass("badge-light-success badge-light-danger");
                        //this method created  in site.js
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: "Successfully",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            targetRaw.addClass('animate__animated animate__bounce');
                            setTimeout(() => { targetRaw.removeClass('animate__animated animate__bounce') }, 3000);
                        });
                    },
                    error: function () {
                        //this method created  in site.js
                        Swal.fire({
                            text: `${response.responseText}`,
                            icon: "warning",
                            buttonsStyling: false,
                            confirmButtonText: "Ok, got it!",
                            customClass: {
                                confirmButton: "btn btn-primary",
                            }
                        }).then(() => {
                            targetRaw.addClass('animate__animated animate__bounce');
                            setTimeout(() => { targetRaw.removeClass('animate__animated animate__bounce') }, 3000);
                        });
                    }
                });
            }
        });

    });


    customLightBox();
});


//DataTables
var KTDatatables = function () {
    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": true,
            "pageLength": 5,
            order: [[1, 'asc']],
            lengthMenu: [5, 10, 15, 25, 50, 75, 100],
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            columnDefs: [
                {
                    target: [-1],
                    searchable: false,
                    orderable: false
                }
            ],
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $(".js-datatables").data("document-title");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]:not(.filterSearchForAvailable)');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
var KTDatatablesPlans = function () {
    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatablePlans = $(table).DataTable({
            "info": true,
            "pageLength": 5,
            order: [[1, 'asc']],
            lengthMenu: [5, 10, 15, 25, 50, 75, 100],
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            columnDefs: [
                {
                    target: [-1],
                    searchable: false,
                    orderable: false
                }
            ],
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $(".js-datatables").data("document-title");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1 , 2 , 3 , 4]
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('.js-search-plans');
        filterSearch.addEventListener('keyup', function (e) {
            datatablePlans.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatable-plans');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
var KTDatatablesAppointments = function () {
    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatableAppointments = $(table).DataTable({
            "info": true,
            "pageLength": 5,
            order: [[1, 'asc']],
            lengthMenu: [5, 10, 15, 25, 50, 75, 100],
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            columnDefs: [
                {
                    target: [-1],
                    searchable: false,
                    orderable: false
                }
            ],
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $(".js-datatables").data("document-title");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('.js-search-appointments');
        filterSearch.addEventListener('keyup', function (e) {
            datatableAppointments.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatable-appointments');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
var KTDatatablesPayments = function () {
    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatablePayments = $(table).DataTable({
            "info": true,
            "pageLength": 5,
            order: [[0, 'asc']],
            autoWidth: false,
            lengthMenu: [5, 10, 15, 25, 50, 75, 100],
            'drawCallback': function () {
                KTMenu.createInstances();
            },
            columnDefs: [
                { width: '25%', targets: 0 },
                {
                    target: [-1],
                    searchable: false,
                    orderable: false
                }
            ],
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $(".js-datatables").data("document-title");
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('.js-search-payments');
        filterSearch.addEventListener('keyup', function (e) {
            datatablePayments.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatable-payments');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();
KTUtil.onDOMContentLoaded(function () {

    KTDatatables.init();
    KTDatatablesPlans.init();
    KTDatatablesAppointments.init();
    KTDatatablesPayments.init();
});

$(".tab-plans-datatable").on("click", function () {

    datatable = datatablePlans;

});

$(".tab-appointments-datatable").on("click", function () {

    datatable = datatableAppoinments;

});
$(".tab-payments-datatable").on("click", function () {

    datatable = datatablePayments;

});

//functionalites
//functionalites
const uploaddocumentationsDragAndDrop = () => {
    const browse = document.querySelector(".select");
    const input = document.querySelector(".form-upload input");

    browse.addEventListener("click", () => { //to make input open from browse text

        input.click();

    });


    //input change event
    input.addEventListener("change", () => {

        documentations = [];
        const imgs = Array.from(input.files);

        imgs.forEach((img) => { //add documentations from input in documentations array after chcek no consistency and length no more 5

            if (documentations.every((e) => { return e.name !== img.name })) {
                documentations.push(img);
            }


        });
        showdocumentations(documentations); //display in image container

    });


    //display selected documentations in container
    const showdocumentations = (documentations) => {
        let cartona = "";

        documentations.forEach((img, index) => {

            cartona +=
                `
            <div class="image" style="margin-right:5px">
                <img src=${URL.createObjectURL(img)} alt="image">
                <span class="deleteImg" data-index=${index}>&times;</span>
            </div>
            `

        });

        document.querySelector(".image-container").innerHTML = cartona;
    }


    document.addEventListener("click", (event) => {
        if (event.target.classList.contains("deleteImg")) {
            removeFileFromFileList(event.target.dataset.index, 1);
            documentations = [];
            const imgs = Array.from(input.files);

            imgs.forEach((img) => { //add documentations from input in documentations array after chcek no consistency and length no more 5

                if (documentations.every((e) => { return e.name !== img.name })) {
                    documentations.push(img);
                }


            });
            showdocumentations(documentations); //display in image container
        }
    });


    function removeFileFromFileList(index) {
        const dt = new DataTransfer()
        const { files } = input

        for (let i = 0; i < files.length; i++) {
            const file = files[i]
            if (parseInt(index) !== i)
                dt.items.add(file) // here you exclude the file. thus removing it.
        }

        input.files = dt.files // Assign the updates list
    }

}
function customLightBox() {
    //handle lightcustom
    let imgIndex = 0;
    let documentationList = [];
    $("body").delegate(".gallery-img-link", "click", function (event) {
        event.preventDefault();
        var card = $(this).data("card");
        documentationList = $(`.documentation-img.${card}`);

        imgIndex = $(this).data("index");
        $(".image-center").attr("src", documentationList[imgIndex].src);

        $(".lightBox-overlay").removeClass("d-none");
        $(".lightBox-overlay").addClass("d-flex");
        $(".lightBox-overlay").css("z-index", 1000000000);
    });

    $("body").delegate(".lightBox-overlay", "click", function () {

        $(".lightBox-overlay").removeClass("d-flex");
        $(".lightBox-overlay").addClass("d-none");

    });

    $("body").delegate(".lightbox-arrow-left", "click", function (event) {

        event.stopPropagation();
        imgIndex--;
        if (imgIndex < 0) {
            imgIndex = documentationList.length - 1;
            $(".image-center").attr("src", documentationList[imgIndex].src);
        }
        else {
            $(".image-center").attr("src", documentationList[imgIndex].src);
        }

    });

    $("body").delegate(".lightbox-arrow-right", "click", function (event) {

        event.stopPropagation();

        imgIndex++;

        if (imgIndex > (documentationList.length - 1)) {
            imgIndex = 0;
            $(".image-center").attr("src", documentationList[imgIndex].src);
        }
        else {
            $(".image-center").attr("src", documentationList[imgIndex].src);
        }

    });
}
//libraries handling
$('.js-signout').on('click', function (event) {
    $('#signout').submit();
});

function disableBtnSubmit() {
    $("body :submit").attr("disabled", "disabled").attr("data-kt-indicator", "on");
}
function onRequestBegin() {
    disableBtnSubmit();
};
function onRequestSuccess(response) {
    let modal = $("#Modal");
    modal.modal("hide");

    Swal.fire({
        position: "center",
        icon: "success",
        title: "Successfully",
        showConfirmButton: false,
        timer: 1500
    }).then(() => {
        var itemObj = $(response);

        if (itemObj.hasClass("boxing")) {

            if (itemObj.hasClass("medical-report")) { //medical report
                if (updatedRaw != undefined) { //update
                    $(updatedRaw).after(itemObj);
                    updatedRaw.remove();
                }
                else {//create
                    $(".reports-container").append(itemObj);
                }
            }
            else { //prescriptions

                if (updatedRaw != undefined) { //update
                    $(updatedRaw).after(itemObj);
                    updatedRaw.remove();
                }
                else {//create
                    $(".prescription-container").append(itemObj);
                }

            }


            customLightBox();

        }
        else {

            if (itemObj.hasClass("payment-row")) {

                let paymentType = $(itemObj).data("type");
                let paymentValue = $(itemObj).data("payment");

                let currentBalance = parseFloat($("#CurrentBalance").text());
                let gainPayment = parseFloat($("#GainPayment").text());

                currentBalance += parseFloat(paymentValue);

                $("#CurrentBalance").text(currentBalance);

                if (paymentType == "Pay") {
                    gainPayment += Math.abs(parseFloat(paymentValue));

                    $("#GainPayment").text(gainPayment);
                }

            }


            if (updatedRaw != undefined) { //update
                datatable.row(updatedRaw).remove().draw();
                datatable.row.add(itemObj).draw();
            }
            else {//create
                datatable.row.add(itemObj).draw();
            }

        }


        KTMenu.init();
        KTMenu.initHandlers();
    });

}
function onRequestFailure(response) {
    Swal.fire({
        text: `${response.responseText}`,
        icon: "warning",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn btn-primary",
        }
    });
}
function onRequestComplete() {
    $("body :submit").removeAttr("disabled").removeAttr("data-kt-indicator");
};