using MetLifeInsurance.Models;

namespace MetLifeInsurance.Repository
{
    public interface IDashboardRepository
    {
        Task<List<BookingStatus>> getbooking(DashboardFilter dashboardFilter);
        Task<List<TestWiseStatus>> gettest(DashboardFilter dashboardFilter);
        Task<List<MedicalInstituteWise>> getinstitute(DashboardFilter dashboardFilter);
        Task<List<PaymentStatus>> getpayment(DashboardFilter dashboardFilter);

        //for physicaldoctor 
        Task<List<BookingStatus>> getdoctorbooking(DashboardFilter dashboardFilter);
        Task<List<MedicalInstituteWise>> getdoctorinstitute(DashboardFilter dashboardFilter);
        Task<List<PaymentStatus>> getdoctorpayment(DashboardFilter dashboardFilter);
        Task<List<DoctorNameWise>> getdoctorstatus(DashboardFilter dashboardFilter);

        // fro tele doctor
        Task<List<BookingStatus>> getteledoctorbooking(DashboardFilter dashboardFilter);
        Task<List<MedicalInstituteWise>> getteledoctorinstitute(DashboardFilter dashboardFilter);
        Task<List<PaymentStatus>> getteledoctorpayment(DashboardFilter dashboardFilter);
        Task<List<DoctorNameWise>> getteledoctorstatus(DashboardFilter dashboardFilter);

        // fro virtual doctor
        Task<List<BookingStatus>> getvirtuualdoctorbooking(DashboardFilter dashboardFilter);
        Task<List<MedicalInstituteWise>> getvirtuualdoctorinstitute(DashboardFilter dashboardFilter);
        Task<List<PaymentStatus>> getvirtuualdoctorpayment(DashboardFilter dashboardFilter);
        Task<List<DoctorNameWise>> getvirtuualdoctorstatus(DashboardFilter dashboardFilter);

        // fro international doctor
        Task<List<BookingStatus>> getinternationaldoctorbooking(DashboardFilter dashboardFilter);
        Task<List<MedicalInstituteWise>> getinternationaldoctorinstitute(DashboardFilter dashboardFilter);
        Task<List<PaymentStatus>> getinternationaldoctorpayment(DashboardFilter dashboardFilter);
        Task<List<DoctorNameWise>> getinternationaldoctorstatus(DashboardFilter dashboardFilter);
    }
}
