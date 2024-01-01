using Dapper.Contrib.Extensions;
using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Models;
using MetLifeInsurance.Repository.RegisterRepository.AccountRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MetLifeInsurance.Repository.AccountRepository
{
    public class RegisterRepository : DALConfig , IRegisterRepository
    {
        public readonly IDataAccessLayer _dah;
        public RegisterRepository(IDataAccessLayer dah)
        {
            _dah = dah;

        }
        public async Task<bool> Changestatus(int userId, bool is_active)
        {
            string query = @"update Users set 
                        is_active= @isactive                  
                        where
                        id = @userid                       
                ";
         
            var parameters = new
            {
                isactive = is_active,
                userid = userId
            };
            var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameters);
            return true;
        }

        public async Task<bool> EditRole(string UserId, string RoleId)
        {

            string query = @"update AspNetUserRoles set 
                        RoleId= @roleid                  
                        where
                        UserId = @userid                       
                ";

            var parameters = new
            {
                userid = UserId,
                roleid = RoleId
            };
            var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameters);
            return true;
        }

        public async Task<bool> EditUser(string Id, string Email)
        {
            string query = @"update AspNetUsers set 
                         UserName= @email,Email= @email                
                        where
                        id = @id                       
                ";

            var parameters = new
            {
                id = Id,
                email = Email
            };
            var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameters);
            return true;
        }

        public async Task<List<RegisterUser>> getallusers()
        {
            string query = @"select * from Users";           
            var user = await _dah.FetchDerivedModelAsync<RegisterUser>(query);
            return user;
        }

        public async Task<List<AspNetRole>> getroleid(string Role)
        {
            string query = @"select Id,Name from AspNetRoles where Name=@role
           ";
            var parameters = new
            {
              role = Role
            };
            var user = await _dah.FetchDerivedModelAsync<AspNetRole>(query, parameters);
            return user.ToList();
        }

        public async Task<List<ApplicationUser>> getUserByEmail(string email)
        {
            string query = @"select is_active from AspNetUsers where Email=@Email
           ";
            var parameters = new
            {
                Email = email
            };
            var user = await _dah.FetchDerivedModelAsync<ApplicationUser>(query, parameters);
            return user.ToList();
        }

        public async Task<int> Adduser(RegisterUser registerUser)
        {
            using (IDbConnection db = GetDbConnection())
            {
                var id = await db.InsertAsync(registerUser);
                return id;
            }
        }

        public async Task<bool> EditUser(long userid, string username, string email, string role)
        {
            string query = @"update Users set 
                        Role = @role ,username=@username,email=@email                
                        where
                        Id = @userid                       
                ";

            var parameters = new
            {
                userid = userid,
                role = role,
                username = username,
                email= email
            };
            var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameters);
            return true;
        }

        public async Task<List<RegisterUser>> getUser(string username)
        {
            string query = @"select * from Users                 
                        where
                        UserName = @username";

            var parameters = new
            {
                username = username
            };
            var data = await _dah.FetchDerivedModelAsync<RegisterUser>(query, parameters);
            return data;
        }
    }
}
