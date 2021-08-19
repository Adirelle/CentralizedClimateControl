using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Intake : CompProperties
    {
        public float baseAirIntake;

        public CompProperties_Intake()
        {
            compClass = typeof(CompIntake);
        }
    }
}
