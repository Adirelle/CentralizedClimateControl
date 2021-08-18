using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class CompVent : ThingComp
    {
        // Magic numbers borrowed from Building_Heater (and 100 is considered the base vent size)
        private const float FlowEnergy = 12.0f * 4.16666651f / 100.0f;

        // Input
        public AirFlow Exhaust;

        // Output
        public float MaxExhaust { get; private set; }
        public bool IsOperating => !Area.IsBlocked && Flickable.SwitchIsOn && NetworkPart.IsConnected;

        private CompArea Area;
        private CompFlickable Flickable;
        private CompNetworkPart NetworkPart;

        private CompProperties_Vent VentProps => (CompProperties_Vent) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            Area = parent.GetComp<CompArea>();
            Flickable = parent.GetComp<CompFlickable>();
            NetworkPart = parent.GetComp<CompNetworkPart>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (!IsOperating)
            {
                MaxExhaust = 0;
                return;
            }

            MaxExhaust = VentProps.baseAirExhaust * Area.MaxLoad;

            var exhaustCell = Area.FreeArea[0];
            var energyLimit = Mathf.Min(MaxExhaust, Exhaust.Throughput) * FlowEnergy;
            var tempChange = GenTemperature.ControlTemperatureTempChange(exhaustCell, parent.Map, energyLimit, Exhaust.Temperature);

            exhaustCell.GetRoomOrAdjacent(parent.Map).Temperature += tempChange;
        }
    }
}
