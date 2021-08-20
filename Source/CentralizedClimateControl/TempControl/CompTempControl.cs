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

        public float CurrentLoad { get; private set; }

        public float CurrentEfficiency { get; private set; }

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

            CurrentCapacity = Props.thermalCapacity * ClearArea.Count / Area.Count;

            if (!IsOperating)
            {
                powerTrader.PowerOutput = 0.0f;
                CurrentLoad = 0.0f;
                CurrentEfficiency = 0.0f;
                MaxInput = 0.0f;
                Output = AirFlow.Zero;
                return;
            }

            MaxInput = Props.thermalCapacity;
            var throughput = Mathf.Min(Input.Throughput, MaxInput);

            if (Mathf.Approximately(throughput, 0.0f))
            {
                powerTrader.PowerOutput = 0.0f;
                Output = AirFlow.Zero;
                return;
            }

            var tempDelta = tempControl.targetTemperature - Input.Temperature;
            var energyDelta = tempDelta * throughput;

            var maxEnergyChange = CurrentCapacity * tempNominalChange;
            var energyChange = Mathf.Clamp(energyDelta, -maxEnergyChange, maxEnergyChange);
            CurrentLoad = Mathf.Abs(energyChange / maxEnergyChange);

            var tempChange = energyChange / throughput;
            CurrentEfficiency = Mathf.Abs(tempChange / tempDelta);

            var highPowerUsage = !Mathf.Approximately(tempChange, 0.0f);

            Output = new AirFlow(throughput, Input.Temperature + tempChange);

            tempControl.operatingAtHighPower = highPowerUsage;
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption * (highPowerUsage ? 1.0f : tempControl.Props.lowPowerConsumptionFactor);

            if (tempChange < 0.0f)
            {
                GenTemperature.PushHeat(ClearArea[0], parent.Map, heatExhaustFactor * -tempChange);
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TODO: translate
            builder.AppendInNewLine("Maximum processing capacity: {0}".Translate(CurrentCapacity.ToStringThroughput()));

            if (IsOperating)
            {
                // @TODO: translate
                builder.AppendInNewLine("Current input => output: {0} => {1}".Translate(Input.Translate(), Output.Translate()));

                // @TODO: translate
                builder.AppendInNewLine("Unit load: {0}".Translate(CurrentLoad.ToStringPercent()));

                // @TODO: translate
                builder.AppendInNewLine("Unit efficiency: {0}".Translate(CurrentEfficiency.ToStringPercent()));
            }

            if (IsConnected)
            {
                // @TODO: translate
                builder.AppendInNewLine(
                    "Grid current / maximum processing: {0} / {1}".Translate(
                        Network.CurrentProcessed.ToStringThroughput(),
                        Network.MaxProcessing.ToStringThroughput()
                    )
                );
            }
        }

        public override string DebugString() =>
            string.Join("\n",
                base.DebugString(),
                $"Input={Input}",
                $"MaxInput={MaxInput}",
                $"Output={Output}",
                $"CurrentCapacity={CurrentCapacity}"
            );
    }
}
