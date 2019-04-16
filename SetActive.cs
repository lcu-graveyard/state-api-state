using LCU.Graphs.Registry.Enterprises.State;
using LCU.Presentation.State;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
using System.Linq;
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
			return await req.WithState<SetActiveRequest, LCUState>(log, async (details, reqData, state, stateMgr) =>
			{
				log.LogInformation("Refresh function processed a request.");

				if (reqData.Lookup.IsNullOrEmpty())
				{
					state.ActiveState = null;

					state.IsStateSettings = null;
				}
				else
				{
					var activeStateName = state.States.FirstOrDefault(s => s == reqData.Lookup);

					var activeStateCfgRef = stateMgr.LoadStateConfigRef(details.EnterpriseAPIKey, activeStateName);

					var activeStateCfg = await stateMgr.LoadState<LCUStateConfiguration>(activeStateCfgRef); 

					activeStateCfg.Description = activeStateCfg.Description ?? activeStateName;

					activeStateCfg.Lookup = activeStateCfg.Lookup ?? activeStateName;

					activeStateCfg.Name = activeStateCfg.Name ?? activeStateName;

					state.ActiveState = activeStateCfg;

					state.IsStateSettings = reqData.IsSettings;
				}

				return state;
			});
		}
	}
}
