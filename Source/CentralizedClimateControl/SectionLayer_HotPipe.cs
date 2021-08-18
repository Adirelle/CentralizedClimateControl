using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_HotPipe : SectionLayer_Pipe
    {
        /// <summary>
        ///     Red Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_HotPipe(Section section) : base(base.FlowType.Hot, section)
        {

        }
    }
}
