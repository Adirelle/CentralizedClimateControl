using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class Graphic_LinkedPipeOverlay : Graphic_Linked
    {
        private static readonly Vector2 cellSize = new(1f, 1f);

        private readonly FlowType flowType;

        public Graphic_LinkedPipeOverlay(Graphic graphic, FlowType flowType) : base(graphic)
        {
            this.flowType = flowType;
        }

        public override void Print(SectionLayer layer, Thing thing, float extraRotation)
        {
            foreach (var loc in thing.OccupiedRect())
            {
                Printer_Plane.PrintPlane(
                    mat: LinkedDrawMatFrom(thing, loc),
                    layer: layer,
                    center: loc.ToVector3ShiftedWithAltitude(AltitudeLayer.Conduits),
                    size: cellSize,
                    rot: extraRotation
                );
            }
        }

        public override bool ShouldLinkWith(IntVec3 loc, Thing parent)
        {
            return loc.GetThingList(parent.Map).Any(thing => flowType.Accept(thing.GetFlowType()));
        }
    }
}
