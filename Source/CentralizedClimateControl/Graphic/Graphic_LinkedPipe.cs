using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class Graphic_LinkedPipe : Graphic_Linked
    {
        public Graphic_LinkedPipe(Graphic graphic) : base(graphic)
        {
        }

        public override bool ShouldLinkWith(IntVec3 loc, Thing parent)
        {
            return parent.Map.NetworkManager().HasPartAt(loc, parent.GetFlowType());
        }
    }
}
