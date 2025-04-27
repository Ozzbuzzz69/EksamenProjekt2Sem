using EksamenProjekt2Sem.Models;
using System.Collections.Generic;

namespace EksamenProjekt2Sem.Services
{
    public class DBContext
    {
        public DBSet<Sandwich> Sandwiches { get; set; }
        public DBSet<Order> Orders { get; set; }
        public DBSet<User> Users { get; set; }
    }
}
