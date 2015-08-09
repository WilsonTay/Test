using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class EFUserRepository : IUserRepository
    {
        private readonly IDbContext dbContext;

        public EFUserRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IEnumerable<IUserModel> GetAll()
        {
            foreach (var user in this.dbContext.Set<ApplicationUser>())
            {
                yield return user;
            }
        }

        public void Update(IUserModel model)
        {
            this.dbContext.Update(model);
        }

        public void Create(IUserModel model)
        {
            throw new NotImplementedException();
        }

        public IUserModel FindByKey(string key)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.dbContext.Commit();
        }

        public IUserModel FindByEmailAddress(string emailAddress)
        {
            // TODO: Null cart.
            var entity = this.dbContext.Set<ApplicationUser>()
                .Include("CartItems.Deal")
                .Include("CartItems.Deal.Images")
                .Include("CartItems.DealOption")
                .Include("CartItems.AttributeValues")
                .Include("Orders.Orderlines.AttributeValues")
                .Include("Orders.Orderlines.Deal")
                .Include("Orders.Orderlines.Deal.Images")
                .Include("Orders.Orderlines.DealOption")
                .Include("Orders.BillingAddress")
                .Include("BillingAddress")
                .FirstOrDefault(u => u.Email == emailAddress);

            return entity;
        }

        //public void AddToCart(string emailAddress, CartItemModel cart)
        //{
        //    var attrIds = cart.AttributeValues.Select(d => Guid.Parse(d.Key.ToString()));
        //    var dealOptionGuid = Guid.Parse(cart.DealOption.Key.ToString());

        //    var dealOption = this.unitOfWork.Set<DealOptionModel>().First(d => d.Id == dealOptionGuid);
        //    var attributes = this.unitOfWork.Set<DealAttributeModel>().Where(d => attrIds.Contains(d.Id)).ToList();
        //    var entity = new Cart
        //    {
        //        DealAttributes = attributes,
        //        DealOption = dealOption,
        //        Quantity = 1,
        //        Id = Guid.Parse(cart.Key.ToString())
        //    };

        //    //var mappedCart = Mapper.Map<DealsWhat.Domain.Model.CartItemModel, Models.Cart>(cart);
        //    var user = this.unitOfWork.Set<Models.User>().FirstOrDefault(u => u.Email.Equals(emailAddress));
        //    user.Carts.Add(entity);
        //}

        //private DealsWhat.Domain.Model.UserModel Convert(Models.User source)
        //{
        //    var mappedDeal = Mapper.Map<Models.User, DealsWhat.Domain.Model.UserModel>(source);

        //    return mappedDeal;
        //}
    }
}
