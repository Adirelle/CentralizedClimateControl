namespace CentralizedClimateControl
{
    public class CompProperties_Pipe : CompProperties_NetworkPart
    {
        public bool hidden;

        public CompProperties_Pipe()
        {
            compClass = typeof(CompPipe);
        }
    }
}
