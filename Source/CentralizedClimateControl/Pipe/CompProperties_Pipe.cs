using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Pipe : CompProperties
    {
        public FlowType flowType;

        public CompProperties_Pipe() : base()
        {
            compClass = typeof(CompPipe);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (var error in base.ConfigErrors(parentDef))
            {
                yield return error;
            }

            if (flowType != FlowType.Cold && flowType != FlowType.Frozen && flowType != FlowType.Hot)
            {
                yield return $"flowType must be a basic type, not {flowType}";
            }
        }
    }
}
