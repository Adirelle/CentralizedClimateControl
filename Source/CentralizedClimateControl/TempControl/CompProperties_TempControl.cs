using Verse;

namespace CentralizedClimateControl
{
    public class CompProperties_TempControl : CompProperties
    {        
        public float thermalCapacity;

        public CompProperties_TempControl() : base(typeof(CompTempControl))
        {
        }
    }
}
