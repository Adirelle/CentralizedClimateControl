using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class Graphic_LinkedPipeOverlay : Graphic_Linked
    {
        private readonly FlowType flowType;

        public Graphic_LinkedPipeOverlay(Graphic graphic, FlowType flowType) : base(graphic)
        {
            this.flowType = flowType;
        }

        public override bool ShouldLinkWith(IntVec3 loc, Thing parent)
        {
            return parent.Map.NetworkManager().HasPartAt(loc, flowType);
        }
    }
}
