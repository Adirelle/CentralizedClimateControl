using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompProperties_Powered : CompProperties_Building
    {
        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!parentDef.HasComp(typeof(CompPowerTrader)))
            {
                yield return $"{compClass.Name} requires a CompPowerTrader";
            }

            if (!parentDef.HasComp(typeof(CompBreakdownable)))
            {
                yield return $"{compClass.Name} requires a CompBreakdownable";
            }
        }
    }
}
