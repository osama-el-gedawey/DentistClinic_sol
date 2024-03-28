function onRequestBegin() {
    disableBtnSubmit();
};
function onRequestSuccess(response) {
    let modal = $("#Modal");
    modal.modal("hide");

    Swal.fire({
        position: "center",
        icon: "success",
        title: response.message,
        showConfirmButton: false,
        timer: 1500
    }).then(() => {
        var itemObj = $(response);

        if (updatedRaw != undefined) { //update
            $(updatedRaw).after(itemObj);
            updatedRaw.remove();
        }
        else {//create
            $(".reports-container").append(itemObj);
        }

        KTMenu.init();
        KTMenu.initHandlers();
        customLightBox();
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