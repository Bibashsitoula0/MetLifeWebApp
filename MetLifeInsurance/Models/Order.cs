namespace MetLifeInsurance.Models
{
  
        public class Order
        {
           
      
            public string status { get; set; }
            public string currency { get; set; }
            public string version { get; set; }
            public bool prices_include_tax { get; set; }
            public DateTime date_created { get; set; }
            public DateTime date_modified { get; set; }
            public decimal discount_total { get; set; }
            public decimal discount_tax { get; set; }
            public decimal total { get; set; }
            public int customer_id { get; set; }
            public string order_key { get; set; }
            public string payment_method { get; set; }
            public string payment_method_title { get; set; }
            public string customer_ip_address { get; set; }
            public DateTime? date_completed { get; set; }
            public DateTime? date_paid { get; set; }
            public string number { get; set; }
            public string payment_url { get; set; }
            public int order_id { get; set; }
            public string shipping_first_name { get; set; }
            public string shipping_last_name { get; set; }
            public string shipping_company { get; set; }
            public string shipping_address1 { get; set; }
            public string shipping_address2 { get; set; }
            public string shipping_city { get; set; }
            public string shipping_state { get; set; }
            public string biling_first_name { get; set; }
            public string biling_last_name { get; set; }
            public string biling_company { get; set; }
            public string biling_adress { get; set; }
            public string biling_adress2 { get; set; }
            public string biling_city { get; set; }
            public string biling_state { get; set; }
            public string biling_email { get; set; }
            public string biling_phone { get; set; }
           public string transaction_id { get; set; }
           public string metavalue { get; set; }

        public long serial_number { get; set; }
        public int id { get; set; }
        public long totalcount { get; set; }
    }
}
