using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DealsWhat.Infrastructure.DataAccess
{
    public class ApplicationUser : IdentityUser, IUserModel
    {

        public string EmailAddress
        {
            get { return Email; }
        }

        public ICollection<CartItemModel> CartItems
        {
            get { return cartItems; }
        }

        public void AddToCart(CartItemModel cartItem)
        {
            this.cartItems.Add(cartItem);
        }

        public void RemoveFromCart(CartItemModel cartItem)
        {
            this.cartItems.Remove(cartItem);
        }

        public AddressModel ContactAddress
        {
            get; private set;
        }

        public AddressModel BillingAddress
        {
            get; private set;
        }

        public string FirstName
        {
            get; private set;
        }

        public string LastName
        {
            get; private set;
        }

        private readonly IList<CartItemModel> cartItems;

        public ApplicationUser()
        {
            Key = Guid.NewGuid().ToString();
            cartItems = new List<CartItemModel>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string Key { get; internal set; }

        public void SetContactAddress(AddressModel addressModel)
        {
            ContactAddress = addressModel;
        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

        public void SetBillingAddress(AddressModel addressModel)
        {
            BillingAddress = addressModel;
        }
    }

}
