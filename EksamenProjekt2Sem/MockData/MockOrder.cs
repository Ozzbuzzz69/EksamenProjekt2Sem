namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    public class MockOrder
    {
        
        private static List<OrderLine> _orderLines = new List<OrderLine>()
        {
            new OrderLine(2, MockFood.GetSandwiches()[1], null),
            new OrderLine(1, MockFood.GetSandwiches()[0], null),
            new OrderLine(1, MockFood.GetSandwiches()[2], null),
            new OrderLine(1, MockFood.GetWarmMeals()[3], null),
        };
        private static List<OrderLine> _orderLines1 = new List<OrderLine>()
        {
            new OrderLine(1, MockFood.GetSandwiches()[1], null),
            new OrderLine(2, MockFood.GetWarmMeals()[0], null),
            new OrderLine(3, MockFood.GetSandwiches()[2], null)
        };
        private static List<OrderLine> _orderLines2 = new List<OrderLine>()
        {
            new OrderLine(1, MockFood.GetWarmMeals()[1], null),
            new OrderLine(1, MockFood.GetSandwiches()[0], null),
            new OrderLine(2, MockFood.GetWarmMeals()[2], null),
            new OrderLine(4, MockFood.GetSandwiches()[3], null),
        };
        private static List<Order> _orders = new List<Order>()
        {
            new(MockUser.GetMockUsers()[0], new DateTime(3000, 2, 1))
            {
                OrderLines = _orderLines
            },
            new(MockUser.GetMockUsers()[1], new DateTime(3000, 3, 1))
            {
                OrderLines = _orderLines1
            },
            new(MockUser.GetMockUsers()[2], new DateTime(3000,1,1))
            {
                OrderLines = _orderLines2
            },
            new(MockUser.GetMockUsers()[3], new DateTime(3000, 4, 1))
            {
                OrderLines = _orderLines
            }
        };
        public static List<Order> GetOrders()
        {
            return _orders;
        }
    }
}