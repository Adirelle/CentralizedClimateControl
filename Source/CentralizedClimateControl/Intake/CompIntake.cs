using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : ThingComp
    {
        public float NetworkLoad;

        // Output
        public AirFlow Intake { get; private set; }

        public float MaxIntake { get; private set; }

        public bool IsOperating => !area.IsBlocked && networkPart.IsConnected && flickable.SwitchIsOn && powerTrader.PowerOn && !breakdownable.BrokenDown;

        private CompArea area;
        private CompFlickable flickable;
        private CompPowerTrader powerTrader;
        private CompBreakdownable breakdownable;
        private CompNetworkPart networkPart;

        private CompProperties_Intake IntakeProps => (CompProperties_Intake) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            area = parent.GetComp<CompArea>();
            flickable = parent.GetComp<CompFlickable>();
            powerTrader = parent.GetComp<CompPowerTrader>();
            breakdownable = parent.GetComp<CompBreakdownable>();
            networkPart = parent.GetComp<CompNetworkPart>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating)
            {
                MaxIntake = 0.0f;
                Intake = AirFlow.Zero;
                powerTrader.PowerOutput = 0.0f;
                return;
            }

            MaxIntake = IntakeProps.baseAirIntake * area.MaxLoad;

            var temperature = area.FreeArea.Average(cell => cell.GetTemperature(parent.Map));
            Intake = new AirFlow(MaxIntake * NetworkLoad, temperature);
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption;   
        }

        public override string CompInspectStringExtra()
        {
            var builder = new StringBuilder();

            // @TODO: translate
            builder.AppendInNewLine("Maximum intake: {0}".Translate(MaxIntake.ToStringThroughput()));

            // @TODO: translate
            builder.AppendInNewLine("Current intake: {0}".Translate(Intake.Translate()));

            return builder.ToString();
        }
    }
}
