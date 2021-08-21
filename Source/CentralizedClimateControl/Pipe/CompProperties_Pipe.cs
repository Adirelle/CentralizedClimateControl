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

            if (flowType != FlowType.Blue && flowType != FlowType.Cyan && flowType != FlowType.Red)
            {
                yield return $"flowType must be a basic type, not {flowType}";
            }
        }
    }
}
