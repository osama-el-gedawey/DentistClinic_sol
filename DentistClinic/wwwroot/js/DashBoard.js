const ctxRservation = document.getElementById('ReservationChart');
const dataReservation = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [{
        label: 'Reservations',
        data: dataReservationFromModel,
        backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(255, 159, 64, 0.2)',
            'rgba(255, 205, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(153, 102, 255, 0.2)'
        ],
        borderColor: [
            'rgb(255, 99, 132)',
            'rgb(255, 159, 64)',
            'rgb(255, 205, 86)',
            'rgb(75, 192, 192)',
            'rgb(54, 162, 235)',
            'rgb(153, 102, 255)'
        ],
        borderWidth: 1
    }]
};
const configRservation = {
    type: 'bar',
    data: dataReservation,
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    },
};
new Chart(ctxRservation, configRservation);
/////////////////////////////////////////////////////
const ctxPayment = document.getElementById('PaymentChart');
const dataPayment = {
    labels: [
        'Earnings',
        'Remaining Credit'
    ],
    datasets: [{
        label: 'Payments',
        data: dataPaymentFromModel,
        backgroundColor: [
            'rgb(144,238,144)',
            'rgb(255, 99, 132)'
            
        ],
        hoverOffset: 4
    }]
};
const configPayment = {
    type: 'pie',
    data: dataPayment,
};
new Chart(ctxPayment, configPayment);
////////////////////////////////////////////////////
const ctxPayment2 = document.getElementById('Payment2Chart');
const dataPayment2 = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    datasets: [
        {
            label: 'Earnings',
            data: dataPayment2FromModel,
            borderColor: 'rgb(144,238,144)',
            backgroundColor: 'rgba(144,238,144,0.2)',
            borderWidth: 2,
            borderRadius: Number.MAX_VALUE,
            borderSkipped: false,
        },
        {
            label: 'Remaining Credit',
            data: dataPayment3FromModel,
            borderColor: 'rgb(255, 99, 132)',
            backgroundColor: 'rgba(255, 99, 132,0.2)',
            borderWidth: 2,
            borderRadius: 5,
            borderSkipped: false,
        }
    ]
};
const configPayment2 = {
    type: 'bar',
    data: dataPayment2,
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    },
};
new Chart(ctxPayment2, configPayment2);
///////////////////////////////////////
const ctxGender = document.getElementById('GenderChart');
const dataGender = {
    labels: [
        'Male',
        'Female',
        'Child'
    ],
    datasets: [{
        label: 'Gender',
        data: dataGenderFromModel,
        backgroundColor: [
            'rgb(111,168,220)',
            'rgb(213,166,189)',
            'rgb(255, 191, 0)'
        ],
        hoverOffset: 4
    }]
};
const configGender = {
    type: 'pie',
    data: dataGender,
};
new Chart(ctxGender, configGender);
///////////////////////////////////////

const ctxPatientStatus = document.getElementById('PatientStatusChart');
const dataPatientStatus = {
    labels: [
        'Online',
        'Offline'
    ],
    datasets: [{
        label: 'Patient Status',
        data: PatientStatusFromModel,
        backgroundColor: [
            'rgb(176, 30, 104)',
            'rgb(255, 225, 93)'
        ],
        hoverOffset: 4
    }]
};
const configPatientStatus = {
    type: 'pie',
    data: dataPatientStatus,
};
new Chart(ctxPatientStatus, configPatientStatus);