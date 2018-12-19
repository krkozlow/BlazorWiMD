using System;

namespace WiMD.Authentication
{
    public class AccountService : IAccountService
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;

        public AccountService(IUserFactory userFactory, IUserRepository userRepository)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
        }

        public User SignIn(User user)
        {
            ValidateSignIn(user);
            User newUser = _userFactory.CreateUser(user.FirstName, user.LastName, user.Email, user.Password);
            User createdUser = _userRepository.Create(newUser);
            //createdUser.Password = null; to be done with real db

            return createdUser;
        }

        public User LogIn(string email, string password)
        {
            User user = _userRepository.Get(email);
            //user.ValidateGivenPassword(password); to do fix bug
            user.GenerateToken();
            //user.Password = null; to be done with real db

            return user;
        }

        private void ValidateSignIn(User user)
        {
            if (_userRepository.Get(user.Email) != null)
            {
                throw new ArgumentException("User with that email already exist!.");
            }
        }
    }
}