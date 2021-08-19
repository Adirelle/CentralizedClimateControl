using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    public static class GraphicsLoader
    {
        private static readonly Graphic[] pipesGraphics;
        private static readonly Graphic hiddenPipeGraphic;
        private static readonly Graphic[] overlayGraphics;

        static GraphicsLoader()
        {
            var hotPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Atlas", ShaderDatabase.Transparent);
            var coldPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Atlas", ShaderDatabase.Transparent);
            var frozenPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Atlas", ShaderDatabase.Transparent);

            pipesGraphics = new Graphic[] {
                null,
                new GraphicPipe(hotPipeAtlas, FlowType.Hot),
                new GraphicPipe(coldPipeAtlas, FlowType.Cold),
                new GraphicPipe(frozenPipeAtlas, FlowType.Frozen),
                null
            };

            var hiddenPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Blank_AirPipe_Atlas", ShaderDatabase.Transparent);

            hiddenPipeGraphic = new GraphicPipe(hiddenPipeAtlas, FlowType.Any);

            var hotOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var coldOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var frozenOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var anyOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Any_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);

            overlayGraphics = new Graphic[] {
                null,
                new GraphicPipe_Overlay(hotOverlayAtlas, anyOverlayAtlas, FlowType.Hot),
                new GraphicPipe_Overlay(coldOverlayAtlas, anyOverlayAtlas, FlowType.Cold),
                new GraphicPipe_Overlay(frozenOverlayAtlas, anyOverlayAtlas, FlowType.Frozen),
                new GraphicPipe(anyOverlayAtlas, FlowType.Frozen),
            };
        }

        public static Graphic GetPipeGraphic(FlowType type, bool hidden)
        {
            return hidden ? hiddenPipeGraphic : pipesGraphics[(int) type];
        }

        public static Graphic GetPipeOverlay(FlowType type)
        {
            return overlayGraphics[(int) type];
        }
    }
}
