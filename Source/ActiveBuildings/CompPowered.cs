using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompPowered : CompBuilding
    {
        public override bool IsOperating => base.IsOperating && IsPowered;

        protected CompPowerTrader powerTrader;
        protected CompBreakdownable breakdownable;

        protected bool IsPowered => flickable.SwitchIsOn && powerTrader.PowerOn && !breakdownable.BrokenDown;

        protected virtual float TargetRate => IsPowered ? Mathf.Clamp(NeededRate, 0.01f, 1.0f) : 0.0f;

        protected float CurrentRate = 0.0f;

        protected float NeededRate = 1.0f;

        protected virtual float PowerCost => powerTrader.Props.basePowerConsumption
            * (Props.adaptivePowerConsumption ? CurrentRate : 1.0f);

        protected new CompProperties_Powered Props => (CompProperties_Powered) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerTrader = parent.GetComp<CompPowerTrader>() ?? throw new System.NullReferenceException("could not find a CompPowerTrader");
            breakdownable = parent.GetComp<CompBreakdownable>() ?? throw new System.NullReferenceException("could not find a CompBreakdownable");
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref CurrentRate, "currentRate", 0.0f);
            Scribe_Values.Look(ref NeededRate, "neededRate", 1.0f);
            if (float.IsNaN(CurrentRate))
            {
                CurrentRate = 0.0f;
            }
            if (float.IsNaN(NeededRate))
            {
                NeededRate = 1.0f;
            }
        }

        public override void NetworkPostTick()
        {
            if (TargetRate > CurrentRate)
            {
                CurrentRate = Mathf.Min(CurrentRate + Props.maxRateChange, TargetRate);
            }
            else
            {
                CurrentRate = Mathf.Max(CurrentRate - Props.maxRateChange, TargetRate);
            }

            powerTrader.PowerOutput = -PowerCost;
            base.NetworkPostTick();
        }
        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            // @TODO: translate
            builder.AppendInNewLine("CentralizedClimateControl.Powered.Load".Translate(CurrentRate.ToStringPercent()));
        }

        public override string DebugString() =>
                string.Join("\n",
                    base.DebugString(),
                    $"BrokenDown={breakdownable.BrokenDown}",
                    $"PowerNet={powerTrader.PowerNet is not null}",
                    $"PowerOn={powerTrader.PowerOn}",
                    $"PowerOutput={powerTrader.PowerOutput}",
                    $"neededRate={NeededRate}",
                    $"targetRate={TargetRate}",
                    $"currentRate={CurrentRate}"
                );
    }
}
