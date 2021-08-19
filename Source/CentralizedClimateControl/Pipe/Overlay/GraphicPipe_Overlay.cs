using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class GraphicPipe_Overlay : GraphicPipe
    {
        private readonly Graphic baseGraphic;
        private readonly Graphic anyGraphic;

        public GraphicPipe_Overlay(Graphic subGraphic, Graphic anyGraphic, FlowType flowType) : base(subGraphic, flowType)
        {
            baseGraphic = subGraphic;
            this.anyGraphic = anyGraphic;
        }

        protected override Material LinkedDrawMatFrom(Thing parent, IntVec3 cell)
        {
            var manager = CentralizedClimateControlUtility.GetNetManager(parent.Map);
            var type = manager.GetFirstPartAt(cell, FlowType)?.FlowType ?? FlowType.None;
            subGraphic = (type == FlowType.Any) ? anyGraphic : baseGraphic;
            return base.LinkedDrawMatFrom(parent, cell);
        }
    }
}
