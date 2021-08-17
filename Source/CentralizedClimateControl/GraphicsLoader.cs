using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    public class GraphicsLoader
    {
        // Actual Atlas
        private static Graphic BlankPipeAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Blank_AirPipe_Atlas", ShaderDatabase.Transparent);

        private static Graphic HotPipeAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Atlas", ShaderDatabase.Transparent);

        private static Graphic ColdPipeAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Atlas", ShaderDatabase.Transparent);

        private static Graphic FrozenPipeAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Atlas", ShaderDatabase.Transparent);

        // Overlays
        private static Graphic HotPipeOverlayAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Overlay_Atlas",
                ShaderDatabase.MetaOverlay);

        private static Graphic ColdPipeOverlayAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Overlay_Atlas",
                ShaderDatabase.MetaOverlay);

        private static Graphic FrozenPipeOverlayAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Overlay_Atlas",
                ShaderDatabase.MetaOverlay);

        private static Graphic AnyPipeOverlayAtlas =
            GraphicDatabase.Get<Graphic_Single>("Things/Building/Any_AirPipe_Overlay_Atlas",
                ShaderDatabase.MetaOverlay);

        private static GraphicPipe GraphicHotPipe = new GraphicPipe(HotPipeAtlas, AirFlowType.Hot);
        private static GraphicPipe GraphicHotPipeHidden = new GraphicPipe(BlankPipeAtlas, AirFlowType.Hot);
        private static GraphicPipe GraphicColdPipe = new GraphicPipe(ColdPipeAtlas, AirFlowType.Cold);
        private static GraphicPipe GraphicColdPipeHidden = new GraphicPipe(BlankPipeAtlas, AirFlowType.Cold);
        private static GraphicPipe GraphicFrozenPipe = new GraphicPipe(FrozenPipeAtlas, AirFlowType.Frozen);
        private static GraphicPipe GraphicFrozenPipeHidden = new GraphicPipe(BlankPipeAtlas, AirFlowType.Frozen);

        public static GraphicPipe GetPipeGraphic(AirFlowType type, bool hidden)
        {
            if (hidden)
            {
                return type switch {
                    AirFlowType.Cold => GraphicColdPipeHidden,
                    AirFlowType.Frozen => GraphicFrozenPipeHidden,
                    _ => GraphicHotPipeHidden,
                };
            }

            return type switch {
                AirFlowType.Cold => GraphicColdPipe,
                AirFlowType.Frozen => GraphicFrozenPipe,
                _ => GraphicHotPipe
            };
        }

        private static GraphicPipe_Overlay GraphicHotPipeOverlay =
            new GraphicPipe_Overlay(HotPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Hot);

        private static GraphicPipe_Overlay GraphicColdPipeOverlay =
            new GraphicPipe_Overlay(ColdPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Cold);

        private static GraphicPipe_Overlay GraphicFrozenPipeOverlay =
            new GraphicPipe_Overlay(FrozenPipeOverlayAtlas, AnyPipeOverlayAtlas, AirFlowType.Frozen);

        public static GraphicPipe_Overlay GetPipeOverlay(AirFlowType type)
        {
            return type switch {
                AirFlowType.Hot => GraphicHotPipeOverlay,
                AirFlowType.Cold => GraphicColdPipeOverlay,
                AirFlowType.Frozen => GraphicFrozenPipeOverlay,
                _ => null
            };
        }
    }
}
