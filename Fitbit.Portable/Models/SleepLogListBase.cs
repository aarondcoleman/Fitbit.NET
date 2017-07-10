namespace Fitbit.Api.Portable.Models
{
    public class SleepLogListBase
    {
        public Pagination Pagination { get; set; }
        //Shortcut property to allow user to check for additional paged data
        public bool HasMorePages => Pagination?.Next != null;
        public SleepLogDateRange[] Sleep { get; set; }
    }
}