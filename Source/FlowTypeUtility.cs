
using Verse;

namespace CentralizedClimateControl
{
    public static class FlowTypeUtility
    {
        public static string ToString(this FlowType type)
        {
            return type switch
            {
                FlowType.None => "None",
                FlowType.Blue => "Blue",
                FlowType.Red => "Red",
                FlowType.Cyan => "Cyan",
                FlowType.Any => "Any",
                _ => "Unknown"
            };
        }

        public static TaggedString Translate(this FlowType type)
        {
            return $"CentralizedClimateControl.FlowType.{type}".Translate();
        }

        public static UnityEngine.Color Color(this FlowType type)
        {
            return type switch
            {
                FlowType.Blue => UnityEngine.Color.blue,
                FlowType.Red => UnityEngine.Color.red,
                FlowType.Cyan => UnityEngine.Color.cyan,
                _ => UnityEngine.Color.white
            };
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
