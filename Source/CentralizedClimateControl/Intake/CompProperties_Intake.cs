using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Intake : CompProperties_Powered
    {
        public float baseAirIntake;

        public CompProperties_Intake()
        {
            compClass = typeof(CompIntake);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!(baseAirIntake > 0))
            {
                yield return "baseAirIntake must be strictly positive";
            }
        }
    }
}
