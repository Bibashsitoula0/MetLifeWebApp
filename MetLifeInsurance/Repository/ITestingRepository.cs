using MetLifeInsurance.Models;

namespace MetLifeInsurance.Repository
{
    public interface ITestingRepository
    {
        Task<List<BookingStatusReturnType>> getbookingData(DashboardFilter filter);
        Task<List<PhysicalDoctorReturnType>> getphysicalData(DashboardFilter filter); 
        Task<List<TeleDoctorReturnType>> getteleData(DashboardFilter filter);
        Task<List<VirtualDoctorReturnType>> getvirtualData(DashboardFilter filter);
        Task<List<InternationalDoctorReturnType>> getinternationalData(DashboardFilter filter);
        Task<List<Order>> getorderList(DashboardFilterOrder filter);
        Task<bool> HomeTestingFeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Physicaldoctorfeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Teledoctorfeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Virtualdoctorfeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Internationaldoctorfeedback(string? id, int? rating, string type, string remarks);
        Task<List<GetBookingStatus>> getbookingstatus();
        Task<List<GetPaymentStatus>> getpaymentstatus();

    }
}
