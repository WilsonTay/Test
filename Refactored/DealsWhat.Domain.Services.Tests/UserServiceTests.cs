using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DealsWhat.Domain.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private IFixture fixture;
        private string emailAddress = "email@email.com";

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());

            fixture.Register<IRepositoryFactory>(() => fixture.Create<FakeRepositoryFactory>());
        }

        [TestMethod]
        public void GetUser_RetrieveUserFromRepository()
        {
            var key = "key";
            var email = "email@email.com";
            var mockedRepository = new Mock<IUserRepository>();
            var sampleUser = TestModelFactory.CreateUser(key, email);

            mockedRepository.Setup(a => a.FindByEmailAddress(email)).Returns(sampleUser);

            var service = new UserService(new FakeRepositoryFactory(userRepository: mockedRepository.Object));
            var actualUser = service.GetUserByEmail(email);

            actualUser.Key.ShouldBeEquivalentTo(key);
            actualUser.EmailAddress.ShouldBeEquivalentTo(email);
        }

        [TestMethod]
        public void GetUser_RetrieveUserFromRepository_WithAddress()
        {
            var key = "key";
            var email = "email@email.com";
            var mockedRepository = new Mock<IUserRepository>();
            var contactAddress = TestModelFactory.CreateAddress();
            var billingAddress = TestModelFactory.CreateAddress();
            var sampleUser = TestModelFactory.CreateUser(key, email, contactAddress, billingAddress);

            mockedRepository.Setup(a => a.FindByEmailAddress(email)).Returns(sampleUser);

            var service = new UserService(new FakeRepositoryFactory(userRepository: mockedRepository.Object));
            var actualUser = service.GetUserByEmail(email);

            actualUser.Key.ShouldBeEquivalentTo(key);
            actualUser.EmailAddress.ShouldBeEquivalentTo(email);
            actualUser.ContactAddress.ShouldBeEquivalentTo(contactAddress);
            actualUser.BillingAddress.ShouldBeEquivalentTo(billingAddress);
        }

        [TestMethod]
        public void UpdateUser_AddressesUpdatedCorrectly()
        {
            var email = "email@email.com";
            var billingAddress = TestModelFactory.CreateAddress();
            var contactAddress = TestModelFactory.CreateAddress();
            var sampleUser = TestModelFactory.CreateUser(emailAddress: email, contactAddress: contactAddress, billingAddress: billingAddress);
            var service = CreateUserService(new List<UserModel>() { sampleUser });

            var returnedUser = service.GetUserByEmail(email);

            returnedUser.BillingAddress.ShouldBeEquivalentTo(billingAddress);
            returnedUser.ContactAddress.ShouldBeEquivalentTo(contactAddress);

            var newBillingAddress = TestModelFactory.CreateAddress();
            var newContactAddress = TestModelFactory.CreateAddress();

            var updateUserModel = new UpdateUserModel
            {
                BillingAddress = newBillingAddress,
                ContactAddress = newContactAddress
            };

            service.UpdateUser(email, updateUserModel);

            returnedUser = service.GetUserByEmail(email);

            returnedUser.BillingAddress.ShouldBeEquivalentTo(newBillingAddress);
            returnedUser.ContactAddress.ShouldBeEquivalentTo(newContactAddress);
        }

        private UserService CreateUserService(
           List<UserModel> users = null,
           IUserRepository userRepository = null)
        {
            if (userRepository == null)
            {
                userRepository = new FakeUserRepository(users ?? new List<UserModel>());
            }

            fixture.Register<IUserRepository>(() => userRepository);

            var fakeRepositoryFactory = fixture.Create<FakeRepositoryFactory>();
            fixture.Register<IRepositoryFactory>(() => fakeRepositoryFactory);

            var userService = fixture.Create<UserService>();

            return userService;
        }
    }
}
