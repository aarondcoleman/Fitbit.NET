using Newtonsoft.Json;
using System;

namespace Fitbit.Models
{
    public class FoodLog
    {
        public bool IsFavorite { get; set; }
        public DateTime LogDate { get; set; }
        public long LogId { get; set; }
        public LoggedFood LoggedFood { get; set; }
        public NutritionalValues NutritionalValues { get; set; }        
    }
}