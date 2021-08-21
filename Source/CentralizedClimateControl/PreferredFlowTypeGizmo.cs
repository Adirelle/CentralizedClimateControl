using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    internal class PreferredFlowTypeGizmo : Command
    {
        private CompBuilding parent;

        private FlowType CurrentFlowType => parent.PreferredFlowType;

        private FlowType NextFlowType => CurrentFlowType switch
        {
            FlowType.Red => FlowType.Blue,
            FlowType.Blue => FlowType.Cyan,
            FlowType.Cyan => FlowType.Any,
            _ => FlowType.Red,
        };

        public PreferredFlowTypeGizmo(CompBuilding parent) : base()
        {
            this.parent = parent;

            // @TRANSLATE: Click to change color affinity
            defaultDesc = "CentralizedClimateControl.PreferredGizmo.Description".Translate();
            hotKey = KeyBindingDefOf.Misc4;

            NotifyChange();
        }

        public void NotifyChange()
        {
            icon = Graphics.PreferredFlowTypeIcons(CurrentFlowType);
            if (CurrentFlowType == FlowType.Any) {
                // @TRANSLATE: Auto
                defaultLabel = "CentralizedClimateControl.PreferredGizmo.Label.Auto".Translate();
            } else {
                // @TRANSLATE: {0} only
                defaultLabel = "CentralizedClimateControl.PreferredGizmo.Label.RestrictedTo".Translate(CurrentFlowType.Translate());
            }
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            parent.SetPreferredFlowType(NextFlowType);
            NotifyChange();
        }
    }
}
