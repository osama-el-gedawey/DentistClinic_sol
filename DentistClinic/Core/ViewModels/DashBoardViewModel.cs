using DentistClinic.Core.Constants;
using DentistClinic.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DentistClinic.Core.ViewModels
{
	public class DashBoardViewModel
    {
		public List<int> ReservationCount { get; set; }=new List<int>();
		public double paymentGains { get; set; }
        public double paymentRemaining { set; get; }
        public List<double> paymentGainsList { get; set; } = new List<double>();
        public List<double> paymentRemainingList { get; set; } = new List<double>();
        public int GenderMale {  get; set; }
        public int GenderFemale { get; set; }
        public int GenderChild { get; set; }
        public int Online { get; set; }
        public int Offline { get; set; }
    }
}
