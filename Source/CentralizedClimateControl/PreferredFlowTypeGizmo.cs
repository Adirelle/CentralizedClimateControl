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
            FlowType.Hot => FlowType.Cold,
            FlowType.Cold => FlowType.Frozen,
            FlowType.Frozen => FlowType.Any,
            _ => FlowType.Hot,
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
                FlowType.Hot => "Red only".Translate(),

                // @TODO: translate
                FlowType.Cold => "Blue only".Translate(),

                // @TODO: translate
                FlowType.Frozen => "Cyan only".Translate(),

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
