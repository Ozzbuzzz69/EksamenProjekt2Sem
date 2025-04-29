using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        
        public string Password { get; set; }

        public User()
        { }

        /// <summary>
        /// Constructor for User class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        public User(int id, string name, string email, string phoneNumber, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }

    }
}
