using System.Collections.Generic;

namespace DealsWhat.Application.WebApi.Models
{
    public class OrderlineViewModel
    {
        public double SpecialPrice { get; set; }
        public double RegularPrice { get; set; }

        public string DealOption { get; set; }

        public string DealUrl { get; set; }

        public string DealThumbnailUrl { get; set; }

        public Dictionary<string, string> DealAttributes { get; set; }

        public int Quantity { get; set; }

        public string Id { get; set; }

        public IList<CouponViewModel> Coupons { get; set; } 

        public OrderlineViewModel()
        {
            DealAttributes = new Dictionary<string, string>();
            Coupons = new List<CouponViewModel>();
        }
    }
}