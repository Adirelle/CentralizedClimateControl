using RimWorld;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    internal abstract class SectionLayer_Pipe : SectionLayer
    {
        private readonly FlowType flowType;

        public override bool Visible => shouldDraw();

        protected SectionLayer_Pipe(FlowType flowType, Section section) : base(section)
        {
            this.flowType = flowType;
            relevantChangeTypes = MapMeshFlag.Buildings | MapMeshFlag.Things;
        }

        private bool shouldDraw()
        {
            if (
                Find.DesignatorManager.SelectedDesignator is Designator_Build designator
                && designator.PlacingDef is ThingDef def
                && flowType.Accept(def.GetFlowType())
            )
            {
                return true;
            }

            if (
                Find.Selector.SelectedObjects.OfType<Thing>()
                .Any(thing => flowType.Accept(thing.GetFlowType()))
            )
            {
                return true;
            }

            return false;
        }

        public override void Regenerate()
        {
            var map = section.map;
            var manager = map.NetworkManager();
            var rect = section.CellRect;
            manager.ClearCache(rect, regenMapMesh: false);
            ClearSubMeshes(MeshParts.All);
            foreach (IntVec3 loc in rect)
            {
                foreach (var thing in map.thingGrid.ThingsListAtFast(loc))
                {
                    if (flowType.Accept(thing.GetFlowType()))
                    {
                        thing.GetFlowType().Overlay().Print(this, thing, 0);
                    }
                }
            }
            FinalizeMesh(MeshParts.All);
        }
    }
}
