using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitbit.Models
{
	/*return info from http://api.fitbit.com/1/user/-/profile.xml */
	/* 
	 "{\"user\":
		{
		\"dateOfBirth\":\"1983-05-08\",
		\"displayName\":\"aaroninsd\",
		\"encodedId\":\"2242TQ\",
		\"gender\":\"MALE\",
		\"height\":172.8,
		\"offsetFromUTCMillis\":-25200000,
		\"strideLengthRunning\":0,
		\"strideLengthWalking\":0,
		\"timezone\":\"America/Los_Angeles\",
		\"weight\":72.12
		}
	}"
	 */

	public class User
	{
		public DateTime dateOfBirth
		{
			get;
			set;
		}

		public string displayName
		{
			get;
			set;
		}

		public string encodedId
		{
			get;
			set;
		}

		public string gender
		{
			get;
			set;
		}

		public decimal height
		{
			get;
			set;
		}

		public long offsetFromUTCMillis
		{
			get;
			set;
		}

		public decimal weight
		{
			get;
			set;
		}


	}
}