namespace Fitbit.Models
{
    public class ApiError
    {
        public ErrorType ErrorType { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}