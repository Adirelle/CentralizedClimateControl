using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class PlaceWorker_NotOnPipe : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            return GenAdj.CellsOccupiedBy(loc, rot, def.Size)
                .All(cell => cell.GetFirstThingWithComp<CompPipe>(map) == null);
        }
    }
}
