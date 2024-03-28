
$(document).ready(function () {

    //Configure FlatePicker
    var maxYear = (new Date().getFullYear() - 2);
    $(".patient-birthdate").flatpickr({
        dateFormat: "Y-m-d",
        maxDate: `${maxYear}-01-01`,
    });


    //Handle Select2
    $('.js-select2').on('select2:select', function (e) {
        let select = $(this);
        $('form').not('#signout').validate().element('#' + select.attr('id'));
    });

    let filterSearchForAvailable = $('.filterSearchForAvailable');

    filterSearchForAvailable.on("keyup", function () {
        var input = $(this);
        datatableAvailable.search(input.val()).draw();
    });


    //deleted patient datatable
    datatableAvailable = $('#patients').DataTable({
        searchDelay: 500,
        serverSide: true,
        autoWidth: false,
        processing: true,
        stateSave: false,
        order: [[1, 'asc']],
        lengthMenu: [5, 10, 15, 25, 50, 75, 100],
        'drawCallback': function () {
            KTMenu.createInstances();
        },
        language: {
            processing:
                `

                        <div class="spinner-border text-primary dt-spinner" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                `
        },
        ajax: {
            url: '/Patients/GetPatients',
            type: 'POST'
        },

        columns: [
            { "data": "id", "name": "Id", "className": "d-none" },
            {
                "name": "FullName",
                "className": "d-flex align-items-center",
                "render": function (data, type, row) {
                    return `
                            <!--begin:: Patient -->
                            <div class="symbol  symbol-50px overflow-hidden me-3">
                                <a href="/Patients/Details/${row.id}">
                                    <div class="symbol-label">
                                        <img src="${(row.profilePicture === null ? '/assets/images/user.jpg' : row.profilePicture)}" alt="Cover" class="w-100">
                                    </div>
                                </a>
                            </div>
                            <!--end::Patient-->
                            <!--begin::Patient details-->
                                <div class="d-flex flex-column">
                                    <a href="/Patients/Details/${row.id}" class="text-gray-800 text-hover-primary fw-bolder mb-1">${row.fullName}</a>
                                    <span>${row.phoneNumber}</span>
                                </div>
                            <!--begin::Patient details-->
                    `
                }
            },
            { "data": "gender", "name": "Gender" },
            {
                "name": "BirthDate",
                "render": function (data, type, row) {
                    return moment(row.birthDate).format('ll');
                }
            },
            {
                "name": "IsDeleted",
                "render": function (data, type, row) {
                    return `
                        <span class=" js-status badge badge-light-${(row.isDeleted ? "danger" : "success")} me-auto">
                            ${(row.isDeleted ? "Deleted" : "Available")}
                        </span>
                    `
                }
            },
            {
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row) {
                    if ($('#patients').data("authorize")) {
                        return `
                            <a href="#" class="btn btn-light btn-active-light-primary btn-flex btn-center btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
                                Actions
                                <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/keenthemes/metronic/docs/core/html/src/media/icons/duotune/arrows/arr072.svg-->
                                    <span class="svg-icon svg-icon-muted svg-icon-5">
                                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor"/>
                                        </svg>
                                    </span>
                                    <!--end::Svg Icon-->
                            </a>
                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-semibold fs-7 w-200px py-4" data-kt-menu="true">
                            <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:;" class="menu-link px-3 js-reserve-appointment" data-id="${row.id}">
                                        Reserve Appointment
                                    </a>
                                </div>
                            <!--end::Menu item-->
                            <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="/Patients/Details/${row.id}" class="menu-link px-3">
                                        Details
                                    </a>
                                </div>
                                <!--end::Menu item-->
                            </div>

                        `

                    }
                    else {
                        return `
                            <a href="#" class="btn btn-light btn-active-light-primary btn-flex btn-center btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
                                Actions
                                <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/keenthemes/metronic/docs/core/html/src/media/icons/duotune/arrows/arr072.svg-->
                                    <span class="svg-icon svg-icon-muted svg-icon-5">
                                        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor"/>
                                        </svg>
                                    </span>
                                    <!--end::Svg Icon-->
                            </a>
                            <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-semibold fs-7 w-200px py-4" data-kt-menu="true">
                            <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:;" class="menu-link px-3 js-reserve-appointment" data-id="${row.id}">
                                        Reserve Appointment
                                    </a>
                                </div>
                            <!--end::Menu item-->
                            <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="/Patients/Details/${row.id}" class="menu-link px-3">
                                        Details
                                    </a>
                                </div>
                                <!--end::Menu item-->
                                <!--begin::Menu item-->
                                <div class="menu-item px-3">
                                    <a href="javascript:;" class="menu-link px-3 js-toggle-status-patient" data-kt-users-table-filter="delete_row" data-url="/Patients/ToggleStatus/${row.id}">
                                        Delete
                                    </a>
                                </div>
                                <!--end::Menu item-->
                            </div>

                        `
                    }

                }
            },



        ],
        columnDefs: [
            {
                target: [0],
                visible: false,
                searchable: false,
            }
        ],

    });

    let filterSearchForDeleted = $('.filterSearchForDeleted');

    filterSearchForDeleted.on("keyup", function () {
        var input = $(this);
        datatableDeleted.search(input.val()).draw();
    });


    //deleted patient datatable
    datatableDeleted = $('#deletedPatient').DataTable({
        searchDelay: 500,
        serverSide: true,
        autoWidth: false,
        processing: true,
        stateSave: false,
        order: [[1, 'asc']],
        lengthMenu: [5, 10, 15, 25, 50, 75, 100],
        'drawCallback': function () {
            KTMenu.createInstances();

        },
        language: {
            processing:
                `

                        <div class="spinner-border text-primary dt-spinner" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                `
        },
        ajax: {
            url: '/Patients/GetDeletedPatients',
            type: 'POST'
        },

        columns: [
            { "data": "id", "name": "Id", "className": "d-none" },
            {
                "name": "FullName",
                "className": "d-flex align-items-center",
                "render": function (data, type, row) {
                    return `
                            <!--begin:: Patient -->
                            <div class="symbol  symbol-50px overflow-hidden me-3">
                                <a href="/Patients/Details/${row.id}">
                                    <div class="symbol-label">
                                        <img src="${(row.profilePicture === null ? '/assets/images/user.jpg' : row.profilePicture)}" alt="Cover" class="w-100">
                                    </div>
                                </a>
                            </div>
                            <!--end::Patient-->
                            <!--begin::Patient details-->
                                <div class="d-flex flex-column">
                                    <a href="/Patients/Details/${row.id}" class="text-gray-800 text-hover-primary fw-bolder mb-1">${row.fullName}</a>
                                    <span>${row.phoneNumber}</span>
                                </div>
                            <!--begin::Patient details-->
                    `
                }
            },
            { "data": "gender", "name": "Gender" },
            {
                "name": "BirthDate",
                "render": function (data, type, row) {
                    return moment(row.birthDate).format('ll');
                }
            },
            {
                "name": "IsDeleted",
                "render": function (data, type, row) {
                    return `
                        <span class=" js-status badge badge-light-${(row.isDeleted ? "danger" : "success")} me-auto">
                            ${(row.isDeleted ? "Deleted" : "Available")}
                        </span>
                    `
                }
            },
            {
                "className": "text-center",
                "orderable": false,
                "render": function (data, type, row) {
                    return `
                        <a href="#" class="btn btn-light btn-active-light-primary btn-flex btn-center btn-sm" data-kt-menu-trigger="click" data-kt-menu-placement="bottom-end">
                            Actions
                            <!--begin::Svg Icon | path: /var/www/preview.keenthemes.com/keenthemes/metronic/docs/core/html/src/media/icons/duotune/arrows/arr072.svg-->
                                <span class="svg-icon svg-icon-muted svg-icon-5">
                                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M11.4343 12.7344L7.25 8.55005C6.83579 8.13583 6.16421 8.13584 5.75 8.55005C5.33579 8.96426 5.33579 9.63583 5.75 10.05L11.2929 15.5929C11.6834 15.9835 12.3166 15.9835 12.7071 15.5929L18.25 10.05C18.6642 9.63584 18.6642 8.96426 18.25 8.55005C17.8358 8.13584 17.1642 8.13584 16.75 8.55005L12.5657 12.7344C12.2533 13.0468 11.7467 13.0468 11.4343 12.7344Z" fill="currentColor"/>
                                    </svg>
                                </span>
                                <!--end::Svg Icon-->
                        </a>
                        <div class="menu menu-sub menu-sub-dropdown menu-column menu-rounded menu-gray-600 menu-state-bg-light-primary fw-semibold fs-7 py-4" data-kt-menu="true" style="width:200px">
                            <!--begin::Menu item-->
                            <div class="menu-item px-3">
                                <a href="javascript:;" class="menu-link px-3 js-toggle-status-patient" data-kt-users-table-filter="delete_row" data-url="/Patients/ToggleStatus/${row.id}">
                                    Make it Available
                                </a>
                            </div>
                            <!--end::Menu item-->
                            <!--begin::Menu item-->
                            <div class="menu-item px-3">
                                <a href="javascript:;" class="menu-link px-3 js-delete-permanent" data-url="/Patients/Delete/${row.id}">
                                    Delete Permanently
                                </a>
                            </div>
                            <!--end::Menu item-->
    
                            <!--begin::Menu item-->

                            <!--end::Menu item-->
                        </div>

                        `

                }
            },



        ],
        columnDefs: [
            {
                target: [0],
                visible: false,
                searchable: false,
            }
        ],

    });

    //hande toggle status
    $("body").delegate(".js-toggle-status-patient", "click", function () {

        var toggleBtn = $(this);
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you sure that you need to toggle this item?",
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
                    cache: false,
                    data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

                    success: function (response) {
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: response,
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            datatableAvailable.ajax.reload(); 
                            datatableDeleted.ajax.reload(); 
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


    //hande delete permenent
    $("body").delegate(".js-delete-permanent", "click", function () {

        var toggleBtn = $(this);
        //handl confirmation sweetAlert2
        Swal.fire({
            title: 'Are you sure?',
            text: "you will lost all data for this patient permanently",
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
                    cache: false,
                    data: { "__RequestVerificationToken": $("#tokkenForgery").val() },

                    success: function () {
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: "Patient has been deleted",
                            showConfirmButton: false,
                            timer: 1500
                        }).then(() => {
                            datatableAvailable.ajax.reload();
                            datatableDeleted.ajax.reload();
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
                $(".patient-image").attr("src", response.profilePicture != null ? `data:image /*;base64,@(Convert.ToBase64String(${response.profilePicture}))` : "../assets/images/user.jpg");


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
        console.log(selectedAppointment);
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
        data: {patientId},
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

                            