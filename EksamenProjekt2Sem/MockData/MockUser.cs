namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    using Microsoft.AspNetCore.Identity;

    public class MockUser
    {
        private static PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        private static List<User> _users = new List<User>() { 
            new User("Carl", "test@example.com", "12345678", passwordHasher.HashPassword(null, "1234")),
            new User("Lars", "lars@vontrier.dk", "69696969", passwordHasher.HashPassword(null, "Nymphomaniac")),
            new User("Mike", "MikeHawk@TooBig.com", "80000003", passwordHasher.HashPassword(null, "benis")),
            new User("Jarjar", "DarthJarJar@binks.ge", "19081999", passwordHasher.HashPassword(null, "DarkSide")),
            new User("Admin", "admin@admin.com", "55555555", passwordHasher.HashPassword(null, "admin"))
        };
        public static List<User> GetMockUsers()
        {
            return _users;
        }
    }
}