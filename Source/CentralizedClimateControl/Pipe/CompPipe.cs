using Verse;

namespace CentralizedClimateControl
{
    public class CompPipe : ThingComp
    {
        public FlowType FlowType => PipeProps.flowType;
        public bool Hidden => PipeProps.hidden;

        private CompProperties_Pipe PipeProps => (CompProperties_Pipe) props;
    }
}
