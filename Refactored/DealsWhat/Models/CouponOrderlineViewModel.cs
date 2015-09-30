using System;
using System.Collections.Generic;

namespace DealsWhat.Models
{
    public class CouponOrderlineViewModel
    {
        public string DealOption { get; set; }
        public double RegularPrice { get; set; }

        public string FinePrint { get; set; }

        public DateTime EndTime { get; set; }

        public string DealImageUrl { get; set; }

        public string Id { get; set; }

        public string DealUrl { get; set; }
        public Dictionary<string, string> DealAttributes { get; set; }

        public CouponOrderlineViewModel()
        {
            DealAttributes = new Dictionary<string, string>();
        }
    }
}