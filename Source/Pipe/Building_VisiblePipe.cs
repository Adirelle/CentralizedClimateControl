using Verse;

namespace CentralizedClimateControl
{
    public class Building_VisiblePipe : Building
    {
        public override Graphic Graphic => DefaultGraphic.LinkedPipe();
    }
}
