using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public class OrderModel : IEntity, IAggregateRoot
    {
        public AddressModel BillingAddress { get; set; }

        public IUserModel User { get; set; }
        public DateTime DateCreated { get; private set; }

        public OrderStatus Status { get; private set; }

        public string Key { get; internal set; }

        public double TotalSpecialPrice { get; internal set; }

        public double TotalRegularPrice { get; internal set; }

        public ICollection<OrderlineModel> Orderlines { get; private set; }

        private OrderModel()
        {
            Orderlines = new List<OrderlineModel>();
        }

        public void SetOrderPaid()
        {
            Status = OrderStatus.Paid;
        }

        public void SetOrderDelivered()
        {
            Status = OrderStatus.Delivered;
        }

        public static OrderModel Create(IUserModel userModel, IEnumerable<CartItemModel> cartItems)
        {
            if (userModel.BillingAddress == null)
            {
                throw new ArgumentException("Billing address cannot be empty.");
            }

            var order = new OrderModel
            {
                BillingAddress = userModel.BillingAddress,
                User = userModel
            };

            foreach (var cartItem in cartItems)
            {
                order.Orderlines.Add(OrderlineModel.Create(cartItem));
            }

            order.Status = OrderStatus.Unpaid;
            order.Key = GenerateOrderId();
            order.DateCreated = DateTime.UtcNow;

            order.TotalSpecialPrice =
                cartItems
                    .ToList()
                    .Select(c => c.DealOption.SpecialPrice)
                    .Aggregate(0.0d, (c1, c2) => { return c1 + c2; });

            order.TotalRegularPrice =
                cartItems
                    .ToList()
                    .Select(c => c.DealOption.RegularPrice)
                    .Aggregate(0.0d, (c1, c2) => { return c1 + c2; });

            return order;
        }

        private static string GenerateOrderId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 18)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
    }

    public enum OrderStatus
    {
        Unpaid = 0,
        Paid = 1,
        Delivered = 2
    }
}
