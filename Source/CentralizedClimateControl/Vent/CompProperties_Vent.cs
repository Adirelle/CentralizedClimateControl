using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Vent : CompProperties
    {
        public float baseAirExhaust;

        public CompProperties_Vent()
        {
            compClass = typeof(CompVent);
        }
    }
}
