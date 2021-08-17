using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_AirFlow : CompProperties
    {        
        public float baseAirExhaust;

        public float baseAirFlow;

        public AirFlowType flowType;

        public float thermalCapacity;

        public bool hiddenPipe;

#if DEBUG
        public override string ToString()
        {
            return $"CompProperties_AirFlow({baseAirExhaust}, {baseAirFlow}, {flowType}, {thermalCapacity}, {hiddenPipe})";
        }
#endif
    }
}
