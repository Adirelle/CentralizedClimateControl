using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_NetworkPart : CompProperties
    {
        public FlowType flowType = FlowType.Any;

        public CompProperties_NetworkPart()
        {
            compClass = typeof(CompNetworkPart);
        }
    }
}
