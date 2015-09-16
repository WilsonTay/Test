using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealsWhat.Domain.Model;

namespace DealsWhat.Application.WebApi.Models
{
    public class CouponViewModel
    {
        public CouponStatus Status { get; set; }

        public string Value { get; set; }

        public DateTime? DateRedeemed { get; set; }
    }
}