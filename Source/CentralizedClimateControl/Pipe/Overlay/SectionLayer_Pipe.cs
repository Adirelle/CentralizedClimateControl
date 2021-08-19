using RimWorld;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    internal abstract class SectionLayer_Pipe : SectionLayer_Things
    {
        public readonly FlowType FlowType;

        /// <summary>
        ///     Blue Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        protected SectionLayer_Pipe(FlowType flowType, Section section) : base(section)
        {
            FlowType = flowType;
            requireAddToMapMesh = false;
            relevantChangeTypes = (MapMeshFlag) 4;
        }

        /// <summary>
        ///     Function which Checks if we need to Draw the Layer or not. If we do, we call the Base DrawLayer();
        /// </summary>
        public override void DrawLayer()
        {
            if (
                Find.DesignatorManager.SelectedDesignator is Designator_Build designator
                && designator.PlacingDef is ThingDef def
            )
            {
                foreach (var part in def.comps.OfType<CompProperties_NetworkPart>())
                {
                    if (FlowType.Accept(part.flowType))
                    {
                        base.DrawLayer();
                        return;
                    }
                }
            }
        }

        protected override void TakePrintFrom(Thing thing)
        {
            if (thing is ThingWithComps thingWithComps)
            {
                var visibleParts = thingWithComps.GetComps<CompNetworkPart>().Where(part => part.IsVisibleOnOverlay(FlowType));
                foreach (var part in visibleParts)
                {
                    part.PrintOnOverlayLayer(this);
                }
            }
        }
    }
}
