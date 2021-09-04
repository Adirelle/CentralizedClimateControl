using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class Graphic_LinkedPipeOverlay : Graphic_Linked
    {
        private static readonly Vector2 cellSize = new(1f, 1f);

        private readonly FlowType flowType;

        private readonly Graphic graphic;

        private readonly Graphic buildingGraphic;

        public Graphic_LinkedPipeOverlay(Graphic graphic, FlowType flowType) : base(graphic)
        {
            this.graphic = graphic;
            this.flowType = flowType;

            var fadedColor = graphic.color;
            var fadedColorTwo = graphic.color;
            fadedColor.a /= 3f;
            fadedColorTwo.a /= 3f;
            buildingGraphic = graphic.GetColoredVersion(graphic.Shader, fadedColor, fadedColorTwo);
        }

        protected override Material LinkedDrawMatFrom(Thing thing, IntVec3 cell)
        {
            subGraphic = (thing is Frame || thing is Blueprint) ? buildingGraphic : graphic;
            return base.LinkedDrawMatFrom(thing, cell);
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
            return parent.Map.NetworkManager().HasPartAt(loc, flowType);
        }
    }
}
