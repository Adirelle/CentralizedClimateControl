using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompTempControl : CompPowered
    {
        private const float heatExhaustFactor = 1.25f;

        public float MaxInput => IsOperating ? Props.baseAirThroughput : 0.0f;

        protected AirFlow Input => IsOperating ? (Network.CurrentIntake * (MaxInput / Network.MaxProcessing)).Clamp(MaxInput) : AirFlow.Zero;

        protected float EnergyDelta => AirFlow.Make(Input.Throughput, tempControl.targetTemperature).Energy - Input.Energy;

        protected float CurrentCapacity { get; private set; }

        protected bool IsActive => Mathf.Abs(tempControl.targetTemperature - Input.Temperature) > 0.5f;

        protected float EffectiveEnergyChange => IsActive ? Mathf.Clamp(EnergyDelta, -CurrentCapacity, CurrentCapacity) : 0.0f;

        public AirFlow Output => AirFlow.FromEnergy(Input.Energy + EffectiveEnergyChange, Input.Throughput);

        protected float EffectiveTempChange => Output.Temperature - Input.Temperature;

        protected float CurrentLoad => Mathf.Abs(EffectiveEnergyChange) / CurrentCapacity;

        protected RimWorld.CompTempControl tempControl;

        protected new CompProperties_TempControl Props => (CompProperties_TempControl) props;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            CurrentCapacity = AirFlow.Make(Props.baseAirThroughput, Props.maxTempChange).Energy;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            tempControl = parent.GetComp<RimWorld.CompTempControl>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            tempControl.operatingAtHighPower = IsActive;
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption * (IsActive ? 1.0f : tempControl.Props.lowPowerConsumptionFactor);

            if (EffectiveTempChange < 0.0f)
            {
                var heatExhaust = heatExhaustFactor * -EffectiveTempChange / ClearArea.Count * TickerType.Rare.TickDuration();
                foreach (var cell in ClearArea)
                {
                    GenTemperature.PushHeat(cell, parent.Map, heatExhaust);
                }
            }
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TRANSLATE: Maximum throughput: {0}
            builder.AppendInNewLine("CentralizedClimateControl.TempControl.Throughput.Maximum".Translate(Props.baseAirThroughput.ToStringThroughput()));

            // @TRANSLATE: Thermal capacity: �{0} for {1}
            var capacity = AirFlow.FromEnergy(CurrentCapacity, MaxInput);
            builder.AppendInNewLine("CentralizedClimateControl.TempControl.Capacity".Translate(capacity.ToStringTemperature(), capacity.ToStringThroughput()));

            if (IsOperating)
            {
                // @TRANSLATE: Current input => output: {0} => {1}
                builder.AppendInNewLine("CentralizedClimateControl.TempControl.Processing.Current".Translate(Input.Translate(), Output.Translate()));

                if (IsActive)
                {
                    // @TRANSLATE: Current load: {0}
                    builder.AppendInNewLine("CentralizedClimateControl.TempControl.Load".Translate(CurrentLoad.ToStringPercent()));
                }
            }

            if (IsConnected)
            {
                builder.AppendInNewLine(
                    // @TRANSLATE: Grid current / maximum processing: {0} / {1}
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
                $"EnergyDelta={EnergyDelta}",
                $"CurrentCapacity={CurrentCapacity}",
                $"EffectiveEnergyChange={EffectiveEnergyChange}",
                $"EffectiveTempChange={EffectiveTempChange}",
                $"Output={Output}",
                $"CurrentLoad={CurrentLoad}",
                $"IsActive={IsActive}"
            );
    }
}
