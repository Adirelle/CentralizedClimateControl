using RimWorld;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    internal abstract class SectionLayer_Pipe : SectionLayer_Things
    {
        private readonly FlowType flowType;

        protected SectionLayer_Pipe(FlowType flowType, Section section) : base(section)
        {
            this.flowType = flowType;
            requireAddToMapMesh = false;
            relevantChangeTypes = MapMeshFlag.Buildings;
        }

        public override void DrawLayer()
        {
            if (shouldDraw())
            {
                base.DrawLayer();
            }
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

        protected override void TakePrintFrom(Thing thing)
        {
            var thingFlowType = thing.GetFlowType();
            if (flowType.Accept(thingFlowType))
            {
                thingFlowType.Overlay().Print(this, thing, 0);
            }
        }
    }
}
