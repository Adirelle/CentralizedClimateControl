using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_Area : CompProperties
    {
        public AreaShape shape;

        public CompProperties_Area()
        {
            compClass = typeof(CompArea);
        }
    }
}