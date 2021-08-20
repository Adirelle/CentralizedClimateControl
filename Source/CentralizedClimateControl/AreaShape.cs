using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public enum AreaShape
    {
        Side,
        Around
    }

    public static class AreaShapeExtensions
    {
        public static IEnumerable<IntVec3> Cells(this AreaShape shape, IntVec3 center, Rot4 rot, IntVec2 size)
        {
            return shape switch
            {
                AreaShape.Around => GenAdj.CellsAdjacentCardinal(center, rot, size),
                AreaShape.Side => GetSideArea(center, rot, size),
                _ => Enumerable.Empty<IntVec3>()
            };
        }

        public static IEnumerable<IntVec3> Cells(this AreaShape shape, Thing thing)
        {
            return shape.Cells(thing.Position, thing.Rotation, thing.def.Size);
        }

        private static IEnumerable<IntVec3> GetSideArea(IntVec3 center, Rot4 rot, IntVec2 size)
        {
            var xAxis = IntVec3.East.RotatedBy(rot);
            var loc = center + IntVec3.South.RotatedBy(rot) * size.z;
            for (var i = size.x; i > 0; i--)
            {
                yield return loc;
                loc += xAxis;
            }
        }
    }
}
