using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFOrderRepository : IRepository<OrderModel>
    {
        private DealsWhatDbContext dbContext;

        public EFOrderRepository(DealsWhatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(OrderModel model)
        {
            throw new NotImplementedException();
        }

        public OrderModel FindByKey(string key)
        {
            try
            {
                //Removed user because it cannot be found when placing order.
                var entity = this.dbContext.Set<OrderModel>()
                    //.Include("User")
                    .Include("Orderlines")
                    .FirstOrDefault(a => a.Key == key);

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public IEnumerable<OrderModel> GetAll()
        {
            return this.dbContext.Set<OrderModel>()
                //.Include("User")
                .Include("Orderlines")
                .Include("Orderlines.Deal")
                .ToList();
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        public void Update(OrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
