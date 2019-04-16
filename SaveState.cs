using LCU.Graphs.Registry.Enterprises.State;
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
	public class SaveStateRequest
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
			return await req.WithState<SaveStateRequest, LCUState>(log, async (details, reqData, state, stateMgr) =>
			{
				log.LogInformation("Save State function processed a request.");

				var stateConfigRef = stateMgr.LoadStateConfigRef(details.EnterpriseAPIKey, reqData.Config.Lookup);

				await stateMgr.SaveState(stateConfigRef, reqData.Config);

				state.States = await stateMgr.ListStateContainers(details.EnterpriseAPIKey);

				state.ActiveState = reqData.Config;

				return state;
			});
		}
	}
}
