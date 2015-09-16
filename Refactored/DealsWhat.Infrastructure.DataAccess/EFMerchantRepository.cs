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
    public class EFMerchantRepository : IRepository<MerchantModel>
    {
        private readonly IDbContext dbContext;

        public EFMerchantRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IEnumerable<MerchantModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(MerchantModel model)
        {
            throw new NotImplementedException();
        }

        public void Create(MerchantModel model)
        {
            throw new NotImplementedException();
        }

        public MerchantModel FindByKey(string key)
        {
           var merchant= this.dbContext.Set<MerchantModel>()
                .Include("Deals.Options.Attributes")
                .Include("Deals.Images")
                .FirstOrDefault(u => u.Key == key);

            return merchant;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
