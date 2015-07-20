using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Model;
using DealsWhat.Domain.Test.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DealsWhat.Domain.Models.Tests
{
    [TestClass]
    public class UserModelTests
    {
        [TestMethod]
        public void Create_ParametersSetCorrectly()
        {
            var emailAddress = "test@test.com";
            var username = "test@test.com";

            var user = UserModel.Create(emailAddress, username);

            user.EmailAddress.ShouldBeEquivalentTo(emailAddress);
            user.Username.ShouldBeEquivalentTo(username);
        }

        [TestMethod]
        public void Create_KeyGenerated()
        {
            var emailAddress = "test@test.com";
            var username = "test@test.com";

            var user = UserModel.Create(emailAddress, username);

            user.Key.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void SetFirstName_SetCorrect()
        {
            var firstName = "first name";
            var user = TestModelFactory.CreateUser();

            user.SetFirstName(firstName);

            user.FirstName.ShouldBeEquivalentTo(firstName);
        }

        [TestMethod]
        public void SetLastName_SetCorrect()
        {
            var lastName = "last name";
            var user = TestModelFactory.CreateUser();

            user.SetLastName(lastName);

            user.LastName.ShouldBeEquivalentTo(lastName);
        }

        [TestMethod]
        public void SetPhoneNumber_SetCorrectly()
        {
            var phoneNumber = "5435433";
            var user = TestModelFactory.CreateUser();

            user.SetPhoneNumber(phoneNumber);

            user.PhoneNumber.ShouldBeEquivalentTo(phoneNumber);
        }

        [TestMethod]
        public void SetAddresses_SetCorrectly()
        {
            var contactAddress = TestModelFactory.CreateAddress();
            var billingAddress = TestModelFactory.CreateAddress();

            var user = TestModelFactory.CreateUser();

            user.SetBillingAddress(billingAddress);
            user.SetContactAddress(contactAddress);

            user.BillingAddress.ShouldBeEquivalentTo(billingAddress);
            user.ContactAddress.ShouldBeEquivalentTo(contactAddress);
        }
    }
}
