using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    internal static class Graphics
    {
        private static readonly Dictionary<FlowType, Graphic> pipeOverlays = new();

        private static readonly Dictionary<string, Graphic> pipeGraphics = new();

        static Graphics()
        {
            var overlayGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/AirPipe_Overlay_Atlas", ShaderDatabase.MetaOverlay);

            void addColoredVersion(FlowType flowType)
            {
                var coloredGraphic = overlayGraphic.GetColoredVersion(overlayGraphic.Shader, flowType.Color(), overlayGraphic.colorTwo);
                pipeOverlays.Add(flowType, new Graphic_LinkedPipeOverlay(coloredGraphic, flowType));
            }

            addColoredVersion(FlowType.Any);
            addColoredVersion(FlowType.Hot);
            addColoredVersion(FlowType.Cold);
            addColoredVersion(FlowType.Frozen);
        }

        public static Graphic PipeOverlay(FlowType flowType) => pipeOverlays.GetValueOrDefault(flowType);

        public static Graphic PipeGraphic(Graphic graphic)
        {
            var key = graphic.data.texPath;
            if (pipeGraphics.TryGetValue(key, out var value))
            {
                return value;
            }
            value = new Graphic_LinkedPipe(graphic);
            pipeGraphics.Add(key, value);
            return value;
        }
    }
}
