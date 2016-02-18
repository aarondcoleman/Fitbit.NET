using System;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    internal static class FitbitClientExtensions
    {
        internal static void ProcessSleepData(SleepData sleepData)
        {
            if ((sleepData != null) && (sleepData.Sleep != null))
            {
                foreach (var sleepLog in sleepData.Sleep)
                {
                    if (sleepLog.MinuteData == null)
                        continue;

                    int startSleepSeconds = sleepLog.StartTime.ToElapsedSeconds();

                    for (int i = 0; i < sleepLog.MinuteData.Count; i++)
                    {
                        //work with a local minute data info instance
                        var minuteData = sleepLog.MinuteData[i];

                        // determine how many seconds have elapsed since mightnight in the date
                        int currentSeconds = minuteData.DateTime.ToElapsedSeconds();

                        // if the current time is post midnight then we've clicked over to the next day
                        int daysToAdd = (currentSeconds < startSleepSeconds) ? 1 : 0;
                        DateTime derivedDate = sleepLog.StartTime.AddDays(daysToAdd);

                        // update the time value with the updated asociated date of the sleep log
                        sleepLog.MinuteData[i].DateTime = new DateTime(
                                                        derivedDate.Year,
                                                        derivedDate.Month,
                                                        derivedDate.Day,
                                                        minuteData.DateTime.Hour,
                                                        minuteData.DateTime.Minute,
                                                        minuteData.DateTime.Second);
                    }
                }
            }
        }
    }
}