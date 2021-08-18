using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    public static class GraphicsLoader
    {
        private static Graphic[] PipesGraphics;
        private static Graphic HiddenPipeGraphic;
        private static Graphic[] OverlayGraphics;
         
        static GraphicsLoader() {
            var hotPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Atlas", ShaderDatabase.Transparent);
            var coldPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Atlas", ShaderDatabase.Transparent);
            var frozenPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Atlas", ShaderDatabase.Transparent);

            PipesGraphics = new Graphic[] {
                 new GraphicPipe(hotPipeAtlas, FlowType.Hot),
                 new GraphicPipe(coldPipeAtlas, FlowType.Cold),
                 new GraphicPipe(frozenPipeAtlas, FlowType.Frozen),
            };

            var hiddenPipeAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Blank_AirPipe_Atlas", ShaderDatabase.Transparent);

            HiddenPipeGraphic = new GraphicPipe(hiddenPipeAtlas, FlowType.Any);

            var hotOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Hot_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var coldOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Cold_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var frozenOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Frozen_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);
            var anyOverlayAtlas = GraphicDatabase.Get<Graphic_Single>("Things/Building/Any_AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);

            OverlayGraphics = new Graphic[] {
                new GraphicPipe_Overlay(hotOverlayAtlas, anyOverlayAtlas, FlowType.Hot),
                new GraphicPipe_Overlay(coldOverlayAtlas, anyOverlayAtlas, FlowType.Cold),
                new GraphicPipe_Overlay(frozenOverlayAtlas, anyOverlayAtlas, FlowType.Frozen),
            };
        }

        public static Graphic GetPipeGraphic(FlowType type, bool hidden)
        {
            return hidden ? HiddenPipeGraphic : PipesGraphics[(int) type];
        }

        public static Graphic GetPipeOverlay(FlowType type)
        {
            return OverlayGraphics[(int) type];
        }
    }
}
