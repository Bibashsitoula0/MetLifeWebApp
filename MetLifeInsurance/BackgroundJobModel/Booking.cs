namespace MetLifeInsurance.BackgroundJobModel
{
    public class Booking
    {
        public long? bookingid { get; set; }
        public string booking_status { get; set; }
        public string booking_code { get; set; }
        public string payment_status { get; set; }
        public string payment_method { get; set; }
        public string medical_institute { get; set; }
        public string patient_info_name { get; set; }
        public string patient_info_email { get; set; }
        public string patient_info_contact { get; set; }
        public bool contact_info_is_patient { get; set; }
        public string contact_info_address_1 { get; set; }
        public string contact_info_address_2 { get; set; }
        public string contact_info_name { get; set; }
        public string contact_info_contact { get; set; }
        public DateTime? preferred_date { get; set; }
        public string appointment_date_time { get; set; }
        public DateTime? last_modified_date { get; set; }
        public DateTime? created_date { get; set; }
        public string patient_info_subscriber_id { get; set; }
        public string patient_info_patient_type { get; set; }
        public int? delay_day { get; set; }
    }
}
