using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompTempControl : ThingComp
    {
        private const float TempNominalChange = 10.0f;
        private const float HeatExhaustFactor = 1.25f;

        // Input
        public AirFlow Input;

        // Output
        public AirFlow Output { get; private set; }

        public float MaxInput { get; private set; }

        public float CurrentCapacity { get; private set; }

        public bool IsOperating => !area.IsBlocked && flickable.SwitchIsOn && networkPart.IsConnected && !breakdownable.BrokenDown && powerTrader.PowerOn;

        private CompArea area;
        private CompFlickable flickable;
        private CompNetworkPart networkPart;
        private CompPowerTrader powerTrader;
        private CompBreakdownable breakdownable;
        private RimWorld.CompTempControl tempControl;

        private CompProperties_TempControl TempProps => (CompProperties_TempControl) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            area = parent.GetComp<CompArea>();
            flickable = parent.GetComp<CompFlickable>();
            networkPart = parent.GetComp<CompNetworkPart>();
            powerTrader = parent.GetComp<CompPowerTrader>();
            breakdownable = parent.GetComp<CompBreakdownable>();
            tempControl = parent.GetComp<RimWorld.CompTempControl>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating || Mathf.Approximately(Input.Throughput, 0.0f))
            {
                powerTrader.PowerOutput = 0.0f;
                MaxInput = 0.0f;
                Output = AirFlow.Zero;
                return;
            }

            MaxInput = TempProps.thermalCapacity;
            CurrentCapacity = TempProps.thermalCapacity * area.MaxLoad;

            var throughput = Mathf.Min(Input.Throughput, MaxInput);
            var tempDelta = tempControl.targetTemperature - Input.Temperature;
            var energyDelta = tempDelta * throughput;
            var maxEnergyChange = CurrentCapacity * TempNominalChange;
            var energyChange = Mathf.Clamp(energyDelta, -maxEnergyChange, maxEnergyChange);
            var tempChange = energyChange / throughput;
            var highPowerUsage = !Mathf.Approximately(tempChange, 0.0f);

            Output = new AirFlow(throughput, Input.Temperature + tempChange);

            tempControl.operatingAtHighPower = highPowerUsage;
            powerTrader.PowerOutput = -powerTrader.Props.basePowerConsumption * (highPowerUsage ? 1.0f : tempControl.Props.lowPowerConsumptionFactor);

            if (tempChange < 0.0f)
            {
                GenTemperature.PushHeat(area.FreeArea[0], parent.Map, HeatExhaustFactor * -tempChange);
            }
        }

        public override string CompInspectStringExtra()
        {
            var builder = new StringBuilder();

            // @TODO: translate
            builder.AppendInNewLine("Thermal capacity: {0}".Translate(CurrentCapacity.ToStringThroughput()));

            // @TODO: translate
            builder.AppendInNewLine("Processing: {0} => {1}".Translate(Input.Translate(), Output.Translate()));

            return builder.ToString();
        }
    }
}
