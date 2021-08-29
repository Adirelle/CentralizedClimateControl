using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompIntake : CompPowered
    {
        public AirFlow Intake => IsOperating ? AirFlow.Make(MaxIntake * Mathf.Min(CurrentRate, NetworkIntakeLoad), AverageTemperature) : AirFlow.Zero;

        public float MaxIntake => IsOperating ? AvailableThroughput : 0.0f;

        protected float NetworkIntakeLoad => IsConnected ? Network.CurrentThroughput / Network.MaxIntake : 0.0f;

        public float AverageTemperature { get; private set; }

        public override void NetworkPreTick()
        {
            base.NetworkPreTick();
            AverageTemperature = !IsBlocked ? ClearArea.Average(cell => cell.GetTemperature(parent.Map)) : 0.0f;
            NeededRate = NetworkIntakeLoad;
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            builder.AppendInNewLine("CentralizedClimateControl.Inspect.Intake.Maximum".Translate(ThroughputCapacity.ToStringThroughput()));

            if (IsOperating)
            {
                builder.AppendInNewLine("CentralizedClimateControl.Inspect.Intake.Current".Translate(Intake.Translate()));
            }

            if (IsConnected)
            {
                builder.AppendInNewLine(
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
