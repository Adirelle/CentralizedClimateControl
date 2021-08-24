using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompTempControl : CompPowered
    {
        private const float heatExhaustFactor = 1.25f;

        public float MaxInput => IsOperating ? AvailableThroughput : 0.0f;

        protected AirFlow Input => IsOperating ? (Network.CurrentIntake * (MaxInput / Network.MaxProcessing)).Clamp(MaxInput) : AirFlow.Zero;

        public AirFlow Output { get; private set; }

        protected RimWorld.CompTempControl tempControl;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            tempControl = parent.GetComp<RimWorld.CompTempControl>() ?? throw new System.NullReferenceException("could not find a CompTempControl");
        }

        public override void NetworkPostTick()
        {
            base.NetworkPostTick();

            var tempDelta = tempControl.targetTemperature - Input.Temperature;

            tempControl.operatingAtHighPower = Mathf.Abs(tempDelta) > 0.5f;
            if (!tempControl.operatingAtHighPower)
            {
                powerTrader.PowerOutput *= tempControl.Props.lowPowerConsumptionFactor;
            }

            var energyDelta = Mathf.Abs(tempDelta) * Input.Throughput;
            var energyCapacity = powerTrader.Props.basePowerConsumption * powerTrader.Props.basePowerConsumption;
            var tempChange = tempDelta * Mathf.Clamp01(1.0f - energyDelta / energyCapacity);

            Output = AirFlow.Make(Input.Throughput, Input.Temperature + tempChange);

            if (tempChange < 0.0f)
            {
                var heatExhaust = heatExhaustFactor * tempChange / ClearArea.Count;
                foreach (var cell in ClearArea)
                {
                    GenTemperature.PushHeat(cell, parent.Map, heatExhaust);
                }
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            builder.AppendInNewLine("CentralizedClimateControl.TempControl.Throughput.Maximum".Translate(ThroughputCapacity.ToStringThroughput()));

            if (IsOperating)
            {
                builder.AppendInNewLine("CentralizedClimateControl.TempControl.Processing.Current".Translate(Input.Translate(), Output.Translate()));
            }

            if (IsConnected)
            {
                builder.AppendInNewLine(
                    "CentralizedClimateControl.TempControl.Processing.Network".Translate(
                        Network.CurrentProcessed.ToStringThroughput(),
                        Network.MaxProcessing.ToStringThroughput()
                    )
                );
            }
        }

        public override string DebugString() =>
            string.Join("\n",
                base.DebugString(),
                $"MaxInput={MaxInput}",
                $"Input={Input}",
                $"Output={Output}"
            );
    }
}