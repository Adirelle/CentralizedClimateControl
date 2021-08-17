using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_HotAirPipe : SectionLayer_AirPipe
    {
        /// <summary>
        ///     Red Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_HotAirPipe(Section section) : base(AirFlowType.Hot, section)
        {

        }
    }
}
