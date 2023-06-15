using MetLifeInsurance.Models;

namespace MetLifeInsurance.Repository.RegisterRepository.AccountRepository
{
    public interface IRegisterRepository
    {
        Task<List<RegisterUser>> getallusers();
        Task<bool> EditUser(long userid,string username,string email,string role);
        Task<int> Adduser(RegisterUser registerUser);
        Task<bool> Changestatus(int userId,bool is_active);
        Task<bool> EditUser(string Id, string Email);
        Task<List<AspNetRole>> getroleid(string Role);
        Task<bool> EditRole(string UserId, string RoleId);
        Task<List<ApplicationUser>> getUserByEmail(string email);
        Task<List<RegisterUser>> getUser(string username);

    }
}
