using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Building : CompProperties
    {
        public AreaShape shape;

        public float baseThroughput;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (baseThroughput <= 0.0f)
            {
                yield return "baseThroughput must be strictly positive";
            }

            if (!parentDef.HasComp(typeof(CompFlickable)))
            {
                yield return "A CompFlickable is required";
            }
        }
    }
}
