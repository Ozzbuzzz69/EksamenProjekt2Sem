namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    using Microsoft.AspNetCore.Identity;

    public class MockOrder
    {

        private static List<OrderLine> _orderLines = new List<OrderLine>()
        {
            new OrderLine(2, MockFood.GetSandwiches()[1]),
            new OrderLine(1, MockFood.GetSandwiches()[0]),
            new OrderLine(1, MockFood.GetSandwiches()[2]),
            new OrderLine(1, MockFood.GetWarmMeals()[3]),
        };
        private static List<OrderLine> _orderLines1 = new List<OrderLine>()
        {
            new OrderLine(1, MockFood.GetSandwiches()[1]),
            new OrderLine(2, MockFood.GetWarmMeals()[0]),
            new OrderLine(3, MockFood.GetSandwiches()[2])
        };
        private static List<OrderLine> _orderLines2 = new List<OrderLine>()
        {
            new OrderLine(1, MockFood.GetWarmMeals()[1]),
            new OrderLine(1, MockFood.GetSandwiches()[0]),
            new OrderLine(2, MockFood.GetWarmMeals()[2]),
            new OrderLine(4, MockFood.GetSandwiches()[3]),
        };
        private static List<Order> _orders = new List<Order>()
        {
            new(MockUser.GetMockUsers()[17], new DateTime(3000, 2, 1), _orderLines)
            {

            },
            new(MockUser.GetMockUsers()[18], new DateTime(3000, 3, 1), _orderLines1)
            {
            },
            new(MockUser.GetMockUsers()[19], new DateTime(3000,1,1), _orderLines2)
            {
            },
            new(MockUser.GetMockUsers()[20], new DateTime(3000, 4, 1), _orderLines2)
            {
            }
        };
        public static List<Order> GetOrders()
        {
            return _orders;
        }
    }
}