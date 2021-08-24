using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    internal class PreferredFlowTypeGizmo : Command
    {
        private readonly CompBuilding parent;

        private FlowType CurrentFlowType => parent.PreferredFlowType;

        private FlowType NextFlowType => CurrentFlowType switch
        {
            FlowType.Red => FlowType.Blue,
            FlowType.Blue => FlowType.Cyan,
            FlowType.Cyan => FlowType.Any,
            _ => FlowType.Red,
        };

        public override string Label => "CentralizedClimateControl.PreferredGizmo.Label".Translate(PreferredTypeLabel);

        private string PreferredTypeLabel => CurrentFlowType switch
        {
            FlowType.Any => "CentralizedClimateControl.PreferredGizmo.Label.Auto".Translate(),
            _ => "CentralizedClimateControl.PreferredGizmo.Label.RestrictedTo".Translate(CurrentFlowType.Translate())
        };

        public PreferredFlowTypeGizmo(CompBuilding parent) : base()
        {
            this.parent = parent;

            activateSound = SoundDefOf.Click;
            defaultDesc = "CentralizedClimateControl.PreferredGizmo.Description".Translate();
            hotKey = KeyBindingDefOf.Misc4;
        }

        public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
        {
            icon = CurrentFlowType.PreferredIcon();
            base.DrawIcon(rect, buttonMat, parms);
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            parent.SetPreferredFlowType(NextFlowType);
        }
    }
}
