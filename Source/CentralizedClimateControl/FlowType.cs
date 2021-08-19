
using Verse;

namespace CentralizedClimateControl
{
    public enum FlowType : int
    {
        None = 0,
        Hot = LinkFlags.Custom6,
        Cold = LinkFlags.Custom7,
        Frozen = LinkFlags.Custom8,
        Any = Hot | Cold | Frozen,
    }

    public static class FlowTypeExtensions
    {
        public static bool IsValid(this FlowType type)
        {
            return type is FlowType.None or FlowType.Hot or FlowType.Cold or FlowType.Frozen or FlowType.Any;
        }

        public static bool IsAny(this FlowType type)
        {
            return type == FlowType.Any;
        }

        public static bool IsNone(this FlowType type)
        {
            return type == FlowType.None;
        }

        public static bool IsConcrete(this FlowType type)
        {
            return type is FlowType.Hot or FlowType.Cold or FlowType.Frozen;
        }

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

        public static LinkFlags ToLinkFlags(this FlowType type)
        {
            return (LinkFlags) type;
        }

        public static FlowType ToFlowType(this LinkFlags flags)
        {
            return ((FlowType) flags) & FlowType.Any;
        }

        public static FlowType GetFlowType(this ThingDef def)
        {
            return def.graphicData?.linkFlags.ToFlowType() ?? FlowType.None;
        }

        public static FlowType GetFlowType(this Thing thing)
        {
            return thing.def.GetFlowType();
        }

        public static FlowType GetFlowType(this ThingComp comp)
        {
            return comp.parent.GetFlowType();
        }
    }
}
