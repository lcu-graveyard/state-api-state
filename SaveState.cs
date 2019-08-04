using LCU.API.State.Models;
using LCU.Presentation.State;
using LCU.State.API.ForgePublic.Harness;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace LCU.API.State
{
	[Serializable]
	[DataContract]
	public class SaveStateConfigRequest
	{
		[DataMember]
		public virtual LCUStateConfiguration Config { get; set; }
	}

	public static class SaveState
	{
		[FunctionName("SaveState")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			return await req.Manage<SaveStateConfigRequest, LCUState, LCUStateHarness>(log, async (mgr, reqData) =>
            {
                return await mgr.SaveStateConfig(reqData.Config);
            });
		}
	}
}
