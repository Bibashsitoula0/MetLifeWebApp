using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Models;
using System.Diagnostics.CodeAnalysis;
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

        public async Task<List<GetBookingStatus>> getbookingstatus()
        {
            var query = "exec sp_get_booking_status";
            var results = await _dah.FetchDerivedModelAsync<GetBookingStatus>(query);
            return results.ToList();
        }
        public async Task<List<Order>> getorderList(DashboardFilterOrder filter)
        {
            try
            {
                if (filter.order != null)
                {
                    filter.order = filter.order.Trim();
                }               

                var query = "exec sp_get_order @from_date, @to_date,@orders,@statuss,@pages,@pageSizes";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate, orders = filter.order, statuss = filter.status,pages = filter.page, pageSizes = filter.pageSize};
                var results = await _dah.FetchDerivedModelAsync<Order>(query, parameter);
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }




        public async Task<List<InternationalDoctorReturnType>> getinternationalData(DashboardFilter filter)
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

                var query = "exec sp_get_international_doctor @from_date, @to_date,@code,@pending,@booking,@page,@pageSize,@name,@contact";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate, code = filter.code, pending = filter.pending_status, booking = filter.booking_status, page = filter.page, pageSize = filter.pageSize, name = filter.name, contact = filter.contact };
                var results = await _dah.FetchDerivedModelAsync<InternationalDoctorReturnType>(query, parameter);
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<VirtualDoctorReturnType>> getvirtualData(DashboardFilter filter)
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

                var query = "exec sp_get_virtual_doctor @from_date, @to_date,@code,@pending,@booking,@page,@pageSize,@name,@contact";
                var parameter = new { from_date = filter.formDate, to_date = filter.toDate, code = filter.code, pending = filter.pending_status, booking = filter.booking_status, page = filter.page, pageSize = filter.pageSize, name = filter.name, contact = filter.contact };
                var results = await _dah.FetchDerivedModelAsync<VirtualDoctorReturnType>(query, parameter);
                return results.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<GetPaymentStatus>> getpaymentstatus()
        {
            var query = "exec sp_get_payment_status";        
            var results = await _dah.FetchDerivedModelAsync<GetPaymentStatus>(query);
            return results.ToList();
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

        public async Task<bool> Virtualdoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            try
            {
                string query = @"UPDATE virtualdoctor
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

        public async Task<bool> Internationaldoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            try
            {
                string query = @"UPDATE internationaldoctor
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
