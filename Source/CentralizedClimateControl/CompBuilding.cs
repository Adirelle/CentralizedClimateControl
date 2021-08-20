using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompBuilding : CompBase
    {
        public AreaShape AreaShape => Props.shape;

        public List<IntVec3> Area { get; private set; }

        public List<IntVec3> ClearArea { get; private set; }

        public bool IsBlocked => ClearArea?.Count == 0;

        public override FlowType FlowType => flowType;

        public override bool IsOperating => base.IsOperating && !IsBlocked && flickable.SwitchIsOn;

        protected CompFlickable flickable;

        private FlowType flowType = FlowType.Any;

        protected CompProperties_Building Props => (CompProperties_Building) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            flickable = parent.GetComp<CompFlickable>();
            Area = null;
        }

        protected override void PostConnected() {
            flowType = Network.FlowType;
            parent.Map.NetworkManager().NotifyChange(this);
        }

        protected override void PostDisconnected() {
            flowType = FlowType.Any;
            parent.Map.NetworkManager().NotifyChange(this);
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (Area is null)
            {
                Area = Props.shape.Cells(parent).ToList();
            }

            ClearArea = Area.AsEnumerable().IsClear(parent.Map, parent).ToList();
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            if (IsBlocked)
            {
                // @TODO: translate
                builder.AppendInNewLine("Blocked by nearby buildings".Translate());
            }
            else if (ClearArea.Count < Area.Count)
            {
                // @TODO: translate
                builder.AppendInNewLine("Performance is hindered by nearby buildings".Translate());
            }
        }

        public override string DebugString() =>
            string.Join("\n",
                base.DebugString(),
                $"IsOperating={IsOperating}",
                $"Area.Count={Area?.Count}",
                $"ClearArea.Count={ClearArea?.Count}",
                $"IsBlocked={IsBlocked}",
                $"SwitchIsOn={flickable.SwitchIsOn}"
            );
    }
}
