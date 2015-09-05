using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return orderlines;
        }
    }
}
