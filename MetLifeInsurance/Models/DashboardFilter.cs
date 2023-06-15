using MetLifeInsurance.Helpers;

namespace MetLifeInsurance.Models
{
    public class DashboardFilter
    {
        public string formDate { get; set; }
        public string toDate { get; set; }
        public string code { get; set; }
        public string booking_status { get; set; }
        public string pending_status { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }

    

}
