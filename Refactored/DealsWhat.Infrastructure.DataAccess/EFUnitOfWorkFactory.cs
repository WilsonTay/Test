using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new DealsWhatDbContext());
        }
    }
}
