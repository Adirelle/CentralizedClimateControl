using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Powered : CompProperties_Building
    {
        public bool adaptivePowerConsumption = false;

        public float maxRateChange = 0.05f;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!parentDef.HasComp(typeof(CompPowerTrader)))
            {
                yield return "A CompPowerTrader is required";
            }

            if (!parentDef.HasComp(typeof(CompBreakdownable)))
            {
                yield return "A CompBreakdownable is required";
            }
        }
    }
}
