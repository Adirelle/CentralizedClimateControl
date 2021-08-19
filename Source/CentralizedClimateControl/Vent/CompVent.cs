using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompVent : ThingComp
    {
        private const float secondsPerRareTick = 250.0f / 60;
        private const float cellsPerCc = 12.0f;
        private const float baseExhaust = 100.0f;

        // Input
        public AirFlow Exhaust;

        // Output
        public float MaxExhaust { get; private set; }
        public bool IsOperating => !area.IsBlocked && flickable.SwitchIsOn && networkPart.IsConnected;

        private CompArea area;
        private CompFlickable flickable;
        private CompNetworkPart networkPart;

        private CompProperties_Vent VentProps => (CompProperties_Vent) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            area = parent.GetComp<CompArea>();
            flickable = parent.GetComp<CompFlickable>();
            networkPart = parent.GetComp<CompNetworkPart>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating)
            {
                MaxExhaust = 0;
                return;
            }

            MaxExhaust = VentProps.baseAirExhaust * area.MaxLoad;

            var exhaustCell = area.FreeArea[0];
            var energyLimit = Mathf.Min(MaxExhaust, Exhaust.Throughput) * cellsPerCc * secondsPerRareTick / baseExhaust;
            var tempChange = GenTemperature.ControlTemperatureTempChange(exhaustCell, parent.Map, energyLimit, Exhaust.Temperature);

            exhaustCell.GetRoomOrAdjacent(parent.Map).Temperature += tempChange;
        }

        public override string CompInspectStringExtra()
        {
            var stringBuilder = new StringBuilder();

            // @TODO: translate
            stringBuilder.AppendInNewLine("Maximum exhaust: {0}".Translate(MaxExhaust.ToStringThroughput()));

            // @TODO: translate
            stringBuilder.AppendInNewLine("Current exhaust: {0}".Translate(Exhaust.Translate()));

            return stringBuilder.ToString();
        }
    }
}
