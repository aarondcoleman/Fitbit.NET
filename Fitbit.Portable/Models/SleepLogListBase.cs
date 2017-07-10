namespace Fitbit.Api.Portable.Models
{
    public class SleepLogListBase
    {
        public Pagination Pagination { get; set; }
        public SleepLogDateRange[] Sleep { get; set; }
    }
}