
//document ready
$(document).ready(function () {


    $("body").delegate(".js-delete-report", "click", function () {

        var deleteBtn = $(this);
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure want delete this report?",
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
                            title: "Medical Report has been deleted",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            
                            deleteBtn.closest(".medical-report").remove();
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

    $("body").delegate(".js-delete-plan", "click", function () {
        var deleteBtn = $(this);

        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure want delete this plan?",
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
                            title: "Medical Report has been deleted",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {

                            deleteBtn.closest(".plan-row").remove();
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


    //Create new Prescription
    $(".js-create-prescription").on("click", function () {


        let createPrescriptionBtn = $(this); 


        //call ajax
        $.post({

            url: createPrescriptionBtn.data("url"),
            cache: false,
            data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

            success: function (response) {
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "Prescription has been created",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {

                    $(".prescription-container").prepend(response);

                    KTMenu.init();
                    KTMenu.initHandlers();
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
    $(".js-delete-prescription").on("click", function () {
        var deleteBtn = $(this);

        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure want delete this prescription?",
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
                            title: "Prescription has been deleted",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {

                            deleteBtn.closest(".prescription-box").remove();
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

    $("body").delegate(".js-show-prescription", "click", function () {

        var deleteBtn = $(this);
        //handl confirmation sweetAlert2
        $.post({

            url: deleteBtn.data("url"),
            cache: false,
            data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

            success: function (response) {
                window.location.href = response.redirectToUrl;
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


    //appoinemts
    $("body").delegate(".js-reserve-appointment", "click", function (event) {
        event.preventDefault();
        let patientId = $(event.target).attr("data-id");

        let modal = $("#reserveAppointment");

        $.get({

            url: "/Appointments/GetAvaillableAppointments",
            data: { patientId: patientId },
            cache: false,
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                let cartona = ``;
                Array.from(response.appointments).forEach((appointment) => {
                    if (response.patientReservedAppointments.some((x) => x == appointment.id)) {
                        cartona += `
                            <div class="appointment-box-date m-2" data-id=${appointment.id} data-reserved=true style="cursor:pointer; border:2px dashed #F00; width:170px;border-radius:8px; padding: 10px; box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 1px 3px 1px;">
                                <div class="date-header d-flex justify-content-between align-items-center py-3">
                                    <span>${moment(appointment.start).format('ll')}</span>
                                    <span class="badge badge-light-info appointment-status">reserved</span>
                                </div>
                                <div class="date-footer d-flex justify-content-between align-items-center">
                                    <span class="appointment-start">${appointment.appointmentStart.substring(0, 5)} PM</span>
                                    <span class="fw-semibold text-muted">To</span>
                                    <span class="appointment-end">${appointment.appointmentEnd.substring(0, 5)} PM</span>
                                </div>
                                <button class="btn btn-link btn-color-danger btn-active-color-danger js-cancel-appointment">Cancel</button>
                            </div>
                    
                        `
                    }
                    else {
                        cartona += `
                            <div class="appointment-box-date m-2" data-id=${appointment.id} data-reserved=false style="cursor:pointer; width:170px;border-radius:8px; padding: 10px; box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 1px 3px 1px;">
                                <div class="date-header d-flex justify-content-between align-items-center py-3">
                                    <span>${moment(appointment.start).format('ll')}</span>
                                    <span class="badge badge-light-success appointment-status">available</span>
                                </div>
                                <div class="date-footer d-flex justify-content-between align-items-center">
                                    <span class="appointment-start">${appointment.appointmentStart.substring(0, 5)} PM</span>
                                    <span class="fw-semibold text-muted">To</span>
                                    <span class="appointment-end">${appointment.appointmentEnd.substring(0, 5)} PM</span>
                                </div>
                                <button class="btn btn-link btn-color-info btn-active-color-primary js-select-appointment">Select</button>
                            </div>
                    
                        `
                    }


                });
                $(".appointment-box").html(``);
                $(".appointment-box").html(cartona);
                $(".js-reserve-appointment-patient").attr("data-patient", patientId);
                $(".js-reserve-appointment-patient").attr("data-appointment", "");

                //hadle personal data
                $(".patient-name").text(response.patient.fullName);
                $(".patient-phone").text(response.patient.phoneNumber);
                $(".patient-image").attr("src", response.profilePicture != null ? `data:image /*;base64,@(Convert.ToBase64String(${response.profilePicture}))` : "/assets/images/user.jpg");


                getReservations(patientId);
                modal.modal("show");
            },
            error: function (response) {
                console.log(response);
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

    let selectedAppointment;
    $("body").delegate(".js-select-appointment", "click", function (event) {
        let selectBtn = $(this);

        let appointmentId = selectBtn.parent(".appointment-box-date").attr("data-id");
        $(".appointment-box-date").each((index, appointmentDate) => {
            if ($(appointmentDate).attr("data-reserved") == "false") {
                $(appointmentDate).css("border", "none")
            }

        });

        selectBtn.parent(".appointment-box-date").css("border", "1px dashed #080");
        $(".js-reserve-appointment-patient").attr("data-appointment", appointmentId);
        selectedAppointment = selectBtn.parent(".appointment-box-date");
    });

    $("body").delegate(".js-cancel-appointment", "click", function (event) {
        let cancelBtn = $(this);
        let appointmentId = $(cancelBtn).parent(".appointment-box-date").attr("data-id");
        let patientId = $(".js-reserve-appointment-patient").attr("data-patient");
        console.log(appointmentId);
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "are you sure cancel this appointment for this patient",
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

                    url: "/Appointments/CancelAppointment",
                    cache: false,
                    data: { "__RequestVerificationToken": $("#tokkenForgery").val(), appointmentId },

                    success: function () {
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: "Reservation has been Cancelled",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            //
                            let appointmentBox = cancelBtn.parent(".appointment-box-date");
                            console.log(appointmentBox);
                            appointmentBox.find("button").removeClass("js-cancel-appointment")
                                .addClass("js-select-appointment");
                            console.log(appointmentBox.find("button"));
                            appointmentBox.find("button").removeClass("btn-color-danger btn-active-color-danger")
                                .addClass("btn-color-primay btn-active-color-primary");
                            appointmentBox.find("button").text("Select");
                            appointmentBox.find(".appointment-status").text("available");
                            appointmentBox.find(".appointment-status").addClass("badge-light-success")
                                .removeClass("badge-light-info");
                            appointmentBox.attr("data-reserved", false);
                            appointmentBox.css("border", "none");

                            getReservations(patientId);
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

    $("body").delegate(".js-reserve-appointment-patient", "click", function (event) {


        let appointmentId = $(".js-reserve-appointment-patient").attr("data-appointment");
        let patientId = $(".js-reserve-appointment-patient").attr("data-patient");

        console.log(appointmentId, patientId);

        if (appointmentId && patientId) {
            $(".js-reserve-appointment-patient").attr("disabled", "disabled").attr("data-kt-indicator", "on");
            $.post({

                url: "/Appointments/ReserveAppointment",
                cache: false,
                data: { "__RequestVerificationToken": $("#tokkenForgery").val(), appointmentId, patientId },

                success: function () {
                    Swal.fire({
                        position: "center",
                        icon: "success",
                        title: "Reservation is Done",
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() => {
                        selectedAppointment.find("button").removeClass("js-select-appointment")
                            .addClass("js-cancel-appointment");
                        selectedAppointment.find("button").addClass("btn-color-danger btn-active-color-danger")
                            .removeClass("btn-color-primay btn-active-color-primary");
                        selectedAppointment.find("button").text("Cancel");
                        selectedAppointment.find(".appointment-status").text("reserved");
                        selectedAppointment.find(".appointment-status").removeClass("badge-light-success")
                            .addClass("badge-light-info");
                        selectedAppointment.attr("data-reserved", true);
                        selectedAppointment.css("border", "2px dashed #F00");

                        getReservations(patientId);
                        $(".js-reserve-appointment-patient").removeAttr("disabled").removeAttr("data-kt-indicator");
                    })
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
                    }).then(() => {
                        $(".js-reserve-appointment-patient").removeAttr("disabled").removeAttr("data-kt-indicator");
                    })
                }
            });
        }
        else {
            Swal.fire({
                text: `no appointment selected..!!`,
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





function disableBtnSubmit() {
    $("body :submit").attr("disabled", "disabled").attr("data-kt-indicator", "on");
}

function getReservations(patientId) {
    $.get({

        url: "/Appointments/GetPatientReservation",
        data: { patientId },
        cache: false,
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            console.log(response);
            let cartona = ``;
            Array.from(response.appointments).forEach((appointment) => {
                cartona += `
                            <div class="appointment-box-date m-2" data-id=${appointment.id} data-reserved=true style="cursor:pointer; width:160px;border-radius:8px; padding: 10px; box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 1px 3px 1px;">
                                <div class="date-header d-flex justify-content-between align-items-center py-3">
                                    <span>${moment(appointment.start).format('ll')}</span>
                                </div>
                                <div class="date-footer d-flex justify-content-between align-items-center">
                                    <span class="appointment-start">${appointment.appointmentStart.substring(0, 5)} PM</span>
                                    <span class="fw-semibold text-muted">To</span>
                                    <span class="appointment-end">${appointment.appointmentEnd.substring(0, 5)} PM</span>
                                </div>
                            </div>

                  
                        `
            });


            $(".reservedAppointments-list").html(``);
            $(".reservedAppointments-list").html(cartona);
            $(".upcoming-appointments").text(response.patient.upComming);
            $(".prev-appointments").text(response.patient.previous);
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


$(".appointment-date-filter").flatpickr({
    altInput: true,
    altFormat: "F j, Y",
    dateFormat: "Y-m-d",
    minDate: "today",
    maxDate: new Date().fp_incr(14),
    defaultDate: "today",
    onChange: function (selectedDates, dateStr, instance) {
        let patientId = $(".js-reserve-appointment-patient").attr("data-patient");
        $.get({

            url: "/Appointments/GetAvaillableAppointmentsByDate",
            data: { dateStr, patientId },
            cache: false,
            dataType: "json",
            contentType: "application/json",
            success: function (response) {

                let cartona = ``;
                Array.from(response.appointments).forEach((appointment) => {
                    if (response.patientReservedAppointments.some((x) => x == appointment.id)) {
                        cartona += `
                            <div class="appointment-box-date m-2" data-id=${appointment.id} data-reserved=true style="cursor:pointer; border:2px dashed #F00; width:170px;border-radius:8px; padding: 10px; box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 1px 3px 1px;">
                                <div class="date-header d-flex justify-content-between align-items-center py-3">
                                    <span>${moment(appointment.start).format('ll')}</span>
                                    <span class="badge badge-light-info appointment-status">reserved</span>
                                </div>
                                <div class="date-footer d-flex justify-content-between align-items-center">
                                    <span class="appointment-start">${appointment.appointmentStart.substring(0, 5)} PM</span>
                                    <span class="fw-semibold text-muted">To</span>
                                    <span class="appointment-end">${appointment.appointmentEnd.substring(0, 5)} PM</span>
                                </div>
                                <button class="btn btn-link btn-color-danger btn-active-color-danger js-cancel-appointment">Cancel</button>
                            </div>
                  
                        `
                    }
                    else {
                        cartona += `
                            <div class="appointment-box-date m-2" data-id=${appointment.id} data-reserved=false style="cursor:pointer; width:170px;border-radius:8px; padding: 10px; box-shadow: rgba(60, 64, 67, 0.3) 0px 1px 2px 0px, rgba(60, 64, 67, 0.15) 0px 1px 3px 1px;">
                                <div class="date-header d-flex justify-content-between align-items-center py-3">
                                    <span>${moment(appointment.start).format('ll')}</span>
                                    <span class="badge badge-light-success appointment-status">available</span>
                                </div>
                                <div class="date-footer d-flex justify-content-between align-items-center">
                                    <span class="appointment-start">${appointment.appointmentStart.substring(0, 5)} PM</span>
                                    <span class="fw-semibold text-muted">To</span>
                                    <span class="appointment-end">${appointment.appointmentEnd.substring(0, 5)} PM</span>
                                </div>
                                <button class="btn btn-link btn-color-info btn-active-color-primary js-select-appointment">Select</button>
                            </div>
                  
                        `
                    }


                });


                $(".appointment-box").html(``);
                $(".appointment-box").html(cartona);
                $(".js-reserve-appointment-patient").attr("data-patient", patientId);
                $(".js-reserve-appointment-patient").attr("data-appointment", "");

            },
            error: function (response) {
                console.log(response);
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





