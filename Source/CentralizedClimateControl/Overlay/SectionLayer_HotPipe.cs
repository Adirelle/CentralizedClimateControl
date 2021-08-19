using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_HotPipe : SectionLayer_Pipe
    {
        public SectionLayer_HotPipe(Section section) : base(FlowType.Hot, section)
        {
        }
    }
}
