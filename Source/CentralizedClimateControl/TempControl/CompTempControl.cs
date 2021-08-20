using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompTempControl : CompPowered
    {
        private const float tempNominalChange = 10.0f;
        private const float heatExhaustFactor = 1.25f;

        // Input
        public AirFlow Input;

        // Output
        public AirFlow Output { get; private set; }

        public float MaxInput { get; private set; }

        public float CurrentCapacity { get; private set; }

        protected RimWorld.CompTempControl tempControl;

        protected new CompProperties_TempControl Props => (CompProperties_TempControl) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            tempControl = parent.GetComp<RimWorld.CompTempControl>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating)
            {
                powerTrader.PowerOutput = 0.0f;
                MaxInput = 0.0f;
                Output = AirFlow.Zero;
                return;
            }

            MaxInput = Props.thermalCapacity;
            CurrentCapacity = Props.thermalCapacity * FreeArea.Count / Area.Count;

            if (Mathf.Approximately(Input.Throughput, 0.0f))
            {
                powerTrader.PowerOutput = 0.0f;
                Output = AirFlow.Zero;
                return;
            }

            var throughput = Mathf.Min(Input.Throughput, MaxInput);
            var tempDelta = tempControl.targetTemperature - Input.Temperature;
            var energyDelta = tempDelta * throughput;
            var maxEnergyChange = CurrentCapacity * tempNominalChange;
            var energyChange = Mathf.Clamp(energyDelta, -maxEnergyChange, maxEnergyChange);
            var tempChange = energyChange / throughput;
            var highPowerUsage = !Mathf.Approximately(tempChange, 0.0f);

            Output = new AirFlow(throughput, Input.Temperature + tempChange);

            tempControl.operatingAtHighPower = highPowerUsage;
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption * (highPowerUsage ? 1.0f : tempControl.Props.lowPowerConsumptionFactor);

            if (tempChange < 0.0f)
            {
                GenTemperature.PushHeat(FreeArea[0], parent.Map, heatExhaustFactor * -tempChange);
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TODO: translate
            builder.AppendInNewLine("Thermal capacity: {0}".Translate(CurrentCapacity.ToStringThroughput()));

            // @TODO: translate
            builder.AppendInNewLine("Processing: {0} => {1}".Translate(Input.Translate(), Output.Translate()));
        }
    }
}
