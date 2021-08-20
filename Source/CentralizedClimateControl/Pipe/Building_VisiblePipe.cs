using Verse;

namespace CentralizedClimateControl
{
    public class Building_VisiblePipe : Building
    {
        public override Graphic Graphic => Graphics.PipeGraphic(def.graphicData.Graphic);
    }
}
