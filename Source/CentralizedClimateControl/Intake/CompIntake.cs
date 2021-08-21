using System.Linq;
using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : CompPowered
    {
        public float NetworkLoad;

        // Output
        public AirFlow Intake { get; private set; }

        public float AvailableIntake { get; private set; }

        public float MaxIntake { get; private set; }

        protected new CompProperties_Intake Props => (CompProperties_Intake) props;

        public override void CompTickRare()
        {
            base.CompTickRare();

            AvailableIntake = Props.baseAirIntake * ClearArea.Count / Area.Count;

            if (!IsOperating)
            {
                MaxIntake = 0.0f;
                Intake = AirFlow.Zero;
                powerTrader.PowerOutput = 0.0f;
                return;
            }

            MaxIntake = AvailableIntake;
            var temperature = ClearArea.Average(cell => cell.GetTemperature(parent.Map));
            Intake = new AirFlow(MaxIntake * NetworkLoad, temperature);
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption;
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TODO: translate
            builder.AppendInNewLine("Maximum intake: {0}".Translate(AvailableIntake.ToStringThroughput()));

            if (IsOperating)
            {
                // @TODO: translate
                builder.AppendInNewLine("Current intake: {0}".Translate(Intake.Translate()));
            }

            if (IsConnected)
            {
                // @TODO: translate
                builder.AppendInNewLine(
                    "Grid current / maximum intake: {0} / {1}".Translate(
                        Network.CurrentIntake.ToStringThroughput(),
                        Network.MaxIntake.ToStringThroughput()
                    )
                );
            }
        }

        public override string DebugString() =>
              string.Join("\n",
                  base.DebugString(),
                  $"Intake={Intake}",
                  $"MaxIntake={MaxIntake}"
              );
    }
}
