using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_TempControl : CompProperties_Powered
    {
        public float baseAirThroughput;

        public float maxTempChange = 10.0f;

        public CompProperties_TempControl()
        {
            compClass = typeof(CompTempControl);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!parentDef.HasComp(typeof(RimWorld.CompTempControl)))
            {
                yield return $"{compClass.Name} requires a RimWorld.CompTempControl";
            }

            if (!(baseAirThroughput > 0))
            {
                yield return "baseAirThroughput must be strictly positive";
            }

            if (!(maxTempChange > 0))
            {
                yield return "maxTempChange must be strictly positive";
            }
        }
    }
}
