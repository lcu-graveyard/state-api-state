using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LCU.API.State.Models;
using LCU.Graphs.Registry.Enterprises.Apps;
using LCU.Personas.Client.Applications;
using LCU.Presentation.State;
using LCU.StateAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LCU.State.API.ForgePublic.Harness
{
    public class LCUStateHarness : LCUStateHarness<LCUState>
    {
        #region Fields
        protected readonly ApplicationManagerClient appMgr;

        #endregion

        #region Properties

        #endregion

        #region Constructors
        public LCUStateHarness(HttpRequest req, ILogger log, LCUState state)
            : base(req, log, state)
        {
            appMgr = req.ResolveClient<ApplicationManagerClient>(log);
        }
        #endregion

        #region API Methods
        public virtual async Task<LCUState> LoadStates()
        {
            var statesResp = await appMgr.ListStates(details.EnterpriseAPIKey);

            state.States = statesResp.Model;

            return state;
        }

        public virtual async Task<LCUState> Refresh()
        {
            await LoadStates();

            return state;
        }

        public virtual async Task<LCUState> SaveStateConfig(LCUStateConfiguration stateCfg)
        {
            logger.LogInformation("Saving State configuration");

            var saved = await appMgr.SaveStateConfig(stateCfg, details.EnterpriseAPIKey, stateCfg.Lookup);

            state.ActiveState = stateCfg;

            await LoadStates();

            return state;
        }

        public virtual async Task<LCUState> SetActive(string lookup, bool isSettings)
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