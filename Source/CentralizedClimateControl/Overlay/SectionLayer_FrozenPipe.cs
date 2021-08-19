using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_FrozenPipe : SectionLayer_Pipe
    {
        public SectionLayer_FrozenPipe(Section section) : base(FlowType.Frozen, section)
        {
        }
    }
}
