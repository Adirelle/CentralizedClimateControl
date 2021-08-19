using RimWorld;
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
            if (
                Find.DesignatorManager.SelectedDesignator is Designator_Build designator
                && designator.PlacingDef is ThingDef def
                && flowType.Accept(def.GetFlowType())
            )
            {
                base.DrawLayer();
            }
        }

        protected override void TakePrintFrom(Thing thing)
        {
            var thingFlowType = thing.GetFlowType();
            if (flowType.Accept(thingFlowType))
            {
                OverlayGraphics.Get(thingFlowType)?.Print(this, thing, 0);
            }
        }
    }
}
