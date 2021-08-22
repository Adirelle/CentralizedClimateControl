using System.Linq;
using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : CompPowered
    {
        // Output
        public AirFlow Intake => IsOperating ? AirFlow.Make(MaxIntake * Network.CurrentThroughput / Network.MaxIntake, AverageTemperature) : AirFlow.Zero;

        public float MaxIntake => IsOperating ? AvailableThroughput : 0.0f;

        public float AverageTemperature { get; private set; }

        public override void CompTickRare()
        {
            base.CompTickRare();
            AverageTemperature = ClearArea.Count > 0 ? ClearArea.Average(cell => cell.GetTemperature(parent.Map)) : 0.0f;
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TRANSLATE: Maximum intake: {0}
            builder.AppendInNewLine("CentralizedClimateControl.Inspect.Intake.Maximum".Translate(ThroughputCapacity.ToStringThroughput()));

            if (IsOperating)
            {
                // @TRANSLATE: Current intake: {0}
                builder.AppendInNewLine("CentralizedClimateControl.Inspect.Intake.Current".Translate(Intake.Translate()));
            }

            if (IsConnected)
            {
                builder.AppendInNewLine(
                    // @TRANSLATE: Grid current / maximum intake: {0} / {1}
                    "CentralizedClimateControl.Inspect.Intake.Network".Translate(
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
