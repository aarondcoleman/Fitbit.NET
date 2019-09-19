using System;
using Fitbit.Models;

namespace Fitbit.Api.Portable.Models
{
	public class ActivityDetail
	{
		public long ActiveDuration { get; set; }

		public ActivityLevel[] ActivityLevel { get; set; }

		public string ActivityName { get; set; }

		public long ActivityTypeId { get; set; }

		public long? AverageHeartRate { get; set; }

		public long Calories { get; set; }

		public long Duration { get; set; }

		public Uri HeartRateLink { get; set; }

		public HeartRateZone[] HeartRateZones { get; set; }

		public DateTimeOffset LastModified { get; set; }

		public long LogId { get; set; }

		public string LogType { get; set; }

		public ManualValuesSpecified ManualValuesSpecified { get; set; }

		public long OriginalDuration { get; set; }

		public DateTimeOffset OriginalStartTime { get; set; }

		public DateTimeOffset StartTime { get; set; }

		public long? Steps { get; set; }

		public Uri TcxLink { get; set; }

		public double? ElevationGain { get; set; }
	}
}