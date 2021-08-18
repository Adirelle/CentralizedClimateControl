using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class GraphicPipe_Overlay : GraphicPipe
    {
        private readonly Graphic baseGraphic;
        private readonly Graphic anyGraphic;

        public GraphicPipe_Overlay(Graphic subGraphic, Graphic anyGraphic, FlowType type) : base(subGraphic, type)
        {
            baseGraphic = subGraphic;
            this.anyGraphic = anyGraphic;
        }

        protected override Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
        {
            var type = TryGetFlowType(cell, parent.Map) ?? FlowType.Any;
            subGraphic = (type == FlowType.Any) ? anyGraphic : baseGraphic;
            return base.LinkedDrawMatFrom(parent, cell);
        }
    }
}
