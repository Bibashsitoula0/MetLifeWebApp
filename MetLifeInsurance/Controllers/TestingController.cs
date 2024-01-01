using MetLifeInsurance.Helpers;
using MetLifeInsurance.Models;
using MetLifeInsurance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using X.PagedList;
using System.Composition;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MetLifeInsurance.Controllers
{
    [AccessValidation(Feature = "testing", Role = "admin")]
    public class TestingController : Controller
    {
        public readonly ITestingRepository _testingRepository;
        public readonly IConfiguration _configuration;
        public TestingController(ITestingRepository testingRepository, IConfiguration configuration)
        {
            _testingRepository=testingRepository;
            _configuration=configuration;   
        }
        [Route("hometesting")]
        public async Task<Object> HomeTesting(int? pageNumber,string code,string name,string contact,string bookingid,string pendingid,string fromdate,string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null )
            {
                return RedirectToAction("Login", "Account");
            }

            string[] BookingStatus = _configuration["BookingStatus:booking_status"].ToString().Split(',');
            ViewBag.BookingStatus = BookingStatus;

            string[] PeningStatus = _configuration["PendingStatus:pending_status"].ToString().Split(',');
            ViewBag.PendingStatus = PeningStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.booking_status = bookingid;
            ViewBag.pending_status = pendingid;
            ViewBag.code = code;
            ViewBag.name = name;
            ViewBag.contact = contact;


            if (pageNumber == null)
            {
                pageNumber = 1;
            }
           
            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }
            return View();
        }

        [Route("home/testing/list")]
        [HttpPost]   
        public async Task<Object> HomeTestingList(DashboardFilter filter)
        {
            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.booking_status = filter.booking_status;
            ViewBag.pending_status = filter.pending_status;
            ViewBag.code = filter.code;
            ViewBag.name = filter.name;
            ViewBag.contact = filter.contact;


            var res = await _testingRepository.getbookingData(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }           
            var pagedList = new StaticPagedList<BookingStatusReturnType>(res.Count > 0 ? res : new List<BookingStatusReturnType>(), filter.page, 10, Convert.ToInt32(totalcount));
            return PartialView("HomeTestingList", pagedList);
        }
   

      [Route("hometestingfeedback")]
        public async Task<Object> HomeTestingFeedback(string? bookingid, int? rating,string type,string remarks)
        {
            var res = await _testingRepository.HomeTestingFeedback(bookingid, rating, type, remarks);
            return RedirectToAction("HomeTesting", "Testing");
        }
                

        [Route("Physicaldoctorfeedback")]
        public async Task<Object> Physicaldoctorfeedback(string? bookingid, int? rating, string type, string remarks)
        {
            var res = await _testingRepository.Physicaldoctorfeedback(bookingid, rating, type, remarks);
            return RedirectToAction("PhysicalDoctor", "Testing");
        }



        [Route("Teledoctorfeedback")]
        public async Task<Object> Teledoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            var res = await _testingRepository.Teledoctorfeedback(id, rating, type, remarks);
            return RedirectToAction("TeleDoctor", "Testing");
        }


        [Route("Virtualdoctorfeedback")]
        public async Task<Object> Virtualdoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            var res = await _testingRepository.Virtualdoctorfeedback(id, rating, type, remarks);
            return RedirectToAction("VirtualDoctor", "Testing");
        }


        [Route("Internationaldoctorfeedback")]
        public async Task<Object> Internationaldoctorfeedback(string? id, int? rating, string type, string remarks)
        {
            var res = await _testingRepository.Internationaldoctorfeedback(id, rating, type, remarks);
            return RedirectToAction("InternationalDoctor", "Testing");
        }



        [Route("home/testing/export")]
        [HttpGet]
        public async Task<Object> BookingExport(string fromdate, string? todate, string? code, string bookingstatus, string pendingstatus,string name ,string contact)
        {
            var obj = new DashboardFilter();
            obj.booking_status = bookingstatus;
            obj.toDate = todate;
            obj.formDate = fromdate;
            obj.formDate = fromdate;
            obj.code = code;
            obj.name = name;
            obj.contact = contact;
            obj.pending_status = pendingstatus;
            obj.page = 1;
            obj.pageSize = 10000000;

            var res = await _testingRepository.getbookingData(obj);
            DataTable datatable = new DataTable();
             datatable.Columns.Add("S.N");
            datatable.Columns.Add("Booking Code");
            datatable.Columns.Add("Booking Status");
            datatable.Columns.Add("Payment Status");           
            datatable.Columns.Add("Payment Method");           
            datatable.Columns.Add("Medical Institute");
            datatable.Columns.Add("Patient Info Name");
            datatable.Columns.Add("Patient Info Contact");
            datatable.Columns.Add("Patient Info Email");
            datatable.Columns.Add("Contact_info_name");
            datatable.Columns.Add("Contact_info_address_1");
            datatable.Columns.Add("Contact_info_address_2");
            datatable.Columns.Add("Last_modified_date");
            datatable.Columns.Add("Appointment_date_time");
            datatable.Columns.Add("Prefered date");
            datatable.Columns.Add("Test");
            datatable.Columns.Add("rating");
            datatable.Columns.Add("type");
            datatable.Columns.Add("remarks");
            datatable.Columns.Add("delay_day");
            datatable.Columns.Add("patient_info_subscriber_id");
            datatable.Columns.Add("patient_info_patient_type");
            for (int i = 0; i < res.Count; i++)
            {
                datatable.Rows.Add(
                  res[i].serial_number,
                  res[i].booking_code,
                  res[i].booking_status,
                  res[i].payment_status,
                  res[i].payment_method,
                  res[i].medical_institute,               
                  res[i].patient_info_name,                 
                  res[i].patient_info_contact,
                  res[i].patient_info_email,
                  res[i].contact_info_name,
                  res[i].contact_info_address_1,
                  res[i].contact_info_address_2,
                  res[i].last_modified_date,
                 res[i].appointment_date_time,
                  res[i].preferred_date,
                  res[i].test_list,
                    res[i].rating,
                      res[i].type,
                        res[i].remarks,
                          res[i].delay_day,
                          res[i].patient_info_subscriber_id,
                            res[i].patient_info_patient_type
             ); }

            var heading = "";
            var heading1 = "'";
            var heading2 = "";
            var heading3 = "";

            byte[] filecontent = ExcelExportHelper.ExportExcel(datatable, heading, heading1, heading2, heading3, true);

            return File(filecontent, ExcelExportHelper.ExcelContentType, "Booking.xlsx");

        }




        [Route("physical/doctor/list")]
        public async Task<Object> PhysicalDoctor(int? pageNumber, string code, string name, string contact, string bookingid, string pendingid, string fromdate, string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] BookingStatus = _configuration["BookingStatus:booking_status"].ToString().Split(',');
            ViewBag.BookingStatus = BookingStatus;

            string[] PeningStatus = _configuration["PendingStatus:pending_status"].ToString().Split(',');
            ViewBag.PendingStatus = PeningStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.booking_status = bookingid;
            ViewBag.pending_status = pendingid;
            ViewBag.code = code;
            ViewBag.name = name;
            ViewBag.contact = contact;


            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }

            return View();
        }

        [Route("physical/doctor/data")]
        [HttpPost]
        public async Task<Object> PhysicalDoctorData(DashboardFilter filter)
        {
            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.booking_status = filter.booking_status;
            ViewBag.pending_status = filter.pending_status;
            ViewBag.code = filter.code;
            ViewBag.name = filter.name;
            ViewBag.contact = filter.contact;


            var res = await _testingRepository.getphysicalData(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }
            var pagedList = new StaticPagedList<PhysicalDoctorReturnType>(res.Count > 0 ? res : new List<PhysicalDoctorReturnType>(), filter.page, 10, Convert.ToInt32(totalcount));
            return PartialView("PhysicalDoctorData", pagedList);
        }


        [Route("physical/doctor/export")]
        [HttpGet]
        public async Task<Object> PhysicalDoctorExport(string fromdate, string? todate, string? code, string bookingstatus, string pendingstatus, string name, string contact)
        {
            var obj = new DashboardFilter();
            obj.booking_status = bookingstatus;
            obj.toDate = todate;
            obj.formDate = fromdate;           
            obj.code = code;           
            obj.name = name;        
            obj.contact = contact;   
            obj.pending_status = pendingstatus;
            obj.page = 1;
            obj.pageSize = 10000000;

            var res = await _testingRepository.getphysicalData(obj);
            DataTable datatable = new DataTable();
            datatable.Columns.Add("S.N");
            datatable.Columns.Add("Booking Code");
            datatable.Columns.Add("Booking Status");
            datatable.Columns.Add("Payment Status");
            datatable.Columns.Add("Payment Method");
            datatable.Columns.Add("Medical Institute");
            datatable.Columns.Add("Patient Info Name");
            datatable.Columns.Add("Patient Info Contact");
            datatable.Columns.Add("Patient Info Email");
            datatable.Columns.Add("Contact_info_name");
            datatable.Columns.Add("Contact_info_address_1");
            datatable.Columns.Add("Contact_info_address_2");
            datatable.Columns.Add("Last_modified_date");
            datatable.Columns.Add("Appointment_date_time");
            datatable.Columns.Add("Prefered date");
            datatable.Columns.Add("Doctor Name");
            datatable.Columns.Add("Doctor Gender");
            datatable.Columns.Add("Doctor NMC");
            datatable.Columns.Add("consulting_category_medical_concern");
            datatable.Columns.Add("consulting_category_speciality");
            datatable.Columns.Add("rating");
            datatable.Columns.Add("type");
            datatable.Columns.Add("remarks");
            datatable.Columns.Add("delay_day");
            datatable.Columns.Add("patient_info_subscriber_id");
            datatable.Columns.Add("patient_info_patient_type");
          
            for (int i = 0; i < res.Count; i++)
            {
                datatable.Rows.Add(
                  res[i].serial_number,
                  res[i].booking_code,
                  res[i].booking_status,
                  res[i].payment_status,
                  res[i].payment_method,
                  res[i].medical_institute,
                  res[i].patient_info_name,
                  res[i].patient_info_contact,
                  res[i].patient_info_email,
                  res[i].contact_info_name,
                  res[i].contact_info_address_1,
                  res[i].contact_info_address_2,
                  res[i].last_modified_date,
                 res[i].appointment_date_time,
                  res[i].preferred_date,
                  res[i].doctor_name,
                  res[i].doctor_gender,
                  res[i].doctor_nmc,
                   res[i].consulting_category_medical_concern,
                    res[i].consulting_category_speciality,
                    res[i].rating,
                      res[i].type,
                        res[i].remarks,
                          res[i].delay_day,
                          res[i].patient_info_subscriber_id,
                            res[i].patient_info_patient_type

             );
            }
               

        var heading = "";
            var heading1 = "'";
            var heading2 = "";
            var heading3 = "";

            byte[] filecontent = ExcelExportHelper.ExportExcel(datatable, heading, heading1, heading2, heading3, true);

            return File(filecontent, ExcelExportHelper.ExcelContentType, "PhysicalDoctor.xlsx");

        }



        [Route("tele/doctor/list")]
        public async Task<Object> TeleDoctor(int? pageNumber, string code, string name, string contact, string bookingid, string pendingid, string fromdate, string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] BookingStatus = _configuration["BookingStatus:booking_status"].ToString().Split(',');
            ViewBag.BookingStatus = BookingStatus;

            string[] PeningStatus = _configuration["PendingStatus:pending_status"].ToString().Split(',');
            ViewBag.PendingStatus = PeningStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.booking_status = bookingid;
            ViewBag.pending_status = pendingid;
            ViewBag.code = code;
            ViewBag.name = name;
            ViewBag.contact = contact;


            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }

            return View();
        }

        [Route("tele/doctor/data")]
        [HttpPost]
        public async Task<Object> TeleDoctorData(DashboardFilter filter)
        {
            
            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.booking_status = filter.booking_status;
            ViewBag.pending_status = filter.pending_status;
            ViewBag.code = filter.code;
            ViewBag.name = filter.name;
            ViewBag.contact = filter.contact;


            var res = await _testingRepository.getteleData(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }
            var pagedList = new StaticPagedList<TeleDoctorReturnType>(res.Count > 0 ? res : new List<TeleDoctorReturnType>(), filter.page, 10, Convert.ToInt32(totalcount));      
            return PartialView("TeleDoctorData", pagedList);

        }


        [Route("tele/doctor/export")]
        [HttpGet]
        public async Task<Object> TeleDoctorExport(string fromdate, string? todate, string? code, string bookingstatus, string pendingstatus, string name, string contact)
        {
            var obj = new DashboardFilter();
            obj.booking_status = bookingstatus;
            obj.toDate = todate;
            obj.formDate = fromdate;
            obj.formDate = fromdate;
            obj.code = code;
            obj.name = name;
            obj.contact = contact;
            obj.pending_status = pendingstatus;
            obj.page = 1;
            obj.pageSize = 10000000;

            var res = await _testingRepository.getteleData(obj);
            DataTable datatable = new DataTable();
            datatable.Columns.Add("S.N");
            datatable.Columns.Add("Booking Code");
            datatable.Columns.Add("Booking Status");
            datatable.Columns.Add("Payment Status");
            datatable.Columns.Add("Payment Method");
            datatable.Columns.Add("Medical Institute");
            datatable.Columns.Add("Patient Info Name");
            datatable.Columns.Add("Patient Info Contact");
            datatable.Columns.Add("Patient Info Email");
            datatable.Columns.Add("Contact_info_name");
            datatable.Columns.Add("Contact_info_address_1");
            datatable.Columns.Add("Contact_info_address_2");
            datatable.Columns.Add("Last_modified_date");
            datatable.Columns.Add("Appointment_date_time");
            datatable.Columns.Add("Prefered date");
            datatable.Columns.Add("Doctor Name");
            datatable.Columns.Add("Doctor Gender");
            datatable.Columns.Add("Doctor NMC");
            datatable.Columns.Add("consulting_category_medical_concern");
            datatable.Columns.Add("consulting_category_speciality");
            datatable.Columns.Add("rating");
            datatable.Columns.Add("type");
            datatable.Columns.Add("remarks");
            datatable.Columns.Add("delay_day");
            datatable.Columns.Add("patient_info_subscriber_id");
            datatable.Columns.Add("patient_info_patient_type");
            for (int i = 0; i < res.Count; i++)
            {
                datatable.Rows.Add(
                  res[i].serial_number,
                  res[i].booking_code,
                  res[i].booking_status,
                  res[i].payment_status,
                  res[i].payment_method,
                  res[i].medical_institute,
                  res[i].patient_info_name,
                  res[i].patient_info_contact,
                  res[i].patient_info_email,
                  res[i].contact_info_name,
                  res[i].contact_info_address_1,
                  res[i].contact_info_address_2,
                  res[i].last_modified_date,
                 res[i].appointment_date_time,
                  res[i].preferred_date,
                  res[i].doctor_name,
                  res[i].doctor_gender,
                  res[i].doctor_nmc,
                    res[i].consulting_category_medical_concern,
                    res[i].consulting_category_speciality,
                    res[i].rating,
                      res[i].type,
                        res[i].remarks,
                          res[i].delay_day,
                          res[i].patient_info_subscriber_id,
                            res[i].patient_info_patient_type
             );
            }

            var heading = "";
            var heading1 = "'";
            var heading2 = "";
            var heading3 = "";
            byte[] filecontent = ExcelExportHelper.ExportExcel(datatable, heading, heading1, heading2, heading3, true);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "TeleDoctor.xlsx");

        }




        [Route("virtual/doctor/list")]
        public async Task<Object> VirtualDoctor(int? pageNumber, string code, string name, string contact, string bookingid, string pendingid, string fromdate, string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] BookingStatus = _configuration["BookingStatus:booking_status"].ToString().Split(',');
            ViewBag.BookingStatus = BookingStatus;

            string[] PeningStatus = _configuration["PendingStatus:pending_status"].ToString().Split(',');
            ViewBag.PendingStatus = PeningStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.booking_status = bookingid;
            ViewBag.pending_status = pendingid;
            ViewBag.code = code;
            ViewBag.name = name;
            ViewBag.contact = contact;


            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }

            return View();
        }

        [Route("virtual/doctor/data")]
        [HttpPost]
        public async Task<Object> VirtualDoctorData(DashboardFilter filter)
        {

            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.booking_status = filter.booking_status;
            ViewBag.pending_status = filter.pending_status;
            ViewBag.code = filter.code;
            ViewBag.name = filter.name;
            ViewBag.contact = filter.contact;


            var res = await _testingRepository.getvirtualData(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }
            var pagedList = new StaticPagedList<VirtualDoctorReturnType>(res.Count > 0 ? res : new List<VirtualDoctorReturnType>(), filter.page, 10, Convert.ToInt32(totalcount));
            return PartialView("VirtualDoctorData", pagedList);

        }


        [Route("virtual/doctor/export")]
        [HttpGet]
        public async Task<Object> VirtualDoctorExport(string fromdate, string? todate, string? code, string bookingstatus, string pendingstatus, string name, string contact)
        {
            var obj = new DashboardFilter();
            obj.booking_status = bookingstatus;
            obj.toDate = todate;
            obj.formDate = fromdate;
            obj.formDate = fromdate;
            obj.code = code;
            obj.name = name;
            obj.contact = contact;
            obj.pending_status = pendingstatus;
            obj.page = 1;
            obj.pageSize = 10000000;

            var res = await _testingRepository.getvirtualData(obj);
            DataTable datatable = new DataTable();
            datatable.Columns.Add("S.N");
            datatable.Columns.Add("Booking Code");
            datatable.Columns.Add("Booking Status");
            datatable.Columns.Add("Payment Status");
            datatable.Columns.Add("Payment Method");
            datatable.Columns.Add("Medical Institute");
            datatable.Columns.Add("Patient Info Name");
            datatable.Columns.Add("Patient Info Contact");
            datatable.Columns.Add("Patient Info Email");
            datatable.Columns.Add("Contact_info_name");
            datatable.Columns.Add("Contact_info_address_1");
            datatable.Columns.Add("Contact_info_address_2");
            datatable.Columns.Add("Last_modified_date");
            datatable.Columns.Add("Appointment_date_time");
            datatable.Columns.Add("Prefered date");
            datatable.Columns.Add("Doctor Name");
            datatable.Columns.Add("Doctor Gender");
            datatable.Columns.Add("Doctor NMC");
            datatable.Columns.Add("consulting_category_medical_concern");
            datatable.Columns.Add("consulting_category_speciality");
            datatable.Columns.Add("rating");
            datatable.Columns.Add("type");
            datatable.Columns.Add("remarks");
            datatable.Columns.Add("delay_day");
            datatable.Columns.Add("patient_info_subscriber_id");
            datatable.Columns.Add("patient_info_patient_type");
            for (int i = 0; i < res.Count; i++)
            {
                datatable.Rows.Add(
                  res[i].serial_number,
                  res[i].booking_code,
                  res[i].booking_status,
                  res[i].payment_status,
                  res[i].payment_method,
                  res[i].medical_institute,
                  res[i].patient_info_name,
                  res[i].patient_info_contact,
                  res[i].patient_info_email,
                  res[i].contact_info_name,
                  res[i].contact_info_address_1,
                  res[i].contact_info_address_2,
                  res[i].last_modified_date,
                 res[i].appointment_date_time,
                  res[i].preferred_date,
                  res[i].doctor_name,
                  res[i].doctor_gender,
                  res[i].doctor_nmc,
                    res[i].consulting_category_medical_concern,
                    res[i].consulting_category_speciality,
                    res[i].rating,
                      res[i].type,
                        res[i].remarks,
                          res[i].delay_day,
                          res[i].patient_info_subscriber_id,
                            res[i].patient_info_patient_type
             );
            }

            var heading = "";
            var heading1 = "'";
            var heading2 = "";
            var heading3 = "";
            byte[] filecontent = ExcelExportHelper.ExportExcel(datatable, heading, heading1, heading2, heading3, true);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "VirtualDoctor.xlsx");

        }






        [Route("international/doctor/list")]
        public async Task<Object> InternationalDoctor(int? pageNumber, string code, string name, string contact, string bookingid, string pendingid, string fromdate, string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] BookingStatus = _configuration["BookingStatus:booking_status"].ToString().Split(',');
            ViewBag.BookingStatus = BookingStatus;

            string[] PeningStatus = _configuration["PendingStatus:pending_status"].ToString().Split(',');
            ViewBag.PendingStatus = PeningStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.booking_status = bookingid;
            ViewBag.pending_status = pendingid;
            ViewBag.code = code;
            ViewBag.name = name;
            ViewBag.contact = contact;


            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }

            return View();
        }

        [Route("international/doctor/data")]
        [HttpPost]
        public async Task<Object> InternationalDoctorData(DashboardFilter filter)
        {

            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.booking_status = filter.booking_status;
            ViewBag.pending_status = filter.pending_status;
            ViewBag.code = filter.code;
            ViewBag.name = filter.name;
            ViewBag.contact = filter.contact;


            var res = await _testingRepository.getinternationalData(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }
            var pagedList = new StaticPagedList<InternationalDoctorReturnType>(res.Count > 0 ? res : new List<InternationalDoctorReturnType>(), filter.page, 10, Convert.ToInt32(totalcount));
            return PartialView("InternationalDoctorData", pagedList);

        }


        [Route("international/doctor/export")]
        [HttpGet]
        public async Task<Object> InternationalDoctorExport(string fromdate, string? todate, string? code, string bookingstatus, string pendingstatus, string name, string contact)
        {
            var obj = new DashboardFilter();
            obj.booking_status = bookingstatus;
            obj.toDate = todate;
            obj.formDate = fromdate;
            obj.formDate = fromdate;
            obj.code = code;
            obj.name = name;
            obj.contact = contact;
            obj.pending_status = pendingstatus;
            obj.page = 1;
            obj.pageSize = 10000000;

            var res = await _testingRepository.getinternationalData(obj);
            DataTable datatable = new DataTable();
            datatable.Columns.Add("S.N");
            datatable.Columns.Add("Booking Code");
            datatable.Columns.Add("Booking Status");
            datatable.Columns.Add("Payment Status");
            datatable.Columns.Add("Payment Method");
            datatable.Columns.Add("Medical Institute");
            datatable.Columns.Add("Patient Info Name");
            datatable.Columns.Add("Patient Info Contact");
            datatable.Columns.Add("Patient Info Email");
            datatable.Columns.Add("Contact_info_name");
            datatable.Columns.Add("Contact_info_address_1");
            datatable.Columns.Add("Contact_info_address_2");
            datatable.Columns.Add("Last_modified_date");
            datatable.Columns.Add("Appointment_date_time");
            datatable.Columns.Add("Prefered date");
            datatable.Columns.Add("Doctor Name");
            datatable.Columns.Add("Doctor Gender");
            datatable.Columns.Add("Doctor NMC");
            datatable.Columns.Add("consulting_category_medical_concern");
            datatable.Columns.Add("consulting_category_speciality");
            datatable.Columns.Add("rating");
            datatable.Columns.Add("type");
            datatable.Columns.Add("remarks");
            datatable.Columns.Add("delay_day");
            datatable.Columns.Add("patient_info_subscriber_id");
            datatable.Columns.Add("patient_info_patient_type");
            for (int i = 0; i < res.Count; i++)
            {
                datatable.Rows.Add(
                  res[i].serial_number,
                  res[i].booking_code,
                  res[i].booking_status,
                  res[i].payment_status,
                  res[i].payment_method,
                  res[i].medical_institute,
                  res[i].patient_info_name,
                  res[i].patient_info_contact,
                  res[i].patient_info_email,
                  res[i].contact_info_name,
                  res[i].contact_info_address_1,
                  res[i].contact_info_address_2,
                  res[i].last_modified_date,
                 res[i].appointment_date_time,
                  res[i].preferred_date,
                  res[i].doctor_name,
                  res[i].doctor_gender,
                  res[i].doctor_nmc,
                    res[i].consulting_category_medical_concern,
                    res[i].consulting_category_speciality,
                    res[i].rating,
                      res[i].type,
                        res[i].remarks,
                          res[i].delay_day,
                          res[i].patient_info_subscriber_id,
                            res[i].patient_info_patient_type
             );
            }

            var heading = "";
            var heading1 = "'";
            var heading2 = "";
            var heading3 = "";
            byte[] filecontent = ExcelExportHelper.ExportExcel(datatable, heading, heading1, heading2, heading3, true);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InternationalDoctor.xlsx");

        }





        [Route("order/list")]
        public async Task<Object> OrderList(int? pageNumber, string order,string status, string fromdate, string todate)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string[] OrderStatus = _configuration["Orderstatus:status"].ToString().Split(',');
            ViewBag.Ordersta= OrderStatus;

            ViewBag.formDate = fromdate;
            ViewBag.toDate = todate;
            ViewBag.order = order;
            ViewBag.status = status;

            if (pageNumber == null)
            {
                pageNumber = 1;
            }

            if (pageNumber != null)
            {
                ViewBag.pageNumber = pageNumber;
            }

            return View();
        }

        [Route("order/list/data")]
        [HttpPost]
        public async Task<Object> OrderListData(DashboardFilterOrder filter)
        {

            ViewBag.formDate = filter.formDate;
            ViewBag.toDate = filter.toDate;
            ViewBag.staus = filter.status;
            ViewBag.order = filter.order;

            var res = await _testingRepository.getorderList(filter);

            long totalcount = 0;
            if (res.Count > 0)
            {
                var totalCount = Convert.ToInt64(res.FirstOrDefault().totalcount);
                totalcount = totalCount;
            }
            var pagedList = new StaticPagedList<Order>(res.Count > 0 ? res : new List<Order>(), filter.page, 10, Convert.ToInt32(totalcount));
            return PartialView("OrderListData", pagedList);

        }






        [AllowAnonymous]
        [Route("getbookingstatus")]

        public async Task<IActionResult> GetBookingStatus()
        {
            var bookingstatus = await _testingRepository.getbookingstatus();         

            return Ok(bookingstatus);
        
        }


        [AllowAnonymous]
        [Route("getpaymentstatus")]
       
        public async Task<IActionResult> getPaymentstatus()
        {
            var bookingstatus = await _testingRepository.getpaymentstatus();
            return Ok(bookingstatus);
        }

    }
}
