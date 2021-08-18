using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_ColdPipe : SectionLayer_Pipe
    {
        /// <summary>
        ///     Blue Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_ColdPipe(Section section) : base(FlowType.Cold, section)
        {
        }
    }
}
