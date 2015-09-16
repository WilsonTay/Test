using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFOrderlineRepository : IOrderlineRepository
    {
        private DealsWhatDbContext dbContext;

        public EFOrderlineRepository(DealsWhatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(OrderlineModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MerchantOrderlineModel> FindByMerchant(string merchantId)
        {
            var merchant = this.dbContext.Set<MerchantModel>()
                .Include("Deals.Images")
                .First(m => m.Key == merchantId);
            var merchantDealIds = merchant.Deals.Select(m => m.Key).ToList();

            var users = this.dbContext
                .Users.Where(user => user.Orders
                  .Any(o => o.Orderlines.Any(ol => merchantDealIds.Contains(ol.Deal.Key))))
                  .Include("Orders.Orderlines.Deal.Images")
                  .Include("Orders.Orderlines.Dealoption")
                  .Include("Orders.Orderlines.AttributeValues")
                  .Include("Orders.Orderlines.Coupons")
                  .Include("Orders.BillingAddress")
                  .ToList();

            var merchantOrderlines = new List<MerchantOrderlineModel>();
            foreach (var user in users)
            {
                foreach (var order in user.Orders)
                {
                    foreach (var orderline in order.Orderlines)
                    {
                        if (!merchantDealIds.Contains(orderline.Deal.Key))
                        {
                            continue;
                        }

                        var merchantOrderline = MerchantOrderlineModel.Create(orderline, order.BillingAddress, order.DateCreated, user.EmailAddress);

                        merchantOrderlines.Add(merchantOrderline);
                    }
                }
            }

            //var merchantOrderlines = new List<MerchantOrderlineModel>();
            //foreach (var order in orders)
            //{
            //    foreach (var orderline in order.Orderlines)
            //    {
            //        if (!merchantDealIds.Contains(orderline.Deal.Key))
            //        {
            //            continue;
            //        }

            //        var merchantOrderline = MerchantOrderlineModel.Create(orderline, order.BillingAddress, order.DateCreated, order.User.EmailAddress);

            //        merchantOrderlines.Add(merchantOrderline);
            //    }
            //}

            return merchantOrderlines;
            //Console.WriteLine(merchantOrderlines);

            //var orderLines = this.dbContext.Set<OrderlineModel>()
            //    .Include("Deal.Images")
            //    .Include("DealOption")
            //    .Include("AttributeValues")

            //    .Where(o => merchantDealIds.Contains(o.Deal.Key))
            //    .ToList();

            //return orderLines;
        }

        public OrderlineModel FindOrderlineWithCoupon(string couponValue)
        {
            var orderline = this.dbContext
                .Set<OrderlineModel>()
                .Include("Deal")
                .Include("Deal.Images")
                .Include("Coupons")
                .Include("DealOption")
                .Include("AttributeValues")
                .FirstOrDefault(a => a.Coupons.Any(c => c.Value == couponValue));

            return orderline;
        }

        public OrderlineModel FindByKey(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderlineModel> GetAll()
        {
            return this.dbContext.Set<OrderlineModel>()
                .Include("Deal")
                .ToList();
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        public void Update(OrderlineModel model)
        {
            throw new NotImplementedException();
        }
    }
}
