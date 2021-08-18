using Verse;
using System.Linq;

namespace CentralizedClimateControl
{
    public class PlaceWorker_Pipe : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            var pipeProps = (def as ThingDef)?.GetCompProperties<CompProperties_Pipe>();
            if (pipeProps == null)
            {
                return true;
            }
            var pipeType = pipeProps.flowType;

            return loc.GetThingList(map)
                .Select(thing => thing.TryGetComp<CompNetworkPart>())
                .All(part => part == null || !part.FlowType.Accept(pipeType));
        }
    }
}
