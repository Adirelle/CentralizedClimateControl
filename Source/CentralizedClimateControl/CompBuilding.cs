﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompBuilding : CompBase
    {
        public List<IntVec3> Area { get; private set; }

        public List<IntVec3> FreeArea { get; private set; }

        public bool IsBlocked => FreeArea.Count == 0;

        public override FlowType FlowType => FlowType.Any;

        public override bool IsOperating => base.IsOperating && !IsBlocked && flickable.SwitchIsOn;

        protected CompFlickable flickable;

        protected CompProperties_Building Props => (CompProperties_Building) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            flickable = parent.GetComp<CompFlickable>();
            Area = null;
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (Area is null)
            {
                Area = Props.shape.Cells(parent).ToList();
            }

            FreeArea = Area.Where(cell => !cell.Impassable(parent.Map)).ToList();
        }
    }
}
