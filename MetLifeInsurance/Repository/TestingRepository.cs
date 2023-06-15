using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Models;
using System.Globalization;

namespace MetLifeInsurance.Repository
{
    public class TestingRepository : DALConfig, ITestingRepository
    {
        public readonly IDataAccessLayer _dah;
        public TestingRepository(IDataAccessLayer dah)
        {
            _dah = dah; 

        }

        public async Task<List<BookingStatusReturnType>> getbookingData(DashboardFilter filter)
        {
            try
            {
                if (filter.name != null)
                {
                    filter.name = filter.name.Trim();
                }
                if (filter.code != null)
                {
                    filter.code = filter.code.Trim();
                }
                if (filter.contact != null)
                {
                    filter.contact = filter.contact.Trim();
                }

                var query = "exec sp_get_booking @from_date, @to_date,@code,@pending,@booking,@page,@pageSize,@name,@contact";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate , code=filter.code,pending=filter.pending_status,booking=filter.booking_status, page = filter.page, pageSize = filter.pageSize,name=filter.name,contact=filter.contact };
                var results = await _dah.FetchDerivedModelAsync<BookingStatusReturnType>(query, parameter);
                
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PhysicalDoctorReturnType>> getphysicalData(DashboardFilter filter)
        {

            try
            {
                if (filter.name != null)
                {
                    filter.name = filter.name.Trim();
                }
                if (filter.code != null)
                {
                    filter.code = filter.code.Trim();
                }
                if (filter.contact != null)
                {
                    filter.contact = filter.contact.Trim();
                }
                var query = "exec sp_get_physical @from_date, @to_date,@code,@pending,@booking,@page,@pageSize,@name,@contact";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate, code = filter.code, pending = filter.pending_status, booking = filter.booking_status, page = filter.page, pageSize = filter.pageSize, name = filter.name, contact = filter.contact };
                var results = await _dah.FetchDerivedModelAsync<PhysicalDoctorReturnType>(query, parameter);              
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TeleDoctorReturnType>> getteleData(DashboardFilter filter)
        {
            try
            {
                if (filter.name != null)
                {
                    filter.name = filter.name.Trim();
                }
                if (filter.code != null)
                {
                    filter.code = filter.code.Trim();
                }
                if (filter.contact != null)
                {
                    filter.contact = filter.contact.Trim();
                }

                var query = "exec sp_get_tele @from_date, @to_date,@code,@pending,@booking,@page,@pageSize,@name,@contact";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate, code = filter.code, pending = filter.pending_status, booking = filter.booking_status, page = filter.page, pageSize = filter.pageSize, name = filter.name, contact = filter.contact };
                var results = await _dah.FetchDerivedModelAsync<TeleDoctorReturnType>(query, parameter);
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> HomeTestingFeedback(string? id, int? rating, string type, string remarks)
        {
            try
            {
                
                string query = @"UPDATE booking
               SET rating = @rating, type = @type, remarks = @remarks
             WHERE booking_code = @Id ";
                var parameter = new { Id = id, rating = rating, type = type, remarks = remarks };

                var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameter);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Physicaldoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            try
            {

                string query = @"UPDATE physicaldoctor
               SET rating = @rating, type = @type, remarks = @remarks
             WHERE booking_code = @Id ";
                var parameter = new { Id = id, rating = rating, type = type, remarks = remarks };

                var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameter);
                return true;
            }
            catch (Exception)
            {

                throw;
            } 
        }

        public async Task<bool> Teledoctorfeedback(string? id, int? rating, string type, string remarks)
        {

            try
            {
                string query = @"UPDATE teledoctor
               SET rating = @rating, type = @type, remarks = @remarks
             WHERE booking_code = @Id ";
                var parameter = new { Id = id, rating = rating, type = type, remarks = remarks };

                var data = await _dah.FetchDerivedModelAsync<dynamic>(query, parameter);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
