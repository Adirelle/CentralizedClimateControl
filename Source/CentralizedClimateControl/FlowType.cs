
using Verse;

namespace CentralizedClimateControl
{
    public enum FlowType : byte
    {
        None = 0x00,
        Hot = 0x01,
        Cold = 0x02,
        Frozen = 0x4,
        Any = Hot | Cold | Frozen,
    }

    public static class FlowTypeExtensions
    {
        public static string ToString(this FlowType type)
        {
            return type switch
            {
                FlowType.None => "None",
                FlowType.Cold => "Cold",
                FlowType.Hot => "Hot",
                FlowType.Frozen => "Frozen",
                FlowType.Any => "Any",
                _ => "Unknown"
            };
        }

        public static string ToKey(this FlowType type)
        {
            return $"CentralizedClimateControl.{type}Air";
        }

        public static UnityEngine.Color Color(this FlowType type)
        {
            return type switch
            {
                FlowType.Cold => UnityEngine.Color.blue,
                FlowType.Hot => UnityEngine.Color.red,
                FlowType.Frozen => UnityEngine.Color.cyan,
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
