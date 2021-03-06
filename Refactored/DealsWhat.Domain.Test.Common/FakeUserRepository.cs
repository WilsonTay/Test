﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsWhat.Domain.Interfaces;
using DealsWhat.Domain.Model;

namespace DealsWhat.Domain.Test.Common
{
    public class FakeUserRepository : IUserRepository
    {
        private IList<UserModel> users;

        public FakeUserRepository(IList<UserModel> users)
        {
            this.users = users;
        }

        public IEnumerable<UserModel> GetAll()
        {
            return this.users;
        }

        public void Update(IUserModel model)
        {
            throw new NotImplementedException();
        }

        public void Create(IUserModel model)
        {
            throw new NotImplementedException();
        }

        IUserModel IRepository<IUserModel>.FindByKey(string key)
        {
            return FindByKey(key);
        }

        public void Update(UserModel model)
        {
         
        }

        public void Create(UserModel model)
        {
            this.users.Add(model);
        }

        IEnumerable<IUserModel> IRepository<IUserModel>.GetAll()
        {
            return GetAll();
        }

        public UserModel FindByKey(object key)
        {
            return users.First(a => a.Key.ToString().Equals(key.ToString()));
        }

        public void Save()
        {
        }

        IUserModel IUserRepository.FindByEmailAddress(string emailAddress)
        {
            return FindByEmailAddress(emailAddress);
        }

        public UserModel FindByEmailAddress(string emailAddress)
        {
            return users.First(a => a.EmailAddress.ToString().Equals(emailAddress));
        }

        public void AddToCart(string emailAddress, CartItemModel cart)
        {
            throw new NotImplementedException();
        }
    }
}
