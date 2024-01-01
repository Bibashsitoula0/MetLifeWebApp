using Dapper;
using MetLifeInsurance.DapperConfigure;
using MetLifeInsurance.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MetLifeInsurance.Repository
{
    public class DashboardRepository : DALConfig, IDashboardRepository
    {
       /* public string connectionString = "Data Source=BIBASH;Initial Catalog=MetLifeInsurance;User ID=sa;Password=sa123##;MultipleActiveResultSets=true; Integrated Security=true; TrustServerCertificate=True;";*/
        public readonly IDataAccessLayer _dah;
        public readonly string[] backgroundcolor= {
    "#9B59B6", "#BDC3C7", "#455C73", "#26B99A", "#3498DB",
    "#FFC312", "#C4E538", "#12CBC4", "#FDA7DF", "#ED4C67",
    "#F79F1F", "#A3CB38", "#1289A7", "#D980FA", "#EE5A24",
    "#009432", "#0652DD", "#9980FA", "#833471", "#EA2027",
    "#006266", "#1B1464", "#F1C40F", "#F97F51", "#B33771",
    "#6F1E51", "#F8EFBA", "#0B5345", "#E1B12C", "#4CD137",
    "#487EB0", "#E84118", "#4A69BD", "#8C7AE6", "#27AE60"
};
        public DashboardRepository(IDataAccessLayer dah)
        {
            _dah = dah;
        }

        /// <summary>
        /// booking status
        /// </summary>
        /// <param name="dashboardFilter"></param>
        /// <returns></returns>
        public async Task<List<BookingStatus>> getbooking(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_booking_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<BookingStatus>(query, parameter);
              
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }

                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TestWiseStatus>> gettest(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_test_wise_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<TestWiseStatus>(query, parameter);
                return results;

            }
            catch (Exception)
            {

                throw;
            }
           
         
        }

        public async Task<List<MedicalInstituteWise>> getinstitute(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_medical_institute_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<MedicalInstituteWise>(query, parameter);
            
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<PaymentStatus>> getpayment(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_payment_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<PaymentStatus>(query, parameter);
               int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// physical doctor
        /// </summary>
        /// <param name="dashboardFilter"></param>
        /// <returns></returns>

        public async Task<List<BookingStatus>> getdoctorbooking(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_doctor_booking_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<BookingStatus>(query, parameter);
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<MedicalInstituteWise>> getdoctorinstitute(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_doctor_medical_institute_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<MedicalInstituteWise>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PaymentStatus>> getdoctorpayment(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_doctor_payment_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<PaymentStatus>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DoctorNameWise>> getdoctorstatus(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_doctor_name_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<DoctorNameWise>(query, parameter);
             
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  tele doctor
        /// </summary>
        /// <param name="dashboardFilter"></param>
        /// <returns></returns>

        public async Task<List<BookingStatus>> getteledoctorbooking(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_tele_doctor_booking_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<BookingStatus>(query, parameter);
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<MedicalInstituteWise>> getteledoctorinstitute(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_tele_doctor_medical_institute_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<MedicalInstituteWise>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PaymentStatus>> getteledoctorpayment(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_tele_doctor_payment_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<PaymentStatus>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DoctorNameWise>> getteledoctorstatus(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_tele_doctor_name_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<DoctorNameWise>(query, parameter);

                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        ///  virtual doctor
        /// </summary>
        /// <param name="dashboardFilter"></param>
        /// <returns></returns>
        public async Task<List<BookingStatus>> getvirtuualdoctorbooking(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_virtual_doctor_booking_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<BookingStatus>(query, parameter);
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<MedicalInstituteWise>> getvirtuualdoctorinstitute(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_virtual_doctor_medical_institute_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<MedicalInstituteWise>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PaymentStatus>> getvirtuualdoctorpayment(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_virtual_doctor_payment_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<PaymentStatus>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DoctorNameWise>> getvirtuualdoctorstatus(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_virtual_doctor_name_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<DoctorNameWise>(query, parameter);

                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  international doctor
        /// </summary>
        /// <param name="dashboardFilter"></param>
        /// <returns></returns>
        /// 

        public async Task<List<BookingStatus>> getinternationaldoctorbooking(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_international_doctor_booking_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<BookingStatus>(query, parameter);
                int colorIndex = 0;

                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<MedicalInstituteWise>> getinternationaldoctorinstitute(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_international_doctor_medical_institute_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<MedicalInstituteWise>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PaymentStatus>> getinternationaldoctorpayment(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_international_doctor_payment_status_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<PaymentStatus>(query, parameter);
                int colorIndex = 0;
                foreach (var item in results)
                {
                    if (colorIndex >= backgroundcolor.Length)
                    {
                        colorIndex = 0;
                    }
                    item.color = backgroundcolor[colorIndex];
                    colorIndex++;
                }
                return results;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<DoctorNameWise>> getinternationaldoctorstatus(DashboardFilter dashboardFilter)
        {
            try
            {
                var query = "exec sp_get_international_doctor_name_count @from_date, @to_date";
                var parameter = new { from_date = dashboardFilter.formDate, to_date = dashboardFilter.toDate };
                var results = await _dah.FetchDerivedModelAsync<DoctorNameWise>(query, parameter);

                return results;

            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
