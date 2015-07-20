using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsWhat.Domain.Model
{
    public interface IUserModel : IAggregateRoot, IEntity
    {
        string EmailAddress { get; }

        ICollection<CartItemModel> CartItems { get; }

        void AddToCart(CartItemModel cartItem);

        void RemoveFromCart(CartItemModel cartItem);

        AddressModel ContactAddress { get; }
        AddressModel BillingAddress { get; }

        string FirstName { get; }

        string LastName { get; }

        void SetBillingAddress(AddressModel billingAddress);

        void SetContactAddress(AddressModel contactAddress);

        void SetFirstName(string firstName);

        void SetLastName(string lastName);
    }

    public class UserModel : IUserModel
    {
        public string EmailAddress { get; private set; }

        public ICollection<CartItemModel> CartItems
        {
            get { return cartItems; }
        }

        private readonly IList<CartItemModel> cartItems;

        private UserModel()
        {
            cartItems = new List<CartItemModel>();
        }

        public static UserModel Create(string emailAddress, string username)
        {
            return new UserModel
            {
                EmailAddress = emailAddress,
                Username = username,
                Key = Guid.NewGuid().ToString()
            };
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

        public string Key { get; internal set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

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

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
