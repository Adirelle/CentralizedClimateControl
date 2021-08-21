using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_ColdPipe : SectionLayer_Pipe
    {
        public SectionLayer_ColdPipe(Section section) : base(FlowType.Cold, section)
        {
        }
    }
}
