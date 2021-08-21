using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_TempControl : CompProperties_Powered
    {
        public float thermalCapacity;

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

            if (!(thermalCapacity > 0))
            {
                yield return "thermalCapacity must be strictly positive";
            }
        }
    }
}
