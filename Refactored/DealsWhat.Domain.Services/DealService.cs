using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class DealService : IDealService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public DealService(IUnitOfWorkFactory unitOfWorkFactory)
        {

            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IEnumerable<DealModel> SearchDeals(DealSearchQuery query)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();

            IEnumerable<DealModel> deals;

            if (query.CategoryId != null)
            {
                var category =
                         unitOfWork.CreateDealCategoryRepository()
                        .GetAll()
                        .FirstOrDefault(c => c.Key.ToString().Equals(query.CategoryId));

                if (category == null)
                {
                    return new List<DealModel>();
                }

                deals = category.Deals;
            }
            else
            {
                deals = unitOfWork.CreateDealRepository().GetAll();
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {

                deals = deals
                    .Where(d =>
                            ContainsIgnoreCase(d.ShortTitle, query.SearchTerm) ||
                            ContainsIgnoreCase(d.ShortDescription, query.SearchTerm) ||
                            ContainsIgnoreCase(d.LongTitle, query.SearchTerm) ||
                            ContainsIgnoreCase(d.LongDescription, query.SearchTerm))
                    .ToList();
            }

            return deals.Where(d => d.Status == DealStatus.Published);
        }

        private static bool ContainsIgnoreCase(string compared, string searchTerm)
        {
            return compared.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// We could actually make single and multiple deals grouped under the same method but
        /// due to performance considerations it's wiser to split them into two.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DealModel SearchSingleDeal(SingleDealSearchQuery query)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateDealRepository();

            if (!string.IsNullOrEmpty(query.Id))
            {
                return repository.GetAll().FirstOrDefault(d => d.Key.ToString().Equals(query.Id));
            }

            if (!string.IsNullOrEmpty(query.CanonicalUrl))
            {
                return repository.GetAll().FirstOrDefault(d => d.CanonicalUrl.Equals(query.CanonicalUrl));
            }

            return null;
        }

        public IEnumerable<DealCategoryModel> GetAllCategories()
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateDealCategoryRepository();

            return repository.GetAll();
        }
    }
}
