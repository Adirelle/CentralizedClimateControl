
using System;
using System.Collections.Generic;
using Verse;

namespace CentralizedClimateControl
{
    public static class FlowTypeUtility
    {
        public static TaggedString Translate(this FlowType type)
        {
            return $"CentralizedClimateControl.FlowType.{type}".Translate();
        }

        private static UnityEngine.Color redPipeColor = new(255f / 255, 120f / 255, 145f / 255);
        private static UnityEngine.Color bluePipeColor = new(100f / 255, 115f / 255, 255f / 255);
        private static UnityEngine.Color cyanPipeColor = new(92f / 255, 211f / 255, 255f / 255);

        public static UnityEngine.Color Color(this FlowType type)
        {
            return type switch
            {
                FlowType.Red => redPipeColor,
                FlowType.Blue => bluePipeColor,
                FlowType.Cyan => cyanPipeColor,
                _ => UnityEngine.Color.white
            };
        }

        public static IEnumerable<FlowType> All()
        {
            foreach (var color in Colors())
            {
                yield return color;
            }
            yield return FlowType.Any;
        }

        public static IEnumerable<FlowType> Colors()
        {
            yield return FlowType.Red;
            yield return FlowType.Blue;
            yield return FlowType.Cyan;
        }

        public static bool Accept(this FlowType selector, FlowType candidate)
        {
            return (selector & candidate) != FlowType.None;
        }

        public static FlowType GetFlowType(this Thing thing)
        {
            var part = thing.TryGetComp<CompBase>();
            return part is not null ? part.FlowType : FlowType.None;
        }

        public static FlowType GetFlowType(this ThingDef def)
        {
            var pipeProps = def.GetCompProperties<CompProperties_Pipe>();
            if (pipeProps is not null)
            {
                return pipeProps.flowType;
            }
            return def.HasAssignableCompFrom(typeof(CompBuilding)) ? FlowType.Any : FlowType.None;
        }

        public static FlowType GetFlowType(this Def def)
        {
            return (def is ThingDef t) ? t.GetFlowType() : FlowType.None;
        }

    }
}
