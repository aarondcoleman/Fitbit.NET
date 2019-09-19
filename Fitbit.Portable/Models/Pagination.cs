namespace Fitbit.Api.Portable.Models
{
	public class Pagination
	{
		public string BeforeDate { get; set; }
		public int Limit { get; set; }
		public string Next { get; set; }
		public int Offset { get; set; }
		public string Previous { get; set; }
		public string Sort { get; set; }
	}
}
