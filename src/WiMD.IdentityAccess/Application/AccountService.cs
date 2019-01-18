using System;
using System.Collections.Generic;
using System.Text;
using WiMD.IdentityAccess.Domain.Model;
using WiMD.IdentityAccess.Infrastructure;

namespace WiMD.IdentityAccess.Application
{
    public class AccountService : IAccountService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AccountService(IUserFactory userFactory, IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public User SignIn(User user)
        {
            ValidateSignIn(user);
            User newUser = _userFactory.CreateUser(user.FirstName, user.LastName, user.Email, user.Password);
            User createdUser = _userRepository.Create(newUser);
            createdUser.Password = null;

            return createdUser;
        }

        public User LogIn(string email, string password)
        {
            User user = _userRepository.Get(email);
            ValidateGivenPassword(user, password);

            user.Token = _jwtTokenProvider.CreateJwtToken(user.Email);
            user.Password = null;

            return user;
        }

        private void ValidateSignIn(User user)
        {
            if (_userRepository.Get(user.Email) != null)
            {
                throw new ArgumentException("User with that email already exist!.");
            }
        }
        private void ValidateGivenPassword(User user, string givenPassword)
        {
            if (CryptographyHelper.HashPassword(givenPassword) != user.Password)
            {
                throw new ArgumentException("Given password is not valid.");
            }
        }
    }
}
