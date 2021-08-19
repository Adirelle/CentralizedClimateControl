using Verse;

namespace CentralizedClimateControl
{
    public class GraphicPipe : Graphic_Linked
    {
        public readonly FlowType FlowType;

        public GraphicPipe(Graphic graphic, FlowType flowType) : base(graphic)
        {
            FlowType = flowType;
        }

        public override bool ShouldLinkWith(IntVec3 vec, Thing parent)
        {
            var manager = CentralizedClimateControlUtility.GetNetManager(parent.Map);
            return vec.InBounds(parent.Map) && manager.GetFirstPartAt(vec, FlowType) != null;
        }
    }
}
