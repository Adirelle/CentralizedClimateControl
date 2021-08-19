using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    internal static class OverlayGraphics
    {
        private static readonly Graphic hotOverlayGraphic;
        private static readonly Graphic coldOverlayGraphic;
        private static readonly Graphic frozenOverlayGraphic;
        private static readonly Graphic anyOverlayGraphic;

        static OverlayGraphics()
        {
            static Graphic load(string name) => new Graphic_Linked(
                GraphicDatabase.Get<Graphic_Single>($"Things/Building/{name}_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay)
            );

            hotOverlayGraphic = load("Hot");
            coldOverlayGraphic = load("Cold");
            frozenOverlayGraphic = load("Frozen");
            anyOverlayGraphic = load("Any");
        }

        public static Graphic Get(FlowType flowType)
        {
            return flowType switch
            {
                FlowType.Hot => hotOverlayGraphic,
                FlowType.Cold => coldOverlayGraphic,
                FlowType.Frozen => frozenOverlayGraphic,
                FlowType.Any => anyOverlayGraphic,
                _ => null
            };
        }

    }
}
