using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model.Exceptions;

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

        public MerchantModel GetMerchantInfo(string emailAddress)
        {
            var merchantRepo = unitOfWorkFactory.CreateUnitOfWork().CreateMerchantRepository();

            return merchantRepo.GetAll().FirstOrDefault(m => m.EmailAddress == emailAddress);
        }

        public OrderlineModel RedeemCoupon(CouponRedemption redemption)
        {
            var uow = unitOfWorkFactory.CreateUnitOfWork();
            var orderlineRepo = uow.CreateOrderlineRepository();
            var orderlineWithCoupon = orderlineRepo.FindOrderlineWithCoupon(redemption.Value);

            var coupon = orderlineWithCoupon.Coupons.First(a => a.Value.Equals(redemption.Value));

            if (coupon == null)
            {
                throw new CouponNotFoundException();
            }

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
