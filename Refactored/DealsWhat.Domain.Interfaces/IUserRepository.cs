using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Interfaces
{
    public interface IUserRepository : IRepository<IUserModel>
    {
        IUserModel FindByEmailAddress(string emailAddress);

        //void AddToCart(string emailAddress, CartItemModel cart);
    }
}
