using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Building : CompProperties
    {
        public AreaShape shape;

        public float flowPerTile = 100.0f;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (flowPerTile <= 0.0f)
            {
                yield return "flowPerTile must be strictly positive";
            }

            if (!parentDef.HasComp(typeof(CompFlickable)))
            {
                yield return "A CompFlickable is required";
            }
        }
    }
}
