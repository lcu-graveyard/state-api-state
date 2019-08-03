using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCU.API.State.Models;
using LCU.Graphs.Registry.Enterprises.Apps;
using LCU.Presentation.Personas.Applications;
using LCU.Presentation.State;
using LCU.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LCU.State.API.ForgePublic.Harness
{
    public class LCUIDEStateHarness : LCUStateHarness<LCUIDEState>
    {
        #region Fields
        protected readonly ApplicationManagerClient appMgr;

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LCUIDEStateHarness(HttpRequest req, ILogger log, LCUIDEState state)
            : base(req, log, state)
        {
            appMgr = req.ResolveClient<ApplicationManagerClient>(log);
        }
        #endregion

        #region API Methods
        public virtual async Task<LCUIDEState> LoadStates()
        {
            var statesResp = await appMgr.ListStates(details.EnterpriseAPIKey);

            state.States = statesResp.Model;

            return state;
        }

        public virtual async Task<LCUIDEState> Refresh()
        {
            await LoadStates();

            return state;
        }

        public virtual async Task<LCUIDEState> SaveStateConfig(LCUStateConfiguration stateCfg)
        {
            logger.LogInformation("Saving State configuration");

            var saved = await appMgr.SaveStateConfig(stateCfg, details.EnterpriseAPIKey, stateCfg.Lookup);

            state.ActiveState = stateCfg;

            await LoadStates();

            return state;
        }

        public virtual async Task<LCUIDEState> SetActive(string lookup, bool isSettings)
        {
            logger.LogInformation("Setting active State");

            if (lookup.IsNullOrEmpty())
            {
                state.ActiveState = null;

                state.IsStateSettings = null;
            }
            else
            {
                var activeStateName = state.States.FirstOrDefault(s => s == lookup);

                var activeStateCfgResp = await appMgr.LoadStateConfig(details.EnterpriseAPIKey, activeStateName);

                var activeStateCfg = activeStateCfgResp.Model;

                activeStateCfg.Description = activeStateCfg.Description ?? activeStateName;

                activeStateCfg.Lookup = activeStateCfg.Lookup ?? activeStateName;

                activeStateCfg.Name = activeStateCfg.Name ?? activeStateName;

                state.ActiveState = activeStateCfg;

                state.IsStateSettings = isSettings;
            }

            return state;
        }
        #endregion

        #region Helpers
        #endregion
    }
}