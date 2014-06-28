namespace Fitbit.Models
{
    public class ActivitiesStats
    {
        public Best best { get; set; }
        public Lifetime lifetime { get; set; }
    }

    public class Best
    {
        public Total total { get; set; }
        public Tracker tracker { get; set; }
    }

    public class Total
    {
        public Caloriesout caloriesOut { get; set; }
        public Distance distance { get; set; }
        public Floors floors { get; set; }
        public Steps steps { get; set; }
    }

    public class Caloriesout
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Distance
    {
        public string date { get; set; }
        public float value { get; set; }
    }

    public class Floors
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Steps
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Tracker
    {
        public Caloriesout1 caloriesOut { get; set; }
        public Distance1 distance { get; set; }
        public Floors1 floors { get; set; }
        public Steps1 steps { get; set; }
    }

    public class Caloriesout1
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Distance1
    {
        public string date { get; set; }
        public float value { get; set; }
    }

    public class Floors1
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Steps1
    {
        public string date { get; set; }
        public int value { get; set; }
    }

    public class Lifetime
    {
        public Total1 total { get; set; }
        public Tracker1 tracker { get; set; }
    }

    public class Total1
    {
        public int activeScore { get; set; }
        public int caloriesOut { get; set; }
        public float distance { get; set; }
        public int floors { get; set; }
        public int steps { get; set; }
    }

    public class Tracker1
    {
        public int activeScore { get; set; }
        public int caloriesOut { get; set; }
        public float distance { get; set; }
        public int floors { get; set; }
        public int steps { get; set; }
    }
}
