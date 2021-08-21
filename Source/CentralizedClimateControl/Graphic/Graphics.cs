using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    internal static class Graphics
    {
        private static readonly Dictionary<FlowType, Graphic> pipeOverlays = new();

        private static readonly Dictionary<string, Graphic> pipeGraphics = new();

        private static readonly Dictionary<FlowType, Texture2D> preferredFlowTypeIcons = new();

        static Graphics()
        {
            var overlayGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/PipeAtlas/Overlay", ShaderDatabase.MetaOverlay);

            void loadPipeOverlayGraphic(FlowType flowType)
            {
                var coloredGraphic = overlayGraphic.GetColoredVersion(overlayGraphic.Shader, flowType.Color(), overlayGraphic.colorTwo);
                pipeOverlays.Add(flowType, new Graphic_LinkedPipeOverlay(coloredGraphic, flowType));
            }

            loadPipeOverlayGraphic(FlowType.Any);
            loadPipeOverlayGraphic(FlowType.Red);
            loadPipeOverlayGraphic(FlowType.Blue);
            loadPipeOverlayGraphic(FlowType.Cyan);

            void loadPreferredFlowTypeIcon(FlowType flowType)
            {
                var icon = ContentFinder<Texture2D>.Get($"UI/Preferred/{flowType}");
                preferredFlowTypeIcons.Add(flowType, icon);
            }

            loadPreferredFlowTypeIcon(FlowType.Red);
            loadPreferredFlowTypeIcon(FlowType.Blue);
            loadPreferredFlowTypeIcon(FlowType.Cyan);
            loadPreferredFlowTypeIcon(FlowType.Any);
        }

        public static Graphic PipeOverlay(FlowType flowType) => pipeOverlays.GetValueOrDefault(flowType);

        public static Texture2D PreferredFlowTypeIcons(FlowType flowType) => preferredFlowTypeIcons.GetValueOrDefault(flowType);

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
