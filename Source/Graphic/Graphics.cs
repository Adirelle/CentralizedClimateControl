using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    [StaticConstructorOnStartup]
    internal static class Graphics
    {
        private static readonly Dictionary<FlowType, Graphic> pipeOverlays = new();

        private static readonly Dictionary<FlowType, Texture2D> preferredFlowTypeIcons = new();

        private static readonly Dictionary<GraphicData, Graphic> linkedPipes = new();

        static Graphics()
        {
            var baseOverlay = GraphicDatabase.Get<Graphic_Single>("Things/Building/PipeAtlas/Overlay", ShaderDatabase.MetaOverlay);

            _ = GraphicDatabase.Get<Graphic_Single>("Things/Building/PipeAtlas/Pipe", ShaderDatabase.CutoutComplex);

            void addFlowType(FlowType flowType)
            {
                var overlayColor = flowType.Color();
                overlayColor.a = 0.75f;
                pipeOverlays[flowType] = new Graphic_LinkedPipeOverlay(baseOverlay.Colored(overlayColor), flowType);
                preferredFlowTypeIcons[flowType] = ContentFinder<Texture2D>.Get($"UI/Preferred/{flowType}");
            }

            foreach (var flowType in FlowTypeUtility.All())
            {
                addFlowType(flowType);
            }
        }

        public static Graphic Colored(this Graphic graphic, FlowType flowType)
            => graphic.Colored(flowType.Color());

        public static Graphic Colored(this Graphic graphic, Color color, Color? colorTwo = null)
            => graphic.GetColoredVersion(graphic.Shader, color, colorTwo ?? graphic.colorTwo);

        public static Graphic Overlay(this FlowType flowType)
            => pipeOverlays[flowType];

        public static Texture2D PreferredIcon(this FlowType flowType)
            => preferredFlowTypeIcons[flowType];

        public static Graphic LinkedPipe(this Graphic graphic)
        {
            if (!linkedPipes.TryGetValue(graphic.data, out var linked))
            {
                linked = new Graphic_LinkedPipe(graphic);
                linkedPipes[graphic.data] = linked;
            }
            return linked;
        }
    }
}
