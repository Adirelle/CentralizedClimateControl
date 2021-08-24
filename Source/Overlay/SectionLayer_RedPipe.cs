using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_RedPipe : SectionLayer_Pipe
    {
        public SectionLayer_RedPipe(Section section) : base(FlowType.Red, section)
        {
        }
    }
}
