using RimWorld;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompProperties_Building : CompProperties
    {
        public AreaShape shape;

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (!parentDef.HasComp(typeof(CompFlickable)))
            {
                yield return $"{compClass.Name} requires a CompFlickable";
            }
        }
    }
}
