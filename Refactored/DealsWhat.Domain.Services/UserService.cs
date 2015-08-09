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
        private IUnitOfWorkFactory unitOfWorkFactory;

        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IUserModel GetUserByEmail(string emailAddress)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var user = unitOfWork
                .CreateUserRepository()
                .FindByEmailAddress(emailAddress);

            return user;
        }

        public void UpdateUser(
            string emailAddress,
            UpdateUserModel updateUserModel)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var repository = unitOfWork.CreateUserRepository();
            var user = repository.FindByEmailAddress(emailAddress);

            if (updateUserModel.BillingAddress != null && IsNewAddress(updateUserModel.BillingAddress))
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

            repository.Save();
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
