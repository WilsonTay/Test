﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<DealModel> CreateDealRepository();
        IRepository<DealCategoryModel> CreateDealCategoryRepository();

        IUserRepository CreateUserRepository();

        IRepository<OrderModel> CreateOrderRepository();

        IOrderlineRepository CreateOrderlineRepository();
        IRepository<MerchantModel> CreateMerchantRepository();
    }
}
