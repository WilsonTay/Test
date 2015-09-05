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

        public IEnumerable<CouponModel> Coupons { get; private set; }

        public ICollection<DealAttributeModel> AttributeValues
        {
            get
            {
                return attributeValues;
            }
        }

        private readonly List<DealAttributeModel> attributeValues;

        private OrderlineModel()
        {
            attributeValues = new List<DealAttributeModel>();
            Coupons = new List<CouponModel>();
        }

        public void AddCoupon(CouponModel coupon)
        {
          
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

        public string Key { get; internal set; }
    }
}
