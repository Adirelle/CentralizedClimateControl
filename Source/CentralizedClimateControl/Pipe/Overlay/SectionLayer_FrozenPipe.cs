using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_FrozenPipe : SectionLayer_Pipe
    {
        /// <summary>
        ///     Cyan Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_FrozenPipe(Section section) : base(FlowType.Frozen, section)
        {
        }
    }
}
