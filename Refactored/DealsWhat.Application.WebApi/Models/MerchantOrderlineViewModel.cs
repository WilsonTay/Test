using System;
using System.Collections.Generic;

namespace DealsWhat.Application.WebApi.Models
{
    public class MerchantOrderlineViewModel
    {
        public IList<string> AddressLines { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DatePlaced { get; set; }

        public double SpecialPrice { get; set; }
        public double RegularPrice { get; set; }

        public string DealOption { get; set; }

        public string DealUrl { get; set; }

        public string DealThumbnailUrl { get; set; }

        public Dictionary<string, string> DealAttributes { get; set; }

        public int Quantity { get; set; }

        public string Id { get; set; }

        public IList<CouponViewModel> Coupons { get; set; } 

        public MerchantOrderlineViewModel()
        {
            DealAttributes = new Dictionary<string, string>();
            AddressLines = new List<string>();
            Coupons = new List<CouponViewModel>();
        }
    }
}