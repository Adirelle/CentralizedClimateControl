using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompVent : CompBuilding
    {
        private const float roomHeight = 2.4f;

        public AirFlow Exhaust => IsOperating ? (Network.CurrentExhaust * (MaxExhaust / Network.MaxExhaust)) : AirFlow.Zero;

        public float MaxExhaust => IsOperating ? AvailableThroughput : 0.0f;

        public override void NetworkPostTick()
        {
            base.NetworkPostTick();

            if (!IsOperating)
            {
                return;
            }

            var exhaustLoad = Exhaust.Throughput / MaxExhaust;
            foreach (var cell in ClearArea)
            {
                var tempDelta = cell.GetTemperature(parent.Map) - Exhaust.Temperature;
                var diminishingReturn = Mathf.Clamp01(1.0f / (0.5f + Mathf.Abs(tempDelta)));
                var energyLimit = exhaustLoad * Props.flowPerTile * diminishingReturn / roomHeight;
                var tempChange = GenTemperature.ControlTemperatureTempChange(cell, parent.Map, energyLimit, Exhaust.Temperature);
                cell.GetRoomOrAdjacent(parent.Map).Temperature += tempChange;
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            builder.AppendInNewLine("CentralizedClimateControl.Vent.Exhaust.Maximum".Translate(ThroughputCapacity.ToStringThroughput()));

            if (IsOperating)
            {
                builder.AppendInNewLine("CentralizedClimateControl.Vent.Exhaust.Current".Translate(Exhaust.Translate()));
            }

            if (IsConnected)
            {
                builder.AppendInNewLine(
                    "CentralizedClimateControl.Vent.Exhaust.Network".Translate(
                        Network.CurrentExhaust.ToStringThroughput(),
                        Network.MaxExhaust.ToStringThroughput()
                    )
                );
            }
        }

        public override string DebugString() =>
            string.Join("\n",
                base.DebugString(),
                $"Exhaust={Exhaust}",
                $"MaxExhaust={MaxExhaust}"
            );
    }
}
