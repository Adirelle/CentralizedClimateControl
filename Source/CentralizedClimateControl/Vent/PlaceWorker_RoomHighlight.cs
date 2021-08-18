using Verse;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace CentralizedClimateControl
{
    internal class PlaceWorker_RoomHighlight : PlaceWorker_NeedsClearArea
    {
        protected override Color GetGhostColor(IntVec3 loc, Color defaultColor)
        {
            var pipe = loc.GetFirstThingWithComp<CompPipe>(Find.CurrentMap).GetComp<CompPipe>();
            if (pipe != null)
            {
                return pipe.FlowType.Color();
            }

            return base.GetGhostColor(loc, defaultColor);
        }

        protected override void DoDrawGhost(IEnumerable<IntVec3> area, Color color)
        {
            base.DoDrawGhost(area, color);

            var room = area
                .Select(cell => cell.GetRoomOrAdjacent(Find.CurrentMap))
                .FirstOrDefault(room => !room.UsesOutdoorTemperature);
            if (room != null)
            {
                GenDraw.DrawFieldEdges(room.Cells.ToList(), color);
            }
        }
    }
}
