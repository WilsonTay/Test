using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public MerchantService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IEnumerable<MerchantOrderlineModel> SearchOrderlines(MerchantOrderLineSearchQuery query)
        {
            var orderlineRepo = unitOfWorkFactory.CreateUnitOfWork().CreateOrderlineRepository();
            var orderlines = orderlineRepo.FindByMerchant(query.MerchantId);

            if (!string.IsNullOrEmpty(query.DealId))
            {
                return orderlines.Where(o => o.Orderline.Deal.Key == query.DealId);
            }

            return orderlines;
        }

        public OrderlineModel RedeemCoupon(CouponRedemption redemption)
        {
            var uow = unitOfWorkFactory.CreateUnitOfWork();
            var orderlineRepo = uow.CreateOrderlineRepository();
            var orderlineWithCoupon = orderlineRepo.FindOrderlineWithCoupon(redemption.Value);

            var coupon = orderlineWithCoupon.Coupons.First(a => a.Value.Equals(redemption.Value));
            coupon.SetRedeemed();

            orderlineRepo.Save();

            return orderlineWithCoupon;
        }
    }

    public class CouponRedemption
    {
        public string Value { get; private set; }

        public CouponRedemption(string value)
        {
            this.Value = value;
        }
    }
}
