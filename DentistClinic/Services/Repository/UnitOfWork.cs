using DentistClinic.Services.Interfaces;

namespace DentistClinic.Services.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        public IPatientRepository patientRepository { get; set;}
        public IAppointmentRepository appointmentRepository { get; set; }
        public IMedicineRepository medicineRepository { get; set;}
        public IXrayRepository xrayRepository { get; set; }
        public IAnalysisRepository analysisRepository { get; set; }
		public IContactRepository contactRepository { get; set; }
        public IPaymentsRepository paymentsRepository { get; set; }
        public IMedicalHistoryRepository medicalHistoryRepository { get; set; }
        public IMedicalHistoryImageRepository medicalHistoryImageRepository { get; set; }
        public ITreatmentPlansRepository treatmentPlansRepository { get; set; }
        public IToothRepository toothRepository { get; set; }
        public IPrescriptionRepository prescriptionRepository { get; set; }
        public IMedicinePrescriptionRepository medicinePrescriptionRepository { get; set; }
        public IAnalysisPrescriptionRepository analysisPrescriptionRepository { get; set; }
        public IXraysPrescriptionRepository xraysPrescriptionRepository { get; set; }

        public UnitOfWork(IPatientRepository patientRepository, IAppointmentRepository appointmentRepository, IMedicineRepository medicineRepository, IXrayRepository xrayRepository, IAnalysisRepository analysisRepository, IContactRepository contactRepository, IPaymentsRepository paymentsRepository, IMedicalHistoryRepository medicalHistoryRepository, IMedicalHistoryImageRepository medicalHistoryImageRepository, ITreatmentPlansRepository treatmentPlansRepository = null, IToothRepository toothRepository = null, IPrescriptionRepository prescriptionRepository = null, IMedicinePrescriptionRepository medicinePrescriptionRepository = null, IAnalysisPrescriptionRepository analysisPrescriptionRepository = null, IXraysPrescriptionRepository xraysPrescriptionRepository = null)
        {
            this.patientRepository = patientRepository;
            this.appointmentRepository = appointmentRepository;
            this.medicineRepository = medicineRepository;
            this.xrayRepository = xrayRepository;
            this.analysisRepository = analysisRepository;
            this.contactRepository = contactRepository;
            this.paymentsRepository = paymentsRepository;
            this.medicalHistoryRepository = medicalHistoryRepository;
            this.medicalHistoryImageRepository = medicalHistoryImageRepository;
            this.treatmentPlansRepository = treatmentPlansRepository;
            this.toothRepository = toothRepository;
            this.prescriptionRepository = prescriptionRepository;
            this.medicinePrescriptionRepository = medicinePrescriptionRepository;
            this.analysisPrescriptionRepository = analysisPrescriptionRepository;
            this.xraysPrescriptionRepository = xraysPrescriptionRepository;
        }


    }
}
