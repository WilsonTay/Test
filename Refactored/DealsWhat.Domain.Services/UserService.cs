using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public class UserService : IUserService
    {
        private IRepositoryFactory repositoryFactory;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public IUserModel GetUserByEmail(string emailAddress)
        {
            var user = this.repositoryFactory
                .CreateUserRepository()
                .FindByEmailAddress(emailAddress);

            return user;
        }

        public void UpdateUser(
            string emailAddress,
            UpdateUserModel updateUserModel)
        {
            var user = GetUserByEmail(emailAddress);

            if (updateUserModel.BillingAddress!=null && IsNewAddress(updateUserModel.BillingAddress))
            {
                user.SetBillingAddress(updateUserModel.BillingAddress);
            }

            if (updateUserModel.ContactAddress != null && IsNewAddress(updateUserModel.ContactAddress))
            {
                user.SetContactAddress(updateUserModel.ContactAddress);
            }

            if (!string.IsNullOrEmpty(updateUserModel.FirstName))
            {
                user.SetFirstName(updateUserModel.FirstName);
            }

            if (!string.IsNullOrEmpty(updateUserModel.LastName))
            {
                user.SetLastName(updateUserModel.LastName);
            }

            this.repositoryFactory
                 .CreateUserRepository()
                 .Save();
        }

        private static bool IsNewAddress(AddressModel address)
        {
            return !string.IsNullOrEmpty(address.City) ||
                   !string.IsNullOrEmpty(address.Line1) ||
                   !string.IsNullOrEmpty(address.Line2) ||
                   !string.IsNullOrEmpty(address.State) ||
                   !string.IsNullOrEmpty(address.PostCode) ||
                   !string.IsNullOrEmpty(address.Country);
        }
    }
}
