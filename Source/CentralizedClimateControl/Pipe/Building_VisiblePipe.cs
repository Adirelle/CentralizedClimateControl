using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace CentralizedClimateControl
{
    public class Building_VisiblePipe : Building
    {
        public override Graphic Graphic => Graphics.PipeGraphic(def.graphicData.Graphic);
    }
}
