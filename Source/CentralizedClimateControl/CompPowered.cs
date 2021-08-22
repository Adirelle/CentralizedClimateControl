﻿using RimWorld;
using UnityEngine;

namespace CentralizedClimateControl
{
    public abstract class CompPowered : CompBuilding
    {
        public override bool IsOperating => base.IsOperating && powerTrader.PowerOn && !breakdownable.BrokenDown;

        protected CompPowerTrader powerTrader;
        protected CompBreakdownable breakdownable;

        protected virtual float PowerCost => powerTrader.Props.basePowerConsumption * Area.Count;

        protected new CompProperties_Powered Props => (CompProperties_Powered) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerTrader = parent.GetComp<CompPowerTrader>() ?? throw new System.NullReferenceException("could not find a CompPowerTrader");
            breakdownable = parent.GetComp<CompBreakdownable>() ?? throw new System.NullReferenceException("could not find a CompBreakdownable");
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            powerTrader.PowerOutput = -5.0f * Mathf.Ceil(PowerCost / Props.energyEfficiency / 5.0f);
        }

        public override string DebugString() =>
                string.Join("\n",
                    base.DebugString(),
                    $"BrokenDown={breakdownable.BrokenDown}",
                    $"PowerNet={powerTrader.PowerNet is not null}",
                    $"PowerOn={powerTrader.PowerOn}",
                    $"PowerOutput={powerTrader.PowerOutput}"
                );
    }
}
