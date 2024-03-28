namespace DentistClinic.Services.Interfaces
{
    public interface IUnitOfWork
    {
        public IPatientRepository patientRepository { get; set; }
        public IAppointmentRepository appointmentRepository { get; set; }
        public IMedicineRepository medicineRepository { get; set; }
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
    }
}
