using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class CompArea : ThingComp
    {
        private CompProperties_Area AreaProps => (CompProperties_Area) props;

        public List<IntVec3> Area { get; private set; }

        public List<IntVec3> FreeArea { get; private set; }

        public AreaShape Shape => AreaProps.shape;

        public bool IsBlocked => FreeArea.Count == 0;

        public float MaxLoad => (float) FreeArea.Count / Area.Count;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Area = GetArea(Shape, parent).ToList();
            CompTickRare();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            FreeArea = Area.Where(cell => !cell.Impassable(parent.Map)).ToList();
        }

        public override string CompInspectStringExtra()
        {
            if (IsBlocked)
            {
                // @TODO: translation
                return "Totally blocked";
            }

            if (MaxLoad < 1.0)
            {
                // @TODO: translate
                return $"Blocked at {(1.0f - MaxLoad) * 100.0f}%";
            }

            return null;
        }

        public static IEnumerable<IntVec3> GetArea(ThingWithComps thing)
        {
            return GetArea(thing.GetComp<CompArea>().Shape, thing);
        }

        public static IEnumerable<IntVec3> GetArea(AreaShape shape, Thing thing)
        {
            return GetArea(shape, thing.Position, thing.Rotation, thing.def.size);
        }

        public static IEnumerable<IntVec3> GetArea(AreaShape shape, IntVec3 location, Rot4 rotation, IntVec2 size)
        {
            switch (shape)
            {
                case AreaShape.Side:
                    return GetSideArea(location, rotation, size);

                case AreaShape.Around:
                    return GenAdj.CellsAdjacent8Way(location, rotation, size);

                default:
                    Log.Error("invalid area shape");
                    return Enumerable.Empty<IntVec3>();
            }
        }

        private static IEnumerable<IntVec3> GetSideArea(IntVec3 location, Rot4 rotation, IntVec2 size)
        {
            var xAxis = IntVec3.East.RotatedBy(rotation);
            var loc = location + IntVec3.South.RotatedBy(rotation) * size.z;
            for (var i = size.x; i > 0; i--)
            {
                yield return loc;
                loc += xAxis;
            }
        }
    }
}
