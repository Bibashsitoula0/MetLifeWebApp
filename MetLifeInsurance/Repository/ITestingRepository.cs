using MetLifeInsurance.Models;

namespace MetLifeInsurance.Repository
{
    public interface ITestingRepository
    {
        Task<List<BookingStatusReturnType>> getbookingData(DashboardFilter filter);
        Task<List<PhysicalDoctorReturnType>> getphysicalData(DashboardFilter filter); 
        Task<List<TeleDoctorReturnType>> getteleData(DashboardFilter filter);
        Task<bool> HomeTestingFeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Physicaldoctorfeedback(string? id, int? rating, string type, string remarks);
        Task<bool> Teledoctorfeedback(string? id, int? rating, string type, string remarks);
    }
}
