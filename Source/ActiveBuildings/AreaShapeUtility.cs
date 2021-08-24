using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    internal static class AreaShapeUtility
    {
        public static AreaShape? GetAreaShape(this ThingComp comp)
        {
            return comp.parent.GetAreaShape();
        }

        public static AreaShape? GetAreaShape(this Thing thing)
        {
            return thing.def.GetAreaShape();
        }

        public static AreaShape? GetAreaShape(this Def def)
        {
            return (def as ThingDef)?.GetCompProperties<CompProperties_Building>()?.shape;
        }

        public static IEnumerable<IntVec3> Cells(this AreaShape areaShape, IntVec3 center, Rot4 rot, IntVec2 size)
        {
            return areaShape switch
            {
                AreaShape.AroundAndInside => GenAdj.CellsAdjacent8Way(center, rot, size).Concat(GenAdj.CellsOccupiedBy(center, rot, size)),
                AreaShape.AdjacentCardinal => GenAdj.CellsAdjacentCardinal(center, rot, size),
                AreaShape.AdjacentSouth => CellsAdjacentEdge(center, rot, size, Rot4.South),
                AreaShape.AdjacentNorth => CellsAdjacentEdge(center, rot, size, Rot4.North),
                AreaShape.AdjacentCardinalAndInside => GenAdj.CellsAdjacentCardinal(center, rot, size).Concat(GenAdj.CellsOccupiedBy(center, rot, size)),
                _ => Enumerable.Empty<IntVec3>()
            };
        }

        public static IEnumerable<IntVec3> IsClear(this IEnumerable<IntVec3> area, Map map, Thing thingToIgnore = null)
        {
            foreach (var cell in area)
            {
                var edifice = cell.GetEdifice(map);
                if (edifice is null || edifice == thingToIgnore)
                {
                    yield return cell;
                }
            }
        }

        public static bool IsBlocked(this IEnumerable<IntVec3> area, Map map, Thing thingToIgnore = null)
        {
            return !area.IsClear(map, thingToIgnore).Any();
        }

        public static IEnumerable<IntVec3> Cells(this AreaShape shape, Thing thing)
        {
            return shape.Cells(thing.Position, thing.Rotation, thing.def.Size);
        }

        private static IEnumerable<IntVec3> CellsAdjacentEdge(IntVec3 center, Rot4 rot, IntVec2 size, Rot4 side)
        {
            var rect = GenAdj.OccupiedRect(center, rot, size);
            rot = new Rot4(rot.AsByte + side.AsByte);
            if (rot.IsHorizontal)
            {
                var x = (rot == Rot4.East) ? (rect.maxX + 1) : (rect.minX - 1);
                for (var z = rect.minZ; z <= rect.maxZ; z++)
                {
                    yield return new IntVec3(x, 0, z);
                }
            }
            else
            {
                var z = (rot == Rot4.North) ? (rect.maxZ + 1) : (rect.minZ - 1);
                for (var x = rect.minX; x <= rect.maxX; x++)
                {
                    yield return new IntVec3(x, 0, z);
                }
            }
        }
    }
}
