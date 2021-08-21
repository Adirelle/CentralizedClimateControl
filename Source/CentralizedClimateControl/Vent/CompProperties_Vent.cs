using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Vent : CompProperties_Building
    {
        public float baseAirExhaust;

        public CompProperties_Vent()
        {
            compClass = typeof(CompVent);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!(baseAirExhaust > 0))
            {
                yield return "baseAirExhaust must be strictly positive";
            }
        }
    }
}
