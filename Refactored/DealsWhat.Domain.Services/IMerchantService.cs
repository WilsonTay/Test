using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Services
{
    public interface IMerchantService
    {
        IEnumerable<MerchantOrderlineModel> SearchOrderlines(MerchantOrderLineSearchQuery query);

        OrderlineModel RedeemCoupon(CouponRedemption redemption);

        MerchantModel GetMerchantInfo(string emailAddress);
    }
}
