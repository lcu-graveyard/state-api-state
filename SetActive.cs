using LCU.API.State.Models;
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
	public class SetActiveRequest
	{
		[DataMember]
		public virtual bool IsSettings { get; set; }

		[DataMember]
		public virtual string Lookup { get; set; }
	}

	public static class SetActive
	{
		[FunctionName("SetActive")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			return await req.Manage<SetActiveRequest, LCUState, LCUStateHarness>(log, async (mgr, reqData) =>
            {
				log.LogInformation($"Setting Active: {reqData.Lookup} {reqData.IsSettings}");

                return await mgr.SetActive(reqData.Lookup, reqData.IsSettings);
            });
		}
	}
}
