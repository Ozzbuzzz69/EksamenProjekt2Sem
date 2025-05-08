namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    public class MockUser
    {
        private static List<User> _users = new List<User>()
        {
            new("Carl", "test@example.com", "12345678", "1234"),
            new("Lars", "lars@vontrier.dk", "69696969", "Nymphomaniac"),
            new("Mike", "MikeHawk@TooBig.com", "80000003", "benis"),
            new("Jarjar", "DarthJarJar@binks.ge", "19081999", "DarkSide")
        };
        public static List<User> GetUsers()
        {
            return _users;
        }
    }
}
