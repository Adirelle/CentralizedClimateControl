using Verse;

namespace CentralizedClimateControl
{
    internal class SectionLayer_FrozenAirPipe : SectionLayer_AirPipe
    {
        /// <summary>
        ///     Cyan Pipe Overlay Section Layer
        /// </summary>
        /// <param name="section">Section of the Map</param>
        public SectionLayer_FrozenAirPipe(Section section) : base(AirFlowType.Frozen, section)
        {
        }
    }
}
