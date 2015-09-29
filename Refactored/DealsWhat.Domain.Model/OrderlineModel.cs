using DealsWhat.Domain.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class OrderlineModel : IEntity, IAggregateRoot
    {
        public int Quantity { get; private set; }

        public double SpecialPrice { get; private set; }

        public double RegularPrice { get; private set; }

        public DealModel Deal { get; private set; }

        public DealOptionModel DealOption { get; private set; }

        public ICollection<CouponModel> Coupons
        {
            get
            {
                return coupons;
            }
        }

        public ICollection<DealAttributeModel> AttributeValues
        {
            get
            {
                return attributeValues;
            }
        }

        private readonly List<DealAttributeModel> attributeValues;

        private readonly List<CouponModel> coupons;

        private OrderlineModel()
        {
            attributeValues = new List<DealAttributeModel>();
            coupons = new List<CouponModel>();
        }

        public static OrderlineModel Create(
            CartItemModel cartItem)
        {
            var orderLine = new OrderlineModel
            {
                Deal = cartItem.Deal,
                DealOption = cartItem.DealOption
            };

            foreach (var attr in cartItem.AttributeValues)
            {
                orderLine.attributeValues.Add(attr);
            }

            orderLine.Key = Guid.NewGuid().ToString();
            orderLine.SpecialPrice = cartItem.DealOption.SpecialPrice * cartItem.Quantity;
            orderLine.RegularPrice = cartItem.DealOption.RegularPrice * cartItem.Quantity;
            orderLine.Quantity = cartItem.Quantity;


            return orderLine;
        }

        public void GenerateCouponIfNecessary()
        {
            if (Deal.DealType == DealType.Coupon)
            {
                if (coupons.Any())
                {
                    throw new CouponAlreadyGeneratedException("Coupons have already been generated.");
                }

                // TODO: COUPON.
                for (int i = 0; i < Quantity; i++)
                {
                    var couponValue = Guid.NewGuid().ToString();
                    var coupon = CouponModel.Create(couponValue);

                    coupons.Add(coupon);
                }
            }

        }

        public string Key { get; internal set; }
    }
}
