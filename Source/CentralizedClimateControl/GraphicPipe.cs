using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class GraphicPipe : Graphic_Linked
    {
        public readonly FlowType FlowType;

        public GraphicPipe(Graphic graphic, FlowType flowType): base(graphic)
        {
            FlowType = flowType;
        }

        protected FlowType? TryGetFlowType(IntVec3 vec, Map map)
        {
            return vec.GetFirstThingWithComp<CompNetworkPart>(map)?.GetComp<CompNetworkPart>().FlowType;
        }

        public override bool ShouldLinkWith(IntVec3 vec, Thing parent)
        {
            return vec.InBounds(parent.Map) && (TryGetFlowType(vec, parent.Map)?.Accept(FlowType) ?? false);
        }
    }
}
