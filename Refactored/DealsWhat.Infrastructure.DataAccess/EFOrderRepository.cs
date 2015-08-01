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
        private DealsWhatUnitOfWork unitOfWork;

        public EFOrderRepository(DealsWhatUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public void Create(OrderModel model)
        {
            throw new NotImplementedException();
        }

        public OrderModel FindByKey(string key)
        {
            return this.unitOfWork.Set<OrderModel>()
                .Include("User")
                .Include("Orderlines")
                .FirstOrDefault(a => a.Key == key);
        }

        public IEnumerable<OrderModel> GetAll()
        {
            return this.unitOfWork.Set<OrderModel>()
                .Include("User")
                .Include("Orderlines")
                .ToList();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(OrderModel model)
        {
            throw new NotImplementedException();
        }
    }
}
