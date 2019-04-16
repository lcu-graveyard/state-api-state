using LCU.Graphs.Registry.Enterprises.State;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LCU.API.State
{
	public static class Refresh
	{
		[FunctionName("Refresh")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			return await req.WithState<dynamic, LCUState>(log, async (details, reqData, state, stateMgr) =>
			{
				var states = await stateMgr.ListStateContainers(details.EnterpriseAPIKey);

				state.States = states;

				return state;
			});
		}
	}
}
