using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_Pipe : Building
    {
        public override Graphic Graphic => graphic;

        private Graphic graphic;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            var pipe = GetComp<CompPipe>();
            graphic = GraphicsLoader.GetPipeGraphic(pipe.FlowType, pipe.Hidden);
        }
    }
}
