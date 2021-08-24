namespace CentralizedClimateControl
{
    public class CompPipe : CompBase
    {
        public override FlowType FlowType => Props.flowType;

        protected CompProperties_Pipe Props => (CompProperties_Pipe) props;
    }
}
