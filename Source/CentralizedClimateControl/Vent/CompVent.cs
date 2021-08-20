using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompVent : CompBuilding
    {
        private const float secondsPerRareTick = 250.0f / 60;
        private const float cellsPerCc = 12.0f;
        private const float baseExhaust = 100.0f;

        // Input
        public AirFlow Exhaust;

        // Output
        public float AvailableExhaust { get; private set; }

        public float MaxExhaust { get; private set; }

        protected new CompProperties_Vent Props => (CompProperties_Vent) props;

        public override void CompTickRare()
        {
            base.CompTickRare();

            AvailableExhaust = Props.baseAirExhaust * ClearArea.Count / Area.Count;

            if (!IsOperating)
            {
                MaxExhaust = 0;
                return;
            }

            MaxExhaust = AvailableExhaust;

            var exhaustCell = ClearArea[0];
            var energyLimit = Mathf.Min(MaxExhaust, Exhaust.Throughput) * cellsPerCc * secondsPerRareTick / baseExhaust;
            var tempChange = GenTemperature.ControlTemperatureTempChange(exhaustCell, parent.Map, energyLimit, Exhaust.Temperature);

            exhaustCell.GetRoomOrAdjacent(parent.Map).Temperature += tempChange;
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
