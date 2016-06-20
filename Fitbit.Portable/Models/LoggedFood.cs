namespace Fitbit.Models
{
    public class LoggedFood
    {
        //Todo: Enumerate access level
        public string AccessLevel { get; set; }
        public float Amount { get; set; }
        public string Brand { get; set; }
        public float Calories { get; set; }
        public long FoodId { get; set; }
        public long MealTypeId { get; set; }
        //Todo: Map to a locale object
        public string Locale { get; set; }
        public string Name { get; set; }
        public FoodLogUnit Unit { get; set; }
    }
}