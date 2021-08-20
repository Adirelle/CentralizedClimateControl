using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    internal class PlaceWorker_NeedsClearArea : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var area = GetArea(def, center, rot);
            var color = GetGhostColor(center, ghostCol);

            DoDrawGhost(area, color);
        }

        protected virtual Color GetGhostColor(IntVec3 loc, Color defaultColor)
        {
            return defaultColor;
        }

        protected virtual void DoDrawGhost(IEnumerable<IntVec3> area, Color color)
        {
            GenDraw.DrawFieldEdges(area.ToList(), color);
        }

        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            return GetArea(def, center, rot).All(cell => !cell.Impassable(map));
        }

        private IEnumerable<IntVec3> GetArea(BuildableDef def, IntVec3 center, Rot4 rot)
        {
            if (def is ThingDef thingDef)
            {
                var props = thingDef.GetCompProperties<CompProperties_Building>();
                if (props != null)
                {
                    return props.shape.Cells(center, rot, thingDef.size);
                }
            }
            return Enumerable.Empty<IntVec3>();
        }
    }
}
