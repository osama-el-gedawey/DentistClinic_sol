"use strict";


$(document).ready(function () {

    $('.js-automatically-appointment').unbind().bind('click', function (event) {

        let modal = $("#automaticallyModal");

        modal.modal("show");

    });


    $('.js-save-appointment-auto').unbind().bind('click', function (event) {

        let modal = $("#automaticallyModal");

        const appointmentForm = document.getElementById("AppointmentAutomaticallyForm");
        const currentAppointment = new Object();
        currentAppointment.startHour = appointmentForm.hour_start.value;
        currentAppointment.endHour = appointmentForm.hour_end.value;
        currentAppointment.start = moment(appointmentForm.current_date.value).format('YYYY-MM-D');
        currentAppointment.end = moment(appointmentForm.current_date.value).format('YYYY-MM-D');
        currentAppointment.slot = appointmentForm.slot.value;

        $.get({

            url: "/Appointments/AddAutomaticaly",
            cache: false,
            data: currentAppointment,
            dataType: "json",
            contentType: "application/json",
            success: function (response) {
                modal.modal("hide");
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: "appointments has been saved",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    KTGeneralFullCalendarSelectDemos.init();
                })
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

        console.log(currentAppointment);

    });

});

// Class definition
var KTGeneralFullCalendarSelectDemos = function () {
    // Private functions

    var exampleSelect = async function  () {

        function getData() {
            return new Promise((resoleve, reject) => {
                var Request = new XMLHttpRequest();
                Request.open("GET", "/Appointments/GetAllAppointments");
                Request.send();

                Request.onreadystatechange = function () {
                    if (Request.readyState == 4 && Request.status == 200) {
                        var arrayEvents = [];
                        var allEvents = Array.from(JSON.parse(Request.responseText)).forEach((event) => {
                            if (event.isReserved) {
                                event.color = "red";
                            }
                            else {
                                event.color = "green";
                            }

                            arrayEvents.push(event);
                        });

                        console.log(arrayEvents);

                        resoleve(arrayEvents);
                    }
                }
            })
        }

        var allEvents = await getData();
        var isAppointmentUpdated = false;
        console.log(allEvents);
        // Define variables
        var calendarEl = document.getElementById('kt_docs_fullcalendar_selectable');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            initialDate: Date.now(),
            navLinks: true, // can click day/week names to navigate views
            selectable: true,
            selectMirror: true,
            eventStartEditable: false,
            // Create new event
            select: function (arg) {
                const dateSeleted = moment(arg.start).format('YYYY-MM-D');
                const dateOfDay = moment(new Date()).format('YYYY-MM-D');

                if (new Date(dateSeleted) < new Date(dateOfDay)) {
                    Swal.fire({
                        text: "appointment can't be in the past ..!!",
                        icon: "warning",
                        buttonsStyling: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn btn-primary"
                        }
                    });
                }
                else {
                    let modal = $("#appointmentModal");
                    isAppointmentUpdated = false;
                    $(".appointment-date").text(`Appointment Date : ${new Date(arg.start).toDateString()}`);
                    $(".modal-title").text(`Create New Appointment`);
                    $(".appointment-start").val(`16:00`);
                    $(".appointment-end").val(`16:30`);
                    $(".js-save-appointment").removeClass("btn-warning");
                    $(".js-save-appointment").removeClass("d-none");
                    $(".js-save-appointment").addClass("btn-primary");
                    $(".js-save-appointment").html(`Create Appointment`);
                    $(".js-delete-appointment").remove();
                    //show modal
                    modal.modal("show");
                   
                    $('.js-save-appointment').unbind().bind('click', function (event) {
                        if (!isAppointmentUpdated) {
                            event.preventDefault();
                            const appointmentForm = document.getElementById("AppointmentForm");
                            const currentAppointment = new Object();
                            currentAppointment.startTime = appointmentForm.appointment_start.value;
                            currentAppointment.endTime = appointmentForm.appointment_end.value;
                            currentAppointment.start = moment(arg.start).format('YYYY-MM-D');
                            currentAppointment.end = moment(arg.end).format('YYYY-MM-D');
                            $.get({

                                url: "/Appointments/CreateAppointment",
                                cache: false,
                                data: currentAppointment,
                                dataType: "json",
                                contentType: "application/json",
                                success: function (response) {
                                    console.log(response);
                                    modal.modal("hide");
                                    Swal.fire({
                                        position: "center",
                                        icon: "success",
                                        title: "appointment has been saved",
                                        showConfirmButton: false,
                                        timer: 1500
                                    }).then(() => {
                                        KTGeneralFullCalendarSelectDemos.init();
                                    })
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
                }
               
            },

            // Delete event
            eventClick: function (arg) {
                calendarEl.events = [];
                let modal = $("#appointmentModal");
                isAppointmentUpdated = true;
                $(".appointment-date").text(`Appointment Date : ${new Date(arg.event.start).toDateString()}`)
                $(".modal-title").text(`Appointment Details for ${arg.event.title}`);
                $(".appointment-start").val(arg.event.extendedProps.appointmentStart.substring(0, 5));
                $(".appointment-end").val(arg.event.extendedProps.appointmentEnd.substring(0, 5));
                $(".js-save-appointment").removeClass("btn-primary");
                $(".js-save-appointment").addClass("btn-warning");
                $(".js-save-appointment").html(`Edit Appointment`);
                let dateSeleted = moment(arg.event.start).format('YYYY-MM-D');
                let dateOfDay = moment(new Date()).format('YYYY-MM-D');


                if (!$(".js-delete-appointment").length) {
                    jQuery('<btn>', {
                        class: 'btn-sm btn btn-danger js-delete-appointment',
                    }).text('Delete').insertBefore('.js-save-appointment');
                }

                if (new Date(dateSeleted) <= new Date(dateOfDay)) {
                    $(".js-save-appointment").addClass("d-none");
                    $(".js-delete-appointment").addClass("d-none");
                }
                else {
                    $(".js-save-appointment").removeClass("d-none");
                    $(".js-delete-appointment").removeClass("d-none");
                }
                


                //show modal
                modal.modal("show");

                //update
                $('.js-save-appointment').unbind().bind('click', function (event) {
                    if (isAppointmentUpdated) {
                        event.preventDefault();
                        const appointmentForm = document.getElementById("AppointmentForm");
                        const currentAppointment = new Object();
                        currentAppointment.id = arg.event.id;
                        currentAppointment.startTime = appointmentForm.appointment_start.value;
                        currentAppointment.endTime = appointmentForm.appointment_end.value;
                        currentAppointment.start = moment(arg.event.start).format('YYYY-MM-D');
                        currentAppointment.end = moment(arg.event.end).format('YYYY-MM-D');
                        console.log(currentAppointment);
                        $.get({

                            url: "/Appointments/EditAppointment",
                            cache: false,
                            data: currentAppointment,
                            dataType: "json",
                            contentType: "application/json",
                            success: function (response) {
                                console.log(response);
                                modal.modal("hide");
                                Swal.fire({
                                    position: "center",
                                    icon: "success",
                                    title: "appointment has been saved",
                                    showConfirmButton: false,
                                    timer: 1500
                                }).then(() => {
                                    KTGeneralFullCalendarSelectDemos.init();
                                })
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

                //delete
                $('.js-delete-appointment').unbind().bind('click', function (event) {
                    console.log("deleted");
                    event.preventDefault();
                    Swal.fire({
                        text: 'Are you sure you want to delete this Appointment?',
                        icon: "warning",
                        showCancelButton: true,
                        buttonsStyling: false,
                        confirmButtonText: "Yes, delete it!",
                        cancelButtonText: "No, return",
                        customClass: {
                            confirmButton: "btn btn-primary",
                            cancelButton: "btn btn-active-light"
                        }
                    }).then(function (result) {
                        if (result.value) {
                            $.get({
                                url: "/Appointments/DeleteAppointment",
                                cache: false,
                                data: { id: arg.event.id },
                                dataType: "json",
                                contentType: "application/json",
                                success: function (response) {
                                    console.log(response);
                                    modal.modal("hide");
                                    Swal.fire({
                                        position: "center",
                                        icon: "success",
                                        title: "appointment has been deleted",
                                        showConfirmButton: false,
                                        timer: 1500
                                    }).then(() => {
                                        KTGeneralFullCalendarSelectDemos.init();
                                    })
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
                    //if (isAppointmentUpdated) {
                    //    event.preventDefault();
                    //    const appointmentForm = document.getElementById("AppointmentForm");
                    //    const currentAppointment = new Object();
                    //    currentAppointment.id = arg.event.id;
                    //    currentAppointment.startTime = appointmentForm.appointment_start.value;
                    //    currentAppointment.endTime = appointmentForm.appointment_end.value;
                    //    currentAppointment.start = moment(arg.event.start).format('YYYY-MM-D');
                    //    currentAppointment.end = moment(arg.event.end).format('YYYY-MM-D');
                    //    console.log(currentAppointment);
                    //    $.get({

                    //        url: "/Appointments/EditAppointment",
                    //        cache: false,
                    //        data: currentAppointment,
                    //        dataType: "json",
                    //        contentType: "application/json",
                    //        success: function (response) {
                    //            console.log(response);
                    //            modal.modal("hide");
                    //            Swal.fire({
                    //                position: "center",
                    //                icon: "success",
                    //                title: "appointment has been saved",
                    //                showConfirmButton: false,
                    //                timer: 1500
                    //            }).then(() => {
                    //                KTGeneralFullCalendarSelectDemos.init();
                    //            })
                    //        },
                    //        error: function (response) {
                    //            console.log(response);
                    //            Swal.fire({
                    //                text: `${response.responseText}`,
                    //                icon: "warning",
                    //                buttonsStyling: false,
                    //                confirmButtonText: "Ok, got it!",
                    //                customClass: {
                    //                    confirmButton: "btn btn-primary",
                    //                }
                    //            });
                    //        }

                    //    });
                    //}
                });
            },

            editable: false,
            dayMaxEvents: true, // allow "more" link when too many events
            editable: true,
            lazyFetching: true,
            events: allEvents,
        });


        calendar.render();
        
    }

    return {
        // Public Functions
        init: function () {
            exampleSelect();

        }
    };




}();

// On document ready
KTUtil.onDOMContentLoaded(function () {
    KTGeneralFullCalendarSelectDemos.init();

});


function disableBtnSubmit() {
    $("body :submit").attr("disabled", "disabled").attr("data-kt-indicator", "on");
}


$(".appointment-start").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    minTime: "16:00",
    maxTime: "22:30",
    defaultDate: "16:00",
    onOpen: [
        function (selectedDates, dateStr, instance) {
            instance.setDate(dateStr);
        },

    ],
});

//Configure FlatePicker
$(".appointment-end").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    minTime: "16:00",
    maxTime: "22:30",
    defaultDate: "16:30",
    onOpen: [
        function (selectedDates, dateStr, instance) {
            instance.setDate(dateStr);
        },

    ],
});


$(".current-date").flatpickr({
    altInput: true,
    altFormat: "F j, Y",
    dateFormat: "Y-m-d",
    minDate: new Date().fp_incr(1),
    maxDate: new Date().fp_incr(14),
    defaultDate: new Date().fp_incr(1),
    
});

$(".hour-start").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    minTime: "16:00",
    maxTime: "22:30",
    defaultDate: "16:00",
    onOpen: [
        function (selectedDates, dateStr, instance) {
            instance.setDate(dateStr);
        },

    ],
});

//Configure FlatePicker
$(".hour-end").flatpickr({
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    minTime: "17:00",
    maxTime: "22:30",
    defaultDate: "22:30",
    onOpen: [
        function (selectedDates, dateStr, instance) {
            instance.setDate(dateStr);
        },

    ],
});