using LCU.API.State.Models;
using LCU.State.API.ForgePublic.Harness;
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
			return await req.Manage<dynamic, LCUIDEState, LCUIDEStateHarness>(log, async (mgr, reqData) =>
            {
                return await mgr.Refresh();
            });
		}
	}
}
