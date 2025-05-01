using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class UserService : GenericDbService<User>
    {
        private List<User> _users; // Overskud fra domain model
        private GenericDbService<User> _dbService; // Overskud fra domain model

        public UserService(UserService userService)
        {
            _users = new List<User>();
            _dbService = userService;
        }
        public UserService()
        {
            _users = new List<User>();
            _dbService = new GenericDbService<User>();
        }
        public void CreateUser(User user)
        {
            // Add user to Database
        }
        public User ReadUser(int id)
        {
            // Read user from Database
            return new User(); // Placeholder return
        }
        public List<User> ReadAllUsers()
        {
            // Read all users from Database
            return new List<User>(); // Placeholder return
        }
        public void UpdateUser(int id, User user)
        {
            // Update user by id in Database
        }
        public User DeleteUser(int id)
        {
            // Delete user from Database
            return new User(); // Placeholder return
        }
        public Order Order(List<OrderLine> orderlines)
        {
            // Create order from orderlines
            return new Order(); // Placeholder return
        }
    }
}
