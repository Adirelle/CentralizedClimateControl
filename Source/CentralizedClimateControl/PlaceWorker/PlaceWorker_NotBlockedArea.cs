using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    internal class PlaceWorker_NotBlockedArea : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            GenDraw.DrawFieldEdges(def.GetAreaShape().Cells(center, rot, def.Size).ToList(), ghostCol);
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            if (def.GetAreaShape().Cells(center, rot, def.Size).IsBlocked(map, thingToIgnore))
            {
                // @TRANSLATE: Air area is blocked
                return "CentralizedClimateControl.PlaceWorker.AreaBlocked".Translate();
            }

            return AcceptanceReport.WasAccepted;
        }
    }
}
