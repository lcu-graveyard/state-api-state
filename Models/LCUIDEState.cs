using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LCU.Presentation.State;

namespace LCU.API.State.Models
{
	[Serializable]
	[DataContract]
	public class LCUIDEState
	{
		[DataMember]
		public virtual LCUStateConfiguration ActiveState { get; set; }

		[DataMember]
		public virtual bool? IsStateSettings { get; set; }

		[DataMember]
		public virtual bool Loading { get; set; }

		[DataMember]
		public virtual List<string> States { get; set; }
	}
}
