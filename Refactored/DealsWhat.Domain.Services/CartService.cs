using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public CartService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void AddCartItem(string emailAddress, NewCartItemModel model)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateUserRepository();
            var dealRepository = unitOfWork.CreateDealRepository();

            var user = repository.FindByEmailAddress(emailAddress);
            var deal = dealRepository.FindByKey(model.DealId);

            var dealOption = deal.Options.First(d => d.Key.ToString().Equals(model.DealOptionId));
            var attributes = dealOption.Attributes.Where(d => model.SelectedAttributes.Contains(d.Key.ToString())).ToList();

            var cartItem = CartItemModel.Create(deal, dealOption, attributes);

            user.AddToCart(cartItem);

            repository.Save();
        }

        public IEnumerable<CartItemModel> GetCartItems(string emailAddress)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);

            return user.CartItems.ToList();
        }

        public void UpdateCartItem(string emailAddress, UpdateCartItemModel cartItemModel)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);
            var cartItem = user.CartItems.FirstOrDefault(c => c.Key == cartItemModel.Key);

            cartItem.SetQuantity(cartItemModel.Quantity);

            repository.Save();
        }

        public void RemoveCartItem(string emailAddress, string cartItemId)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateUserRepository();

            var user = repository.FindByEmailAddress(emailAddress);
            var toRemove = user.CartItems.FirstOrDefault(c => c.Key.ToString().Equals(cartItemId));
          
            user.RemoveFromCart(toRemove);

            try
            {
                repository.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
