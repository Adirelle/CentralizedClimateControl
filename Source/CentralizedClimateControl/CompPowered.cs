using RimWorld;

namespace CentralizedClimateControl
{
    public abstract class CompPowered : CompBuilding
    {
        public override bool IsOperating => base.IsOperating && powerTrader.PowerOn && !breakdownable.BrokenDown;

        protected CompPowerTrader powerTrader;
        protected CompBreakdownable breakdownable;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            powerTrader = parent.GetComp<CompPowerTrader>();
            breakdownable = parent.GetComp<CompBreakdownable>();
        }
    }
}
