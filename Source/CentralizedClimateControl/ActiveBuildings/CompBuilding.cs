using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompBuilding : CompBase
    {
        public AreaShape AreaShape => Props.shape;

        public List<IntVec3> Area { get; private set; }

        public List<IntVec3> ClearArea { get; private set; }

        public float ThroughputCapacity => Props.baseThroughput;

        public float AvailableThroughput => Props.baseThroughput * ClearArea.Count / Area.Count;

        public bool IsBlocked => ClearArea?.Count == 0;

        public FlowType PreferredFlowType = FlowType.Any;

        public override FlowType FlowType => Network?.FlowType ?? PreferredFlowType;

        public override bool IsOperating => base.IsOperating && !IsBlocked && flickable.SwitchIsOn;

        protected CompFlickable flickable;

        private readonly PreferredFlowTypeGizmo preferredFlowTypeGizmo;

        protected CompProperties_Building Props => (CompProperties_Building) props;

        public CompBuilding() : base()
        {
            preferredFlowTypeGizmo = new PreferredFlowTypeGizmo(this);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            flickable = parent.GetComp<CompFlickable>() ?? throw new System.NullReferenceException("could not find a CompFlickable");
            Area = AreaShape.Cells(parent).ToList();
            ClearArea = Area;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref PreferredFlowType, "preferredFlowType", FlowType.Any);
        }

        public void SetPreferredFlowType(FlowType preferredFlowType)
        {
            if (preferredFlowType != PreferredFlowType)
            {
                PreferredFlowType = preferredFlowType;
                Network = null;
                parent.Map.NetworkManager().NotifyChange(this);
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            yield return preferredFlowTypeGizmo;
        }

        public override void NetworkPreTick()
        {
            base.NetworkPreTick();
            ClearArea = Area.AsEnumerable().IsClear(parent.Map, parent).ToList();
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            if (IsBlocked)
            {
                builder.AppendInNewLine("CentralizedClimateControl.Blocked".Translate());
            }
            else if (ClearArea.Count < Area.Count)
            {
                builder.AppendInNewLine("CentralizedClimateControl.PartiallyBlocked".Translate());
            }
        }
        public override string DebugString() =>
            string.Join("\n",
                base.DebugString(),
                $"PreferredFlowType={PreferredFlowType}",
                $"IsOperating={IsOperating}",
                $"Area.Count={Area?.Count}",
                $"ClearArea.Count={ClearArea?.Count}",
                $"IsBlocked={IsBlocked}",
                $"SwitchIsOn={flickable.SwitchIsOn}"
            );
    }
}
