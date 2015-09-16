using DealsWhat.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Interfaces
{
    public interface IOrderlineRepository : IRepository<OrderlineModel>
    {
        IEnumerable<MerchantOrderlineModel> FindByMerchant(string merchantId);

        OrderlineModel FindOrderlineWithCoupon(string couponValue);
    }
}
