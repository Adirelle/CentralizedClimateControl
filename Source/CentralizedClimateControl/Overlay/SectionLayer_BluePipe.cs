using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_BluePipe : SectionLayer_Pipe
    {
        public SectionLayer_BluePipe(Section section) : base(FlowType.Blue, section)
        {
        }
    }
}
