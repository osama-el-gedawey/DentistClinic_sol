const medicineList = [];
const analysisList = [];
const xraysList = [];

$(document).ready(function () {




    $("body").delegate(".js-delete-pmedicine", "click", function () {

        var deleteBtn = $(this);
        //handl confirmation sweetAlert2
        //call ajax
        $.post({

            url: deleteBtn.data("url"),
            cache: false,
            data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

            success: function () {
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Medicine has been deleted",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {

                    deleteBtn.closest(".medicine-box").remove();
                    //remove it from datatable
                });
            },
            error: function (response) {
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
        });

    });
    $("body").delegate(".js-delete-panalysis", "click", function () {

        var deleteBtn = $(this);
        //handl confirmation sweetAlert2
        //call ajax
        $.post({

            url: deleteBtn.data("url"),
            cache: false,
            data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

            success: function () {
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Medicine has been deleted",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {

                    deleteBtn.closest(".analysis-box").remove();
                    //remove it from datatable
                });
            },
            error: function (response) {
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
        });

    });
    $("body").delegate(".js-delete-pxray", "click", function () {

        var deleteBtn = $(this);
        //handl confirmation sweetAlert2
        //call ajax
        $.post({

            url: deleteBtn.data("url"),
            cache: false,
            data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

            success: function () {
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Xray has been deleted",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {

                    deleteBtn.closest(".xray-box").remove();
                    //remove it from datatable
                });
            },
            error: function (response) {
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
        });

    });



    $("body").delegate(".js-render-prescription-modal", "click", function (event) {

        event.preventDefault();

        const renderModelBtn = $(this);

        const modal = $("#PrescriptionModal");

        updatedRaw = undefined;

        if (renderModelBtn.data("update") !== undefined) {
            updatedRaw = renderModelBtn.closest(`.${renderModelBtn.data('box')}`);
        }

        //set modal body (ajax call)
        $.get({

            url: renderModelBtn.data("url"),
            success: function (result) {

                modal.find(".modal-title").html(renderModelBtn.data("title"));
                modal.find(".modal-body").html(result);



                if ($('.js-select2').length) {

                    $('.js-select2').select2({
                        dropdownParent: $('#PrescriptionModal'),
                    });

                }

                if ($('.js-days').length) {

                    $('.js-days').select2({
                        dropdownParent: $('#PrescriptionModal'),
                        tags: true
                    });

                }


                ApplaySelect2();

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



    });

    //handle Select2 function
    function ApplaySelect2() {

        $('.js-select2').on('select2:select', function (e) {
            console.log("omar")
            let select = $(this);
            $('#medicineForm').validate();

        });
    };


    customLightBox();
    
});


//functionalites

function customLightBox() {    
    ////handle lightcustom
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

function disableBtnSubmit() {
    $("body :submit").attr("disabled", "disabled").attr("data-kt-indicator", "on");
}
function onPrescriptionRequestBegin() {
    disableBtnSubmit();
};
function onPrescriptionRequestSuccess(response) {
    let modal = $("#PrescriptionModal");
    modal.modal("hide");

    Swal.fire({
        position: "center",
        icon: "success",
        title: "Successfully",
        showConfirmButton: false,
        timer: 1500
    }).then(() => {

        var itemObj = $(response);


        if (updatedRaw != undefined) {

            console.log(updatedRaw);
            $(updatedRaw).after(itemObj);
            $(updatedRaw).remove();

        }
        else {


            if (itemObj.hasClass("medicine-box")) { //check if response is medicine
                $(".medicines-container").append(itemObj);
            }

            if (itemObj.hasClass("analysis-box")) {//check if response is analysis
                $(".analysis-container").append(itemObj);
            }

            if (itemObj.hasClass("xray-box")) {//check if response is analysis
                $(".xrays-container").append(itemObj);
            }

        }



        customLightBox();
        KTMenu.init();
        KTMenu.initHandlers();
    });

}
function onPrescriptionRequestFailure(response) {
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
function onPrescriptionRequestComplete() {
    $("body :submit").removeAttr("disabled").removeAttr("data-kt-indicator");
};