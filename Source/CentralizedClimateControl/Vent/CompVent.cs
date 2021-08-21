using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompVent : CompBuilding
    {
        private const float cellsPerCc = 12.0f;
        private const float baseExhaust = 100.0f;

        public AirFlow Exhaust => IsOperating ? Network.CurrentExhaust * (MaxExhaust / Network.MaxExhaust) : AirFlow.Zero;

        public float AvailableExhaust => Props.baseAirExhaust * ClearArea.Count / Area.Count;

        public float MaxExhaust => IsOperating ? AvailableExhaust : 0.0f;

        protected new CompProperties_Vent Props => (CompProperties_Vent) props;

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating)
            {
                return;
            }

            var energyLimit = Mathf.Min(MaxExhaust, Exhaust.Throughput) * cellsPerCc * TickerType.Rare.TickDuration() / baseExhaust;
            energyLimit /= ClearArea.Count;
            foreach (var cell in ClearArea)
            {
                var tempChange = GenTemperature.ControlTemperatureTempChange(cell, parent.Map, energyLimit, Exhaust.Temperature);
                cell.GetRoomOrAdjacent(parent.Map).Temperature += tempChange;
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TODO: translate
            builder.AppendInNewLine("Maximum exhaust: {0}".Translate(AvailableExhaust.ToStringThroughput()));

            if (IsOperating)
            {
                // @TODO: translate
                builder.AppendInNewLine("Current exhaust: {0}".Translate(Exhaust.Translate()));
            }

            if (IsConnected)
            {
                // @TODO: translate
                builder.AppendInNewLine(
                    "Grid current / maximum exhaust: {0} / {1}".Translate(
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
