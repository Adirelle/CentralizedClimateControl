using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class PlaceWorker_Pipe : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            /*
            if (def is ThingDef thingDef)
            {
                var flowType = thingDef.GetFlowType();
                return !flowType.Accept(map.linkGrid.LinkFlagsAt(loc).ToFlowType());
            }*/

            return AcceptanceReport.WasAccepted;
        }
    }
}
