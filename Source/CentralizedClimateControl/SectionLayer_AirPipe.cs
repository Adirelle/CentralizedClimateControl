using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    internal abstract class SectionLayer_AirPipe : SectionLayer_Things
    {
        public readonly AirFlowType FlowType;

        /// <summary>
        ///     Blue Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        protected SectionLayer_AirPipe(AirFlowType flowType, Section section) : base(section)
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
            var designatorBuild = Find.DesignatorManager.SelectedDesignator as Designator_Build;

            var thingDef = designatorBuild?.PlacingDef as ThingDef;

            if (thingDef?.comps.OfType<CompProperties_AirFlow>().FirstOrDefault(x => x.flowType == FlowType) != null)
            {
                base.DrawLayer();
            }
        }

        /// <summary>
        ///     Called when a Draw is initiated from DrawLayer.
        /// </summary>
        /// <param name="thing">Thing that triggered the Draw Call</param>
        protected override void TakePrintFrom(Thing thing)
        {
            (thing as ThingWithComps)
                ?.GetComps<CompAirFlow>()
                .FirstOrDefault(x => FlowType == x.FlowType)
                ?.PrintForGrid(this, FlowType);
        }
    }
}
