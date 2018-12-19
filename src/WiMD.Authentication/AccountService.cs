
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
            User newUser = _userFactory.CreateUser(user.Email, user.Password);
            User createdUser = _userRepository.Create(newUser);

            createdUser.GenerateToken();
            return createdUser;
        }

        public User LogIn(string email, string password)
        {
            User user = _userRepository.Get(email);
            user.ValidateGivenPassword(password);
            user.GenerateToken();

            return user;
        }    
    }
}