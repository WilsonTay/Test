using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Services
{
    public interface IUserService
    {
        IUserModel GetUserByEmail(string emailAddress);

        void UpdateUser(
            string emailAddress, 
            UpdateUserModel updateUserModel);
    }
}