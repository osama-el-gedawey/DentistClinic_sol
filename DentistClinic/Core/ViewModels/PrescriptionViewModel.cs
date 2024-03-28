using DentistClinic.Core.Models;

namespace DentistClinic.Core.ViewModels
{
    public class PrescriptionViewModel
    {
        public int? Id { get; set; }
        public DateOnly Date { get; set; }
        public string? Notes { get; set; }
        public  List<XrayPrescriptionViewModel> XrayPrescriptions { get; set; } = new List<XrayPrescriptionViewModel>();
        public  List<MedicinePrescriptionViewModel> MedicinePrescriptions { get; set; } = new List<MedicinePrescriptionViewModel>();
        public  List<AnalysisPrescriptionViewModel> AnalysisPrescriptions { get; set; } = new List<AnalysisPrescriptionViewModel>();
        public Patient patient { get; set; } = null!;

    }
}
