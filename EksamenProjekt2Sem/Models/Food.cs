namespace EksamenProjekt2Sem.Models
{
    public abstract class Food
    {
        public int Id { get; set; }
        public string Ingrediens { get; set; }
        public bool? InSeason { get; set; }
        public string? MeatType { get; set; }
        public double? Price { get; set; }
    }
}
