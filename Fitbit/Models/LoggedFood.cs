namespace Fitbit.Models
{
    public class LoggedFood
    {
        public string AccessLevel { get; set; }
        public decimal Amount { get; set; }
        public string Brand { get; set; }
        public int Calories { get; set; }
        public long FoodId { get; set; }
        public string Locale { get; set; }
        public int MealTypeId { get; set; }
        public string Name { get; set; }
        public FoodUnit Unit { get; set; }
    }
}