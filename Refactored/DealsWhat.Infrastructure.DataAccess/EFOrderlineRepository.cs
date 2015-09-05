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

            var orders = this.dbContext.Orders
                  .Where(o => o.Orderlines.Any(ol => merchantDealIds.Contains(ol.Deal.Key)))
                  .Include("Orderlines.Deal.Images")
                  .Include("Orderlines.Dealoption")
                  .Include("Orderlines.AttributeValues")
                  .Include("BillingAddress")
                  .ToList();

            var merchantOrderlines = new List<MerchantOrderlineModel>();
            foreach (var order in orders)
            {
                foreach (var orderline in order.Orderlines)
                {
                    if (!merchantDealIds.Contains(orderline.Deal.Key))
                    {
                        continue;
                    }

                    var merchantOrderline = MerchantOrderlineModel.Create(orderline, order.BillingAddress, order.DateCreated);

                    merchantOrderlines.Add(merchantOrderline);
                }
            }

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
            throw new NotImplementedException();
        }

        public void Update(OrderlineModel model)
        {
            throw new NotImplementedException();
        }
    }
}
