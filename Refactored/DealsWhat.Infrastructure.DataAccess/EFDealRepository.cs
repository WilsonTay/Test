using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFDealRepository : IRepository<DealModel>
    {
        private readonly IDbContext dbContext;

        public EFDealRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IEnumerable<DealModel> GetAll()
        {
            foreach (var deal in this.dbContext.Set<DealModel>().Include("Options.Attributes").Include("Images"))
            {
                yield return deal;
            }
        }

        public void Update(DealModel model)
        {
            throw new NotImplementedException();
        }

        public void Create(DealModel model)
        {
            throw new NotImplementedException();
        }

        public DealModel FindByKey(string key)
        {
            // HACK: Optimize this.
            var entity = this.dbContext.Set<DealModel>()
                .Include("Options.Attributes")
                .Include("Images")
                .ToList()
                .FirstOrDefault(u => u.Key == key);

            return entity;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        //private DealsWhat.Domain.Model.DealModel Convert(Models.Deal source)
        //{
        //    var mappedDeal = Mapper.Map<Models.Deal, DealsWhat.Domain.Model.DealModel>(source);

        //     return mappedDeal;
        //}
    }
}
