$(document).ready(function () {

    //hande toggle status
    $("body").delegate(".js-delete-Reception", "click", function () {

        const deleteBtn = $(this);
   
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure want delete this ?",
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

                    url: deleteBtn.data("url"),
                    cache: false,
                    data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

                    success: function () {
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: "Successfully",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            const targetRaw = deleteBtn.closest("tr");
                            datatable.row(targetRaw).remove().draw();
                            $(targetRaw).remove();
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
            }
        });

    });



});