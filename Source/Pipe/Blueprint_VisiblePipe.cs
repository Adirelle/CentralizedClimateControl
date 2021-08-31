using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class Blueprint_VisiblePipe : Blueprint_Build
    {
        public override Graphic Graphic => DefaultGraphic.LinkedPipe();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            map.NetworkManager().ClearCache(Position);
        }
    }
}
