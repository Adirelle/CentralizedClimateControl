using System.Linq;
using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : CompPowered
    {
        // Output
        public AirFlow Intake => IsOperating ? AirFlow.Make(MaxIntake * Network.CurrentThroughput / Network.MaxIntake, AverageTemperature) : AirFlow.Zero;

        public float AvailableIntake => Props.baseAirIntake * ClearArea.Count / Area.Count;

        public float MaxIntake => IsOperating ? AvailableIntake : 0.0f;

        public float AverageTemperature { get; private set; }

        protected new CompProperties_Intake Props => (CompProperties_Intake) props;

        public override void CompTickRare()
        {
            base.CompTickRare();
            AverageTemperature = ClearArea.Count > 0 ? ClearArea.Average(cell => cell.GetTemperature(parent.Map)) : 0.0f;
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
