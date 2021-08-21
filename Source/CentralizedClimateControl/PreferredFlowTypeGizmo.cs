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

            // @TODO: translate
            defaultDesc = "Click to change color affinity".Translate();
            hotKey = KeyBindingDefOf.Misc4;

            NotifyChange();
        }

        public void NotifyChange()
        {
            icon = Graphics.PreferredFlowTypeIcons(CurrentFlowType);
            defaultLabel = CurrentFlowType switch
            {
                // @TODO: translate
                FlowType.Red => "Red only".Translate(),

                // @TODO: translate
                FlowType.Blue => "Blue only".Translate(),

                // @TODO: translate
                FlowType.Cyan => "Cyan only".Translate(),

                // @TODO: translate
                _ => "Auto".Translate(),
            };
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            parent.SetPreferredFlowType(NextFlowType);
            NotifyChange();
        }
    }
}
