using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DealsWhatDbContext dbContext;

        public EFUnitOfWork(DealsWhatDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepository<DealModel> CreateDealRepository()
        {
            return new EFDealRepository(this.dbContext);
        }

        public IRepository<DealCategoryModel> CreateDealCategoryRepository()
        {
            return new EFDealCategoryRepository(this.dbContext);
        }

        IUserRepository IUnitOfWork.CreateUserRepository()
        {
            return new EFUserRepository(this.dbContext);
        }

        public IRepository<OrderModel> CreateOrderRepository()
        {
            return new EFOrderRepository(this.dbContext);
        } 
    }
}
