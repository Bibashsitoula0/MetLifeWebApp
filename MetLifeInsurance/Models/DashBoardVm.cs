namespace MetLifeInsurance.Models
{
    public class DashBoardVm
    {
        public List<PaymentStatus> paymentstatus { get; set; }
        public List<MedicalInstituteWise> medicalstatus { get; set; }
        public List<BookingStatus> bookingstatus { get; set; }
        public List<TestWiseStatus> teststatus { get; set; }
        public List<DoctorNameWise> doctorstatus{get;set;}
    }
}
