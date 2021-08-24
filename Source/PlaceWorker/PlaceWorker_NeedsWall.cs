using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class PlaceWorker_NeedsWall : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            return center.GetFirstThing(map, ThingDefOf.Wall) is not null
                ? AcceptanceReport.WasAccepted
                : new AcceptanceReport("CentralizedClimateControl.PlaceWorker.NeedsWall".Translate());
        }
    }
}
