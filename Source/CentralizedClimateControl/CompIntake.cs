using System.Linq;
using RimWorld;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : ThingComp
    {
        // Output
        public AirFlow Intake { get; private set; }

        public bool IsOperating => !Area.IsBlocked && NetworkPart.IsConnected && Flickable.SwitchIsOn && PowerTrader.PowerOn && !Breakdownable.BrokenDown;

        private CompArea Area;
        private CompFlickable Flickable;
        private CompPowerTrader PowerTrader;
        private CompBreakdownable Breakdownable;
        private CompNetworkPart NetworkPart;

        private CompProperties_Intake IntakeProps => (CompProperties_Intake) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Area = parent.GetComp<CompArea>();
            Flickable = parent.GetComp<CompFlickable>();
            PowerTrader = parent.GetComp<CompPowerTrader>();
            Breakdownable = parent.GetComp<CompBreakdownable>();
            NetworkPart = parent.GetComp<CompNetworkPart>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (IsOperating)
            {
                Intake = CalculateIntake(parent.Map);
                PowerTrader.PowerOutput = -PowerTrader.Props.basePowerConsumption;
            }
            else
            {
                Intake = AirFlow.Zero;
                PowerTrader.PowerOutput = 0.0f;
            }
        }

        private AirFlow CalculateIntake(Map map)
        {
            var temperature = Area.FreeArea.Average(cell => cell.GetTemperature(parent.Map));

            return new AirFlow(IntakeProps.baseAirIntake * Area.MaxLoad, temperature);
        }

        public override string CompInspectStringExtra()
        {
            // @TODO: translate
            return $"Maximum intake: {IntakeProps.baseAirIntake} cc/s\nCurrent intake: {Intake}";
        }
    }
}
