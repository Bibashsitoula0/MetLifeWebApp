using Hangfire;
using MetLifeInsurance.BackgroundJobModel;  
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MetLifeInsurance.BackgoundService
{
    public class BackgroundServiceJob
    {
        private readonly ILogger<BackgroundServiceJob> _logger;
        public readonly IConfiguration _configuration;

        public BackgroundServiceJob(IConfiguration configuration, ILogger<BackgroundServiceJob> logger)
        {
            _configuration = configuration;

            _logger = logger;
        }

        private DateTime _lastSyncDate = DateTime.Now;
        private int Limit => Convert.ToInt32(_configuration["Limit"]);
        private int ThreadTime => Convert.ToInt32(_configuration["ThreadTime"]);

        /// <summary>
        /// api key
        /// </summary>
        private string ApiKey => _configuration["ApiKey"];

        /// <summary>
        /// log file name
        /// </summary>
        private string BookingLog => _configuration["BookingLog"];
        private string PhysicalDoctorLog => _configuration["PhysicalDoctorLog"];
        private string TeleDoctorLog => _configuration["TeleDoctorLog"];
        private string InternationalDoctorLog => _configuration["InternationalDoctorLog"];
        private string OrderLog => _configuration["OrderLog"];
        public string VirtualDoctorLog => _configuration["VirtualDoctorLog"];


        /// <summary>
        /// userName Password
        /// </summary>
        public string username => _configuration["OrderUserName"];
        public string password => _configuration["OrderPassword"];


        /// <summary>
        /// api url 
        /// </summary>
        /// 
        public string bookingurl => _configuration["BookingApiUrl"];
        public string physical_Doctor_Url => _configuration["PhysicalDoctorApiUrl"];
        public string tele_Doctor_Url => _configuration["TeleDoctorApiUrl"];
        public string virtual_doctor_url => _configuration["VirtualDoctorUrl"];
        public string international_doctor_url => _configuration["InternationalDoctorUrl"];
        public string Order_url => _configuration["OrderApiUrl"];

        public string connectionString => _configuration["ConnectionStrings:connection"];

        private readonly string _logDirectory = "Log"; // Directory name for the log files


        public void ScheduleApiCall()
        {
            RecurringJob.AddOrUpdate("api-data-job", () => SetBackgroundData(), Cron.MinuteInterval(2));           
        }

        public async Task SetBackgroundData()
        {
            try
            {
                string analyticsData = await GetAnalyticsData(_lastSyncDate);
                JObject data = JObject.Parse(analyticsData);
                LogMessage($"Received Booking data: {data}");
                BookingInsert(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());              
            }
            try
            {

                string physicaldoctordata = await GetPhysicalDoctorData(_lastSyncDate);
                JObject phydoctor = JObject.Parse(physicaldoctordata);
                LogMessagePhysicalDoctor($"Received Physical Doctor  data: {phydoctor}");
                PhysciInser(phydoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
               
            }
            try
            {
                // get the data of tele doctor
                string teledoctordata = await GetTeleDoctorData(_lastSyncDate);
                JObject teledoctor = JObject.Parse(teledoctordata);
                TeleDoctor teleDoctorObject = teledoctor.ToObject<TeleDoctor>();
                LogMessageTeleDoctor($"Received Tele Doctor data: {teledoctor}");
                TeleDoctors(teledoctor);
               }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
               
            }
            try
            {
                string virtualdoctordata = await GetVirtualDoctorData(_lastSyncDate);
                JObject virtualdoctor = JObject.Parse(virtualdoctordata);
                LogMessageVitrualDoctor($"Received Virtual Doctor data: {virtualdoctor}");
                VirtualDoctors(virtualdoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            try
            {
                string internationaldoctordata = await GetInternationalDoctorData(_lastSyncDate);
                JObject internationaldoctor = JObject.Parse(internationaldoctordata);
                LogMessageInternationalDoctor($"Received International Doctor data: {internationaldoctor}");
                GetInternationalDoctor(internationaldoctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
              
            }
            try
            {
                var orderssss = await GetOrderData(_lastSyncDate);
                LogMessageOrderData($"Received Order data: {orderssss.ToList()}");
                OrderInsert(orderssss);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());              

            }

        }


        #region Api called

        // called the booking api
        public async Task<string> GetAnalyticsData(DateTime lastSyncDate)
        {
            try
            {
                _logger.LogInformation("Started calling booking Api success");
                HttpClient _httpClient = new HttpClient();
                _lastSyncDate = DateTime.Now;
                _httpClient.BaseAddress = new Uri(bookingurl);
                _httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{bookingurl}/analytics?date={lastSyncDate.ToString("yyyy-MM-dd HH:mm:ss")}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sucess calling booking Api");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        // called the PhysicalDoctor api
        public async Task<string> GetPhysicalDoctorData(DateTime lastSyncDate)
        {
            try
            {
                _logger.LogInformation("Started calling Physical Doctor Api success");
                HttpClient _httpClient = new HttpClient();

                _lastSyncDate = DateTime.Now;
                _httpClient.BaseAddress = new Uri(physical_Doctor_Url);
                _httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{physical_Doctor_Url}/analytics?date={lastSyncDate.ToString("yyyy-MM-dd HH:mm:ss")}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sucess calling PhysicalDoctor Api");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }


        }

        // called the Tele Doctor api
        public async Task<string> GetTeleDoctorData(DateTime lastSyncDate)
        {
            try
            {
                _logger.LogInformation("Started calling Tele Doctor Api success");
                HttpClient _httpClient = new HttpClient();

                _lastSyncDate = DateTime.Now;
                _httpClient.BaseAddress = new Uri(tele_Doctor_Url);
                _httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{tele_Doctor_Url}/analytics?date={lastSyncDate.ToString("yyyy-MM-dd HH:mm:ss")}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sucess calling Tele Doctor Api");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

        }

        // called the VirtualDoctor api
        public async Task<string> GetVirtualDoctorData(DateTime lastSyncDate)
        {
            try
            {

                _logger.LogInformation("Started calling Virtual Doctor Api success");
                HttpClient _httpClient = new HttpClient();
                _lastSyncDate = DateTime.Now;
                string beforeDate = _lastSyncDate.ToString("yyyy-MM-ddTHH:mm:ss");
                _httpClient.BaseAddress = new Uri(virtual_doctor_url);
                _httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{virtual_doctor_url}/analytics?date={beforeDate}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sucess calling Virtual Doctor  Api");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }


        }

        // called the International Doctor Data api
        public async Task<string> GetInternationalDoctorData(DateTime lastSyncDate)
        {
            try
            {

                _logger.LogInformation("Started calling International Doctor Api success");
                HttpClient _httpClient = new HttpClient();
                _lastSyncDate = DateTime.Now;
                string beforeDate = _lastSyncDate.ToString("yyyy-MM-ddTHH:mm:ss");
                _httpClient.BaseAddress = new Uri(international_doctor_url);
                _httpClient.DefaultRequestHeaders.Add("apiKey", ApiKey);
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string url = $"{international_doctor_url}/analytics?date={beforeDate}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Sucess calling International Doctor Api");
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }


        }

        // called the Order Data api
        public async Task<List<Order>> GetOrderData(DateTime lastSyncDate)
        {
            try
            {

                _logger.LogInformation("Started calling Order Doctor Api success");
                HttpClient _httpClient = new HttpClient();
                int perPage = 10;
                int page = 1;
                List<Order> allOrders = new List<Order>();

                string responseBody = string.Empty;
                int totalPageCount = 0;

                _httpClient.BaseAddress = new Uri(Order_url);
                var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
                _httpClient.DefaultRequestHeaders.Authorization = authValue;

                do
                {
                    string currentPageUrl = $"{Order_url}?page={page}&per_page={perPage}";

                    HttpResponseMessage response = await _httpClient.GetAsync(currentPageUrl);
                    response.EnsureSuccessStatusCode();

                    if (response.Headers.TryGetValues("X-WP-TotalPages", out var totalPagesValues))
                    {
                        string totalPages = totalPagesValues.FirstOrDefault();
                        totalPageCount = int.Parse(totalPages);

                        string pageContent = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(pageContent);

                        foreach (JToken token in jsonArray)
                        {
                            Order order = token.ToObject<Order>();
                            allOrders.Add(order);
                        }
                    }

                    page++;

                } while (page <= totalPageCount);
                _logger.LogInformation("Sucess calling Order Api");
                return allOrders;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }

        }


        #endregion

        #region logging

        // print the response in txt file for booking 
        private void LogMessage(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for booking success");
                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
               
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);             
                string logFilePath = Path.Combine(logDirectoryPath, BookingLog);


                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }

        // print the data in txt file for PhysicalDoctor
        private void LogMessagePhysicalDoctor(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for PhysicalDoctor success");

                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);
                string logFilePath = Path.Combine(logDirectoryPath, PhysicalDoctorLog);

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }

        // print the data in txt file for TeleDoctor
        private void LogMessageTeleDoctor(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for TeleDoctor success");

                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);
                string logFilePath = Path.Combine(logDirectoryPath, TeleDoctorLog);


                
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }
        // print the data in txt file for VirtualDoctor
        private void LogMessageVitrualDoctor(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for VirtualDoctor success");

                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);
                string logFilePath = Path.Combine(logDirectoryPath, VirtualDoctorLog);

               
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }

        // print the data in txt file for internationalDoctor
        private void LogMessageInternationalDoctor(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for internationalDoctor success");

                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);
                string logFilePath = Path.Combine(logDirectoryPath, InternationalDoctorLog);

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }
        // print the data in txt file for order
        private void LogMessageOrderData(string message)
        {
            try
            {
                _logger.LogInformation("Creating .txt file for order success");

                string projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Log");
                string logDirectoryPath = Path.Combine(projectDirectory, _logDirectory);
                Directory.CreateDirectory(logDirectoryPath);
                string logFilePath = Path.Combine(logDirectoryPath, OrderLog);

                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

            }

        }
        #endregion






        public void BookingInsert(JObject data)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in data["bookings"])
                {
                    var appointmentdate = (string)item["appointment_date_time"] ?? " ";
                    var apointmentstring = Convert.ToString(appointmentdate);

                    Booking book = new Booking()
                    {
                        booking_status = (string)item["booking_status"] ?? "",
                        booking_code = (string)item["booking_code"] ?? "",
                        payment_status = (string)item["payment_status"] ?? "",
                        payment_method = (string)item["payment_method"] ?? "",
                        medical_institute = (string)item["medical_institute"] ?? "",
                        contact_info_address_1 = (string)item["contact_info"]["address_1"] ?? "",
                        contact_info_address_2 = (string)item["contact_info"]["address_2"] ?? "",
                        contact_info_contact = (string)item["contact_info"]["contact"] ?? "",
                        contact_info_is_patient = (bool)item["contact_info"]["is_patient"],
                        contact_info_name = (string)item["contact_info"]["name"] ?? "",
                        patient_info_contact = (string)item["patient_info"]["contact"] ?? "",
                        patient_info_email = (string)item["patient_info"]["email"] ?? "",
                        patient_info_name = (string)item["patient_info"]["name"] ?? "",
                        appointment_date_time = apointmentstring ?? "",
                        last_modified_date = (DateTime?)item["last_modified_date"],
                        preferred_date = (DateTime?)item["preferred_date"],
                        created_date = (DateTime?)item["created_date"],
                        patient_info_subscriber_id = (string)item["patient_info"]["subscriber_id"] ?? "",
                        patient_info_patient_type = (string)item["patient_info"]["patient_type"] ?? "",
                        delay_day = (int)item["delay_day"]
                    };


                    //get the bookingid by booking_code 
                    string getbyid = "Select bookingid from booking where booking_code=@booking_code";
                    SqlCommand getcommandbyid = new SqlCommand(getbyid, connection);
                    getcommandbyid.Parameters.AddWithValue("@booking_code", book.booking_code);
                    SqlDataReader reader = getcommandbyid.ExecuteReader();
                    while (reader.Read())
                    {
                        long bookingID = reader.GetInt64(0);

                        // delete the booking_detail by booking Id

                        string deleteboookingdetail = "delete from bookingdetail where bookingid =@bookingID ";
                        SqlCommand bookingdelete = new SqlCommand(deleteboookingdetail, connection);
                        bookingdelete.Parameters.AddWithValue("@bookingID", bookingID);
                        int delete_booking_details = bookingdelete.ExecuteNonQuery();
                    }

                    // delete the booking
                    string delete = "delete from booking where booking_code = @booking_code";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@booking_code", book.booking_code);
                    int result = deletecommand.ExecuteNonQuery();


                    // insert the booking data
                    string query = "INSERT INTO booking (booking_status, booking_code, payment_status, payment_method, medical_institute, contact_info_address_1, contact_info_address_2, contact_info_contact, contact_info_is_patient, contact_info_name, patient_info_contact, patient_info_email, patient_info_name, appointment_date_time, last_modified_date, preferred_date,created_date,patient_info_subscriber_id,patient_info_patient_type,delay_day) VALUES (@booking_status, @booking_code, @payment_status, @payment_method, @medical_institute, @contact_info_address_1, @contact_info_address_2, @contact_info_contact, @contact_info_is_patient, @contact_info_name, @patient_info_contact, @patient_info_email, @patient_info_name, @appointment_date_time, @last_modified_date, @preferred_date,@created_date,@patient_info_subscriber_id,@patient_info_patient_type,@delay_day); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@booking_status", book.booking_status);
                    command.Parameters.AddWithValue("@booking_code", book.booking_code);
                    command.Parameters.AddWithValue("@payment_status", book.payment_status);
                    command.Parameters.AddWithValue("@payment_method", book.payment_method);
                    command.Parameters.AddWithValue("@medical_institute", book.medical_institute);
                    command.Parameters.AddWithValue("@contact_info_address_1", book.contact_info_address_1);
                    command.Parameters.AddWithValue("@contact_info_address_2", book.contact_info_address_2);
                    command.Parameters.AddWithValue("@contact_info_contact", book.contact_info_contact);
                    command.Parameters.AddWithValue("@contact_info_is_patient", book.contact_info_is_patient);
                    command.Parameters.AddWithValue("@contact_info_name", book.contact_info_name);
                    command.Parameters.AddWithValue("@patient_info_contact", book.patient_info_contact);
                    command.Parameters.AddWithValue("@patient_info_email", book.patient_info_email);
                    command.Parameters.AddWithValue("@patient_info_name", book.patient_info_name);
                    command.Parameters.AddWithValue("@appointment_date_time", book.appointment_date_time);
                    command.Parameters.AddWithValue("@last_modified_date", book.last_modified_date);
                    command.Parameters.AddWithValue("@preferred_date", book.preferred_date);
                    command.Parameters.AddWithValue("@created_date", book.created_date);
                    command.Parameters.AddWithValue("@patient_info_subscriber_id", book.patient_info_subscriber_id);
                    command.Parameters.AddWithValue("@patient_info_patient_type", book.patient_info_patient_type);
                    command.Parameters.AddWithValue("@delay_day", book.delay_day);
                    long newBookingId = Convert.ToInt64(command.ExecuteScalar());
                    book.bookingid = newBookingId;

                    // Insert the data in  boooking_details table 
                    JArray medicalTests = (JArray)item["medical_tests"];
                    foreach (JObject test in medicalTests)
                    {
                        string testName = (string)test["test"];
                        decimal testCost = (decimal)test["cost"];
                        BookingDetail op = new BookingDetail();
                        op.cost = testCost;
                        op.test = testName;
                        op.bookingid = newBookingId;

                        string queries = "INSERT INTO bookingdetail (bookingid, test, cost) VALUES (@bookingid,@test,@cost ); SELECT SCOPE_IDENTITY();";
                        SqlCommand commands = new SqlCommand(queries, connection);
                        commands.Parameters.AddWithValue("@bookingid", op.bookingid);
                        commands.Parameters.AddWithValue("@test", op.test);
                        commands.Parameters.AddWithValue("@cost", op.cost);
                        long newBookingIds = Convert.ToInt64(commands.ExecuteScalar());
                        op.bookingdetailid = newBookingIds;


                    }
                }
            }

        }

        public void PhysciInser(JObject phydoctor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in phydoctor["bookings"])
                {

                    var appointmentdate = (string)item["appointment_date_time"] ?? " ";
                    var apointmentstring = Convert.ToString(appointmentdate);

                    JToken consultingCategoryToken = item["consulting_category"];
                    string consulting_category_medical_concern = null;
                    string consulting_category_speciality = null;

                    if (consultingCategoryToken is JObject consultingCategoryObject)
                    {
                        consulting_category_medical_concern = (string)consultingCategoryObject["medical_concern"];
                        consulting_category_speciality = (string)consultingCategoryObject["speciality"];
                    }
                    else if (consultingCategoryToken is JValue consultingCategoryValue)
                    {
                        // Handle the case when the consulting_category is a single value (e.g., null, string, number, boolean)
                        consulting_category_medical_concern = consultingCategoryValue.Value?.ToString();
                        consulting_category_speciality = null; // Handle as needed
                    }

                    PhysicalDoctor physicaldoctor = new PhysicalDoctor()
                    {
                        booking_status = (string)item["booking_status"] ?? "",
                        booking_code = (string)item["booking_code"] ?? "",
                        payment_status = (string)item["payment_status"] ?? "",
                        payment_method = (string)item["payment_method"] ?? "",
                        medical_institute = (string)item["medical_institute"] ?? "",
                        doctor_name = (string)item["doctor"]["name"] ?? "",
                        doctor_nmc = (string)item["doctor"]["nmc"] ?? "",
                        doctor_gender = (string)item["doctor"]["gender"] ?? "",
                        contact_info_address_1 = (string)item["contact_info"]["address_1"] ?? "",
                        contact_info_address_2 = (string)item["contact_info"]["address_2"] ?? "",
                        contact_info_contact = (string)item["contact_info"]["contact"] ?? "",
                        contact_info_is_patient = (bool)item["contact_info"]["is_patient"],
                        contact_info_name = (string)item["contact_info"]["name"] ?? "",
                        patient_info_contact = (string)item["patient_info"]["contact"] ?? "",
                        patient_info_email = (string)item["patient_info"]["email"] ?? "",
                        patient_info_name = (string)item["patient_info"]["name"] ?? "",
                        appointment_date_time = apointmentstring ?? "",
                        last_modified_date = (DateTime?)item["last_modified_date"],
                        preferred_date = (DateTime?)item["preferred_date"],
                        created_date = (DateTime?)item["created_date"],
                        consulting_category_medical_concern = consulting_category_medical_concern ?? "",
                        consulting_category_speciality = consulting_category_speciality ?? "",
                        patient_info_subscriber_id = (string)item["patient_info"]["subscriber_id"] ?? "",
                        patient_info_patient_type = (string)item["patient_info"]["patient_type"] ?? "",
                        delay_day = (int)item["delay_day"]

                    };

                    // delete the physicaldoctor data
                    string delete = "delete from physicaldoctor where booking_code = @booking_code";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@booking_code", physicaldoctor.booking_code);
                    int result = deletecommand.ExecuteNonQuery();


                    // insert the physicaldoctor data
                    string query = "INSERT INTO physicaldoctor (booking_status, booking_code, payment_status, payment_method, medical_institute,doctor_name,doctor_nmc,doctor_gender, contact_info_address_1, contact_info_address_2, contact_info_contact, contact_info_is_patient, contact_info_name, patient_info_contact, patient_info_email, patient_info_name, appointment_date_time, last_modified_date, preferred_date,created_date,consulting_category_medical_concern,consulting_category_speciality,patient_info_subscriber_id,patient_info_patient_type,delay_day) VALUES (@booking_status, @booking_code, @payment_status, @payment_method, @medical_institute,@doctor_name,@doctor_nmc,@doctor_gender, @contact_info_address_1, @contact_info_address_2, @contact_info_contact, @contact_info_is_patient, @contact_info_name, @patient_info_contact, @patient_info_email, @patient_info_name, @appointment_date_time, @last_modified_date, @preferred_date,@created_date,@consulting_category_medical_concern,@consulting_category_speciality,@patient_info_subscriber_id,@patient_info_patient_type,@delay_day); SELECT SCOPE_IDENTITY();";
                    SqlCommand commands = new SqlCommand(query, connection);
                    commands.Parameters.AddWithValue("@booking_status", physicaldoctor.booking_status);
                    commands.Parameters.AddWithValue("@booking_code", physicaldoctor.booking_code);
                    commands.Parameters.AddWithValue("@payment_status", physicaldoctor.payment_status);
                    commands.Parameters.AddWithValue("@payment_method", physicaldoctor.payment_method);
                    commands.Parameters.AddWithValue("@medical_institute", physicaldoctor.medical_institute);
                    commands.Parameters.AddWithValue("@doctor_name", physicaldoctor.doctor_name);
                    commands.Parameters.AddWithValue("@doctor_nmc", physicaldoctor.doctor_nmc);
                    commands.Parameters.AddWithValue("@doctor_gender", physicaldoctor.doctor_gender);
                    commands.Parameters.AddWithValue("@contact_info_address_1", physicaldoctor.contact_info_address_1);
                    commands.Parameters.AddWithValue("@contact_info_address_2", physicaldoctor.contact_info_address_2);
                    commands.Parameters.AddWithValue("@contact_info_contact", physicaldoctor.contact_info_contact);
                    commands.Parameters.AddWithValue("@contact_info_is_patient", physicaldoctor.contact_info_is_patient);
                    commands.Parameters.AddWithValue("@contact_info_name", physicaldoctor.contact_info_name);
                    commands.Parameters.AddWithValue("@patient_info_contact", physicaldoctor.patient_info_contact);
                    commands.Parameters.AddWithValue("@patient_info_email", physicaldoctor.patient_info_email);
                    commands.Parameters.AddWithValue("@patient_info_name", physicaldoctor.patient_info_name);
                    commands.Parameters.AddWithValue("@appointment_date_time", physicaldoctor.appointment_date_time);
                    commands.Parameters.AddWithValue("@last_modified_date", physicaldoctor.last_modified_date);
                    commands.Parameters.AddWithValue("@preferred_date", physicaldoctor.preferred_date);
                    commands.Parameters.AddWithValue("@created_date", physicaldoctor.created_date);
                    commands.Parameters.AddWithValue("@consulting_category_medical_concern", physicaldoctor.consulting_category_medical_concern);
                    commands.Parameters.AddWithValue("@consulting_category_speciality", physicaldoctor.consulting_category_speciality);
                    commands.Parameters.AddWithValue("@patient_info_subscriber_id", physicaldoctor.patient_info_subscriber_id);
                    commands.Parameters.AddWithValue("@patient_info_patient_type", physicaldoctor.patient_info_patient_type);
                    commands.Parameters.AddWithValue("@delay_day", physicaldoctor.delay_day);
                    long newphydoctorId = Convert.ToInt64(commands.ExecuteScalar());
                    physicaldoctor.physical_doctor_id = newphydoctorId;
                }

            }
        }

        public void TeleDoctors(JObject teledoctor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var item in teledoctor["bookings"])
                {
                    var appointmentdate = (string)item["appointment_date_time"] ?? " ";
                    var apointmentstring = Convert.ToString(appointmentdate);

                    JToken consultingCategoryToken = item["consulting_category"];
                    string consulting_category_medical_concern = null;
                    string consulting_category_speciality = null;

                    if (consultingCategoryToken is JObject consultingCategoryObject)
                    {
                        consulting_category_medical_concern = (string)consultingCategoryObject["medical_concern"];
                        consulting_category_speciality = (string)consultingCategoryObject["speciality"];
                    }
                    else if (consultingCategoryToken is JValue consultingCategoryValue)
                    {
                        // Handle the case when the consulting_category is a single value (e.g., null, string, number, boolean)
                        consulting_category_medical_concern = consultingCategoryValue.Value?.ToString();
                        consulting_category_speciality = null; // Handle as needed
                    }

                    TeleDoctor teleedoctor = new TeleDoctor()
                    {
                        booking_status = (string)item["booking_status"] ?? "",
                        booking_code = (string)item["booking_code"] ?? "",
                        payment_status = (string)item["payment_status"] ?? "",
                        payment_method = (string)item["payment_method"] ?? "",
                        medical_institute = (string)item["medical_institute"] ?? "",
                        doctor_name = (string)item["doctor"]?["name"] ?? "",
                        doctor_nmc = (string)item["doctor"]?["nmc"] ?? "",
                        doctor_gender = (string)item["doctor"]?["gender"] ?? "",
                        contact_info_address_1 = (string)item["contact_info"]["address_1"] ?? "",
                        contact_info_address_2 = (string)item["contact_info"]["address_2"] ?? "",
                        contact_info_contact = (string)item["contact_info"]["contact"] ?? "",
                        contact_info_is_patient = (bool)item["contact_info"]["is_patient"],
                        contact_info_name = (string)item["contact_info"]["name"] ?? "",
                        patient_info_contact = (string)item["patient_info"]["contact"] ?? "",
                        patient_info_email = (string)item["patient_info"]["email"] ?? "",
                        patient_info_name = (string)item["patient_info"]["name"] ?? "",
                        appointment_date_time = apointmentstring ?? "",
                        last_modified_date = (DateTime?)item["last_modified_date"],
                        preferred_date = (DateTime?)item["preferred_date"],
                        created_date = (DateTime?)item["created_date"],
                        consulting_category_medical_concern = consulting_category_medical_concern ?? "",
                        consulting_category_speciality = consulting_category_speciality ?? "",
                        patient_info_subscriber_id = (string)item["patient_info"]["subscriber_id"] ?? "",
                        patient_info_patient_type = (string)item["patient_info"]["patient_type"] ?? "",
                        delay_day = (int)item["delay_day"]
                    };


                    // delete the teledoctor data
                    string delete = "delete from teledoctor where booking_code = @booking_code";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@booking_code", teleedoctor.booking_code);
                    int result = deletecommand.ExecuteNonQuery();

                    // insert the teledoctor data
                    string query = "INSERT INTO teledoctor (booking_status, booking_code, payment_status, payment_method, medical_institute,doctor_name,doctor_nmc,doctor_gender, contact_info_address_1, contact_info_address_2, contact_info_contact, contact_info_is_patient, contact_info_name, patient_info_contact, patient_info_email, patient_info_name, appointment_date_time, last_modified_date, preferred_date,created_date,consulting_category_medical_concern,consulting_category_speciality,patient_info_subscriber_id,patient_info_patient_type,delay_day) VALUES (@booking_status, @booking_code, @payment_status, @payment_method, @medical_institute,@doctor_name,@doctor_nmc,@doctor_gender, @contact_info_address_1, @contact_info_address_2, @contact_info_contact, @contact_info_is_patient, @contact_info_name, @patient_info_contact, @patient_info_email, @patient_info_name, @appointment_date_time, @last_modified_date, @preferred_date,@created_date,@consulting_category_medical_concern,@consulting_category_speciality,@patient_info_subscriber_id,@patient_info_patient_type,@delay_day); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@booking_status", teleedoctor.booking_status);
                    command.Parameters.AddWithValue("@booking_code", teleedoctor.booking_code);
                    command.Parameters.AddWithValue("@payment_status", teleedoctor.payment_status);
                    command.Parameters.AddWithValue("@payment_method", teleedoctor.payment_method);
                    command.Parameters.AddWithValue("@medical_institute", teleedoctor.medical_institute);
                    command.Parameters.AddWithValue("@doctor_name", teleedoctor.doctor_name);
                    command.Parameters.AddWithValue("@doctor_nmc", teleedoctor.doctor_nmc);
                    command.Parameters.AddWithValue("@doctor_gender", teleedoctor.doctor_gender);
                    command.Parameters.AddWithValue("@contact_info_address_1", teleedoctor.contact_info_address_1);
                    command.Parameters.AddWithValue("@contact_info_address_2", teleedoctor.contact_info_address_2);
                    command.Parameters.AddWithValue("@contact_info_contact", teleedoctor.contact_info_contact);
                    command.Parameters.AddWithValue("@contact_info_is_patient", teleedoctor.contact_info_is_patient);
                    command.Parameters.AddWithValue("@contact_info_name", teleedoctor.contact_info_name);
                    command.Parameters.AddWithValue("@patient_info_contact", teleedoctor.patient_info_contact);
                    command.Parameters.AddWithValue("@patient_info_email", teleedoctor.patient_info_email);
                    command.Parameters.AddWithValue("@patient_info_name", teleedoctor.patient_info_name);
                    command.Parameters.AddWithValue("@appointment_date_time", teleedoctor.appointment_date_time);
                    command.Parameters.AddWithValue("@last_modified_date", teleedoctor.last_modified_date);
                    command.Parameters.AddWithValue("@preferred_date", teleedoctor.preferred_date);
                    command.Parameters.AddWithValue("@created_date", teleedoctor.created_date);
                    command.Parameters.AddWithValue("@consulting_category_medical_concern", teleedoctor.consulting_category_medical_concern);
                    command.Parameters.AddWithValue("@consulting_category_speciality", teleedoctor.consulting_category_speciality);
                    command.Parameters.AddWithValue("@patient_info_subscriber_id", teleedoctor.patient_info_subscriber_id);
                    command.Parameters.AddWithValue("@patient_info_patient_type", teleedoctor.patient_info_patient_type);
                    command.Parameters.AddWithValue("@delay_day", teleedoctor.delay_day);
                    long newteledoctorId = Convert.ToInt64(command.ExecuteScalar());
                    teleedoctor.tele_doctor_id = newteledoctorId;
                }
            }
        }
        
        public void OrderInsert(List<Order> orderssss)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var order in orderssss)
                {

                    string delete1 = "delete from BillingAddress where order_id = @order_id";
                    SqlCommand deletecommand1 = new SqlCommand(delete1, connection);
                    deletecommand1.Parameters.AddWithValue("@order_id", order.id);
                    int result1 = deletecommand1.ExecuteNonQuery();

                    string deletes = "delete from ShippingAddress where order_id = @order_id";
                    SqlCommand deletecommands = new SqlCommand(deletes, connection);
                    deletecommands.Parameters.AddWithValue("@order_id", order.id);
                    int results = deletecommands.ExecuteNonQuery();

                    string delete = "delete from Orders where order_id = @order_id";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@order_id", order.id);
                    int result = deletecommand.ExecuteNonQuery();

                    string deletemeta = "delete from MetaData where order_id = @order_id";
                    SqlCommand deletemetadata = new SqlCommand(deletemeta, connection);
                    deletemetadata.Parameters.AddWithValue("@order_id", order.id);
                    int resultmeta = deletemetadata.ExecuteNonQuery();

                    var metass = order.meta_data;
                    foreach (var item in metass)
                    {
                        if (item.key == "paying_network")
                        {
                            string insertmeta = $@"
                          INSERT INTO MetaData(order_id, meta_key, meta_value)
                           VALUES ({order.id}, '{item.key}', '{item.value}')";

                            SqlCommand meta_cmd = new SqlCommand(insertmeta, connection);

                            meta_cmd.ExecuteNonQuery();
                        }
                    }




                    // Example: Inserting data into Orders table

                    string insertOrderQuery = $@"
               INSERT INTO Orders (
            parent_id, status, currency, version, prices_include_tax,
            date_created, date_modified, discount_total, discount_tax,
            shipping_total, shipping_tax, cart_tax, total, total_tax,
            customer_id, order_key, payment_method, payment_method_title,
            transaction_id, customer_ip_address, customer_user_agent,
            created_via, customer_note, date_completed, date_paid,
            cart_hash, number, payment_url, is_editable, needs_payment,
            needs_processing, date_created_gmt, date_modified_gmt,
            date_completed_gmt, date_paid_gmt, currency_symbol,order_id
        )
        VALUES (
             '{order.parent_id}', '{order.status}', '{order.currency}', '{order.version}', {Convert.ToInt32(order.prices_include_tax)},
            '{order.date_created}', '{order.date_modified}', {order.discount_total}, {order.discount_tax},
            {order.shipping_total}, {order.shipping_tax}, {order.cart_tax}, {order.total}, {order.total_tax},
            {order.customer_id}, '{order.order_key}', '{order.payment_method}', '{order.payment_method_title}',
            '{order.transaction_id}', '{order.customer_ip_address}', '{order.customer_user_agent}',
            '{order.created_via}', '{order.customer_note}', '{order.date_completed}', '{order.date_paid}',
            '{order.cart_hash}', '{order.number}', '{order.payment_url}', {Convert.ToInt32(order.is_editable)},
            {Convert.ToInt32(order.needs_payment)}, {Convert.ToInt32(order.needs_processing)}, '{order.date_created_gmt}', '{order.date_modified_gmt}',
            '{order.date_completed_gmt}', '{order.date_paid_gmt}', '{order.currency_symbol}',{order.id}  );";

                    SqlCommand command = new SqlCommand(insertOrderQuery, connection);
                    command.ExecuteNonQuery();



                    string insertBillingAddressQuery = $@"              
                         INSERT INTO BillingAddress (
                       first_name, last_name, company, address_1, address_2,
                       city, state, postcode, country, email, phone, order_id
                           )
                          VALUES (
                       '{order.billing.first_name}', '{order.billing.last_name}', '{order.billing.company}',
                       '{order.billing.address_1}', '{order.billing.address_2}', '{order.billing.city}',
                       '{order.billing.state}', '{order.billing.postcode}', '{order.billing.country}',
                        '{order.billing.email}', '{order.billing.phone}', {order.id}
                          );        
                             ";

                    SqlCommand command1 = new SqlCommand(insertBillingAddressQuery, connection);
                    command1.ExecuteNonQuery();




                    string insertShippingAddressQuery = $@"
              
                        INSERT INTO ShippingAddress (
                        first_name, last_name, company, address_1, address_2,
                       city, state, postcode, country, phone, order_id
                        )
        VALUES (

            '{order.shipping.first_name}', '{order.shipping.last_name}', '{order.shipping.company}',
            '{order.shipping.address_1}', '{order.shipping.address_2}', '{order.shipping.city}',
            '{order.shipping.state}', '{order.shipping.postcode}', '{order.shipping.country}',
            '{order.shipping.phone}', {order.id}
        ); ";

                    SqlCommand commands = new SqlCommand(insertShippingAddressQuery, connection);
                    commands.ExecuteNonQuery();

                    _lastSyncDate = DateTime.Now;
                    // Sleep 5 minutes and again call the api after 5 minutes

                }

            }

         }
       
        public void GetInternationalDoctor(JObject internationaldoctor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var item in internationaldoctor["bookings"])
                {
                    var appointmentdate = (string)item["appointment_date_time"] ?? " ";
                    var apointmentstring = Convert.ToString(appointmentdate);

                    JToken consultingCategoryToken = item["consulting_category"];
                    string consulting_category_medical_concern = null;
                    string consulting_category_speciality = null;

                    if (consultingCategoryToken is JObject consultingCategoryObject)
                    {
                        consulting_category_medical_concern = (string)consultingCategoryObject["medical_concern"];
                        consulting_category_speciality = (string)consultingCategoryObject["speciality"];
                    }
                    else if (consultingCategoryToken is JValue consultingCategoryValue)
                    {
                        // Handle the case when the consulting_category is a single value (e.g., null, string, number, boolean)
                        consulting_category_medical_concern = consultingCategoryValue.Value?.ToString();
                        consulting_category_speciality = null; // Handle as needed
                    }

                    InternationalDoctor international_doctor = new InternationalDoctor()
                    {
                        booking_status = (string)item["booking_status"] ?? "",
                        booking_code = (string)item["booking_code"] ?? "",
                        payment_status = (string)item["payment_status"] ?? "",
                        payment_method = (string)item["payment_method"] ?? "",
                        medical_institute = (string)item["medical_institute"] ?? "",
                        doctor_name = (string)item["doctor"]["name"] ?? "",
                        doctor_nmc = (string)item["doctor"]["nmc"] ?? "",
                        doctor_gender = (string)item["doctor"]["gender"] ?? "",
                        contact_info_address_1 = (string)item["contact_info"]["address_1"] ?? "",
                        contact_info_address_2 = (string)item["contact_info"]["address_2"] ?? "",
                        contact_info_contact = (string)item["contact_info"]["contact"] ?? "",
                        contact_info_is_patient = (bool)item["contact_info"]["is_patient"],
                        contact_info_name = (string)item["contact_info"]["name"] ?? "",
                        patient_info_contact = (string)item["patient_info"]["contact"] ?? "",
                        patient_info_email = (string)item["patient_info"]["email"] ?? "",
                        patient_info_name = (string)item["patient_info"]["name"] ?? "",
                        appointment_date_time = apointmentstring ?? "",
                        last_modified_date = (DateTime?)item["last_modified_date"],
                        preferred_date = (DateTime?)item["preferred_date"],
                        created_date = (DateTime?)item["created_date"],
                        consulting_category_medical_concern = consulting_category_medical_concern ?? "",
                        consulting_category_speciality = consulting_category_speciality ?? "",
                        patient_info_subscriber_id = (string)item["patient_info"]["subscriber_id"] ?? "",
                        patient_info_patient_type = (string)item["patient_info"]["patient_type"] ?? "",

                        delay_day = (int)item["delay_day"]
                    };


                    // delete the internationaldoctor data
                    string delete = "delete from internationaldoctor where booking_code = @booking_code";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@booking_code", international_doctor.booking_code);
                    int result = deletecommand.ExecuteNonQuery();

                    // insert the internationaldoctor data
                    string query = "INSERT INTO internationaldoctor (booking_status, booking_code, payment_status, payment_method, medical_institute,doctor_name,doctor_nmc,doctor_gender, contact_info_address_1, contact_info_address_2, contact_info_contact, contact_info_is_patient, contact_info_name, patient_info_contact, patient_info_email, patient_info_name, appointment_date_time, last_modified_date, preferred_date,created_date,consulting_category_medical_concern,consulting_category_speciality,delay_day) VALUES (@booking_status, @booking_code, @payment_status, @payment_method, @medical_institute,@doctor_name,@doctor_nmc,@doctor_gender, @contact_info_address_1, @contact_info_address_2, @contact_info_contact, @contact_info_is_patient, @contact_info_name, @patient_info_contact, @patient_info_email, @patient_info_name, @appointment_date_time, @last_modified_date, @preferred_date,@created_date,@consulting_category_medical_concern,@consulting_category_speciality,@delay_day); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@booking_status", international_doctor.booking_status);
                    command.Parameters.AddWithValue("@booking_code", international_doctor.booking_code);
                    command.Parameters.AddWithValue("@payment_status", international_doctor.payment_status);
                    command.Parameters.AddWithValue("@payment_method", international_doctor.payment_method);
                    command.Parameters.AddWithValue("@medical_institute", international_doctor.medical_institute);
                    command.Parameters.AddWithValue("@doctor_name", international_doctor.doctor_name);
                    command.Parameters.AddWithValue("@doctor_nmc", international_doctor.doctor_nmc);
                    command.Parameters.AddWithValue("@doctor_gender", international_doctor.doctor_gender);
                    command.Parameters.AddWithValue("@contact_info_address_1", international_doctor.contact_info_address_1);
                    command.Parameters.AddWithValue("@contact_info_address_2", international_doctor.contact_info_address_2);
                    command.Parameters.AddWithValue("@contact_info_contact", international_doctor.contact_info_contact);
                    command.Parameters.AddWithValue("@contact_info_is_patient", international_doctor.contact_info_is_patient);
                    command.Parameters.AddWithValue("@contact_info_name", international_doctor.contact_info_name);
                    command.Parameters.AddWithValue("@patient_info_contact", international_doctor.patient_info_contact);
                    command.Parameters.AddWithValue("@patient_info_email", international_doctor.patient_info_email);
                    command.Parameters.AddWithValue("@patient_info_name", international_doctor.patient_info_name);
                    command.Parameters.AddWithValue("@appointment_date_time", international_doctor.appointment_date_time);
                    command.Parameters.AddWithValue("@last_modified_date", international_doctor.last_modified_date);
                    command.Parameters.AddWithValue("@preferred_date", international_doctor.preferred_date);
                    command.Parameters.AddWithValue("@created_date", international_doctor.created_date);
                    command.Parameters.AddWithValue("@consulting_category_medical_concern", international_doctor.consulting_category_medical_concern);
                    command.Parameters.AddWithValue("@consulting_category_speciality", international_doctor.consulting_category_speciality);
                    command.Parameters.AddWithValue("@patient_info_subscriber_id", international_doctor.patient_info_subscriber_id);
                    command.Parameters.AddWithValue("@patient_info_patient_type", international_doctor.patient_info_patient_type);
                    command.Parameters.AddWithValue("@delay_day", international_doctor.delay_day);
                    long newinternationaldoctorId = Convert.ToInt64(command.ExecuteScalar());
                    international_doctor.international_doctor_id = newinternationaldoctorId;
                }
            }

        }

        public void VirtualDoctors(JObject virtualdoctor)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var item in virtualdoctor["bookings"])
                {
                    var appointmentdate = (string)item["appointment_date_time"] ?? " ";
                    var apointmentstring = Convert.ToString(appointmentdate);

                    JToken consultingCategoryToken = item["consulting_category"];
                    string consulting_category_medical_concern = null;
                    string consulting_category_speciality = null;

                    if (consultingCategoryToken is JObject consultingCategoryObject)
                    {
                        consulting_category_medical_concern = (string)consultingCategoryObject["medical_concern"];
                        consulting_category_speciality = (string)consultingCategoryObject["speciality"];
                    }
                    else if (consultingCategoryToken is JValue consultingCategoryValue)
                    {
                        // Handle the case when the consulting_category is a single value (e.g., null, string, number, boolean)
                        consulting_category_medical_concern = consultingCategoryValue.Value?.ToString();
                        consulting_category_speciality = null; // Handle as needed
                    }

                    VirtualDoctor virtual_doctor = new VirtualDoctor()
                    {
                        booking_status = (string)item["booking_status"] ?? "",
                        booking_code = (string)item["booking_code"] ?? "",
                        payment_status = (string)item["payment_status"] ?? "",
                        payment_method = (string)item["payment_method"] ?? "",
                        medical_institute = (string)item["medical_institute"] ?? "",
                        doctor_name = (string)item["doctor"]["name"] ?? "",
                        doctor_nmc = (string)item["doctor"]["nmc"] ?? "",
                        doctor_gender = (string)item["doctor"]["gender"] ?? "",
                        contact_info_address_1 = (string)item["contact_info"]["address_1"] ?? "",
                        contact_info_address_2 = (string)item["contact_info"]["address_2"] ?? "",
                        contact_info_contact = (string)item["contact_info"]["contact"] ?? "",
                        contact_info_is_patient = (bool)item["contact_info"]["is_patient"],
                        contact_info_name = (string)item["contact_info"]["name"] ?? "",
                        patient_info_contact = (string)item["patient_info"]["contact"] ?? "",
                        patient_info_email = (string)item["patient_info"]["email"] ?? "",
                        patient_info_name = (string)item["patient_info"]["name"] ?? "",
                        appointment_date_time = apointmentstring ?? "",
                        last_modified_date = (DateTime?)item["last_modified_date"],
                        preferred_date = (DateTime?)item["preferred_date"],
                        created_date = (DateTime?)item["created_date"],
                        consulting_category_medical_concern = consulting_category_medical_concern ?? "",
                        consulting_category_speciality = consulting_category_speciality ?? "",
                        patient_info_subscriber_id = (string)item["patient_info"]["subscriber_id"] ?? "",
                        patient_info_patient_type = (string)item["patient_info"]["patient_type"] ?? "",
                        delay_day = (int)item["delay_day"]
                    };


                    // delete the virtual_doctor data
                    string delete = "delete from virtualdoctor where booking_code = @booking_code";
                    SqlCommand deletecommand = new SqlCommand(delete, connection);
                    deletecommand.Parameters.AddWithValue("@booking_code", virtual_doctor.booking_code);
                    int result = deletecommand.ExecuteNonQuery();

                    // insert the virtual_doctor data
                    string query = "INSERT INTO virtualdoctor (booking_status, booking_code, payment_status, payment_method, medical_institute,doctor_name,doctor_nmc,doctor_gender, contact_info_address_1, contact_info_address_2, contact_info_contact, contact_info_is_patient, contact_info_name, patient_info_contact, patient_info_email, patient_info_name, appointment_date_time, last_modified_date, preferred_date,created_date,consulting_category_medical_concern,consulting_category_speciality,delay_day) VALUES (@booking_status, @booking_code, @payment_status, @payment_method, @medical_institute,@doctor_name,@doctor_nmc,@doctor_gender, @contact_info_address_1, @contact_info_address_2, @contact_info_contact, @contact_info_is_patient, @contact_info_name, @patient_info_contact, @patient_info_email, @patient_info_name, @appointment_date_time, @last_modified_date, @preferred_date,@created_date,@consulting_category_medical_concern,@consulting_category_speciality,@delay_day); SELECT SCOPE_IDENTITY();";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@booking_status", virtual_doctor.booking_status);
                    command.Parameters.AddWithValue("@booking_code", virtual_doctor.booking_code);
                    command.Parameters.AddWithValue("@payment_status", virtual_doctor.payment_status);
                    command.Parameters.AddWithValue("@payment_method", virtual_doctor.payment_method);
                    command.Parameters.AddWithValue("@medical_institute", virtual_doctor.medical_institute);
                    command.Parameters.AddWithValue("@doctor_name", virtual_doctor.doctor_name);
                    command.Parameters.AddWithValue("@doctor_nmc", virtual_doctor.doctor_nmc);
                    command.Parameters.AddWithValue("@doctor_gender", virtual_doctor.doctor_gender);
                    command.Parameters.AddWithValue("@contact_info_address_1", virtual_doctor.contact_info_address_1);
                    command.Parameters.AddWithValue("@contact_info_address_2", virtual_doctor.contact_info_address_2);
                    command.Parameters.AddWithValue("@contact_info_contact", virtual_doctor.contact_info_contact);
                    command.Parameters.AddWithValue("@contact_info_is_patient", virtual_doctor.contact_info_is_patient);
                    command.Parameters.AddWithValue("@contact_info_name", virtual_doctor.contact_info_name);
                    command.Parameters.AddWithValue("@patient_info_contact", virtual_doctor.patient_info_contact);
                    command.Parameters.AddWithValue("@patient_info_email", virtual_doctor.patient_info_email);
                    command.Parameters.AddWithValue("@patient_info_name", virtual_doctor.patient_info_name);
                    command.Parameters.AddWithValue("@appointment_date_time", virtual_doctor.appointment_date_time);
                    command.Parameters.AddWithValue("@last_modified_date", virtual_doctor.last_modified_date);
                    command.Parameters.AddWithValue("@preferred_date", virtual_doctor.preferred_date);
                    command.Parameters.AddWithValue("@created_date", virtual_doctor.created_date);
                    command.Parameters.AddWithValue("@consulting_category_medical_concern", virtual_doctor.consulting_category_medical_concern);
                    command.Parameters.AddWithValue("@consulting_category_speciality", virtual_doctor.consulting_category_speciality);
                    command.Parameters.AddWithValue("@patient_info_subscriber_id", virtual_doctor.patient_info_subscriber_id);
                    command.Parameters.AddWithValue("@patient_info_patient_type", virtual_doctor.patient_info_patient_type);
                    command.Parameters.AddWithValue("@delay_day", virtual_doctor.delay_day);
                    long newvirtualdoctorId = Convert.ToInt64(command.ExecuteScalar());
                    virtual_doctor.virtual_doctor_id = newvirtualdoctorId;
                }
            }

        }



    }
}
