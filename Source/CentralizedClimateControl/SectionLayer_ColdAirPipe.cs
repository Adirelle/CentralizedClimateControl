using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_ColdAirPipe : SectionLayer_AirPipe
    {
        /// <summary>
        ///     Blue Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_ColdAirPipe(Section section) : base(AirFlowType.Cold, section)
        {
        }
    }
}
