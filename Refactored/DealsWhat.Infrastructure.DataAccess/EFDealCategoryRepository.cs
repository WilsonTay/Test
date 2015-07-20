using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFDealCategoryRepository : IRepository<DealCategoryModel>
    {
        private readonly IUnitOfWork unitOfWork;

        public EFDealCategoryRepository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DealCategoryModel> GetAll()
        {
            foreach (var category in this.unitOfWork.Set<DealCategoryModel>())
            {
                yield return category;
            }
        }

        public void Update(DealCategoryModel model)
        {
            throw new NotImplementedException();
        }

        public void Create(DealCategoryModel model)
        {
            throw new NotImplementedException();
        }

        public DealCategoryModel FindByKey(string key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
