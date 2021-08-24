using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    internal class PlaceWorker_HighlightsRooms : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var maybeShape = def.GetAreaShape();
            if (maybeShape is not AreaShape shape)
            {
                return;
            }
            var color = thing?.GetFlowType().Color() ?? def.GetFlowType().Color();
            var map = Find.CurrentMap;
            foreach (var cell in shape.Cells(center, rot, def.Size).IsClear(map, thing))
            {
                var room = cell.GetRoom(map);
                if (room is not null && !room.UsesOutdoorTemperature)
                {
                    GenDraw.DrawFieldEdges(room.Cells.ToList(), color);
                }
            }
        }
    }
}
