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

        private FlowType nextFlowType;

        public PreferredFlowTypeGizmo(CompBuilding parent) : base()
        {
            this.parent = parent;

            // @TODO: translate
            defaultDesc = "Click to change color affinity".Translate();
            hotKey = KeyBindingDefOf.Misc4;
        }

        public void NotifyChange()
        {
            icon = Graphics.PreferredFlowTypeIcons(parent.PreferredFlowType);

            (defaultLabel, nextFlowType) = parent.PreferredFlowType switch
            {
                // @TODO: translate
                FlowType.Hot => ("Red only".Translate(), FlowType.Cold),

                // @TODO: translate
                FlowType.Cold => ("Blue only".Translate(), FlowType.Frozen),

                // @TODO: translate
                FlowType.Frozen => ("Cyan only".Translate(), FlowType.Any),

                // @TODO: translate
                _ => ("Auto".Translate(), FlowType.Hot),
            };
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            parent.SetPreferredFlowType(nextFlowType);
        }
    }
}
