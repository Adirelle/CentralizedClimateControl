using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Powered : CompProperties_Building
    {
        public float energyEfficiency = 1.0f;

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

            if (energyEfficiency <= 0.0f)
            {
                yield return "enerfyEfficiency must be strictly positive";
            }
        }
    }
}
