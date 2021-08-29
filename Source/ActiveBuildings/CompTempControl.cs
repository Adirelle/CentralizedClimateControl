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
            var tempDelta = tempControl.targetTemperature - Input.Temperature;
            var tempFactor = Mathf.Max(1.0f, Mathf.Pow(Mathf.Abs(tempDelta), 0.3f));
            var weightedThroughput = tempFactor * Input.Throughput;
            var energyDelta = Mathf.Abs(tempDelta) * weightedThroughput;
            var energyCapacity = Mathf.Pow(powerTrader.Props.basePowerConsumption, 2f);
            neededRate = Mathf.Clamp01(energyDelta / energyCapacity);

            base.NetworkPostTick();

            tempControl.operatingAtHighPower = currentRate > 0.5f;
            var energyChange = Mathf.Sign(tempDelta) * energyCapacity * currentRate;
            var tempChange = Input.Throughput > 0.0f ? energyChange / weightedThroughput : 0.0f;

            Output = AirFlow.Make(Input.Throughput, Input.Temperature + tempChange);

            if (IsOperating && tempChange < 0.0f)
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
