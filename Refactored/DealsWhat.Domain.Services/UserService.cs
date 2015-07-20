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

            if (updateUserModel.NewBillingAddress != null)
            {
                user.SetBillingAddress(updateUserModel.NewBillingAddress);
            }

            if (updateUserModel.NewContactAddress != null)
            {
                user.SetContactAddress(updateUserModel.NewContactAddress);
            }

            this.repositoryFactory
                 .CreateUserRepository()
                 .Save();
        }
    }
}
