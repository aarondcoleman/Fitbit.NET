namespace Fitbit.Api.Portable.Models
{
	public class ActivitiesList
	{
		public ActivityDetail[] Activities { get; set; }

		public Pagination Pagination { get; set; }
	}
}
