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
        protected const float RareTickDuration = 250.0f / 60.0f;

        public AreaShape AreaShape => Props.shape;

        public List<IntVec3> Area { get; private set; }

        public List<IntVec3> ClearArea { get; private set; }

        public float ThroughputCapacity => Props.flowPerTile * Area.Count;

        public float AvailableThroughput => Props.flowPerTile * ClearArea.Count;

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
            preferredFlowTypeGizmo.NotifyChange();
        }

        public void SetPreferredFlowType(FlowType preferredFlowType)
        {
            if (preferredFlowType != PreferredFlowType)
            {
                PreferredFlowType = preferredFlowType;
                preferredFlowTypeGizmo.NotifyChange();
                Disconnect();
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

        public override void CompTickRare()
        {
            base.CompTickRare();
            ClearArea = Area.AsEnumerable().IsClear(parent.Map, parent).ToList();
        }

        protected override void BuildInspectString(StringBuilder builder)
        {
            base.BuildInspectString(builder);

            if (IsBlocked)
            {
                // @TRANSLATE: Blocked by nearby buildings
                builder.AppendInNewLine("CentralizedClimateControl.Blocked".Translate());
            }
            else if (ClearArea.Count < Area.Count)
            {
                // @TRANSLATE: Performance is hindered by nearby buildings
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
