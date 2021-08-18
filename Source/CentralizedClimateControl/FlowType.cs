namespace CentralizedClimateControl
{
    public enum FlowType : byte
    {
        None = 0,
        Hot = 1,
        Cold = 2,
        Frozen = 3,
        Any = 4,

        Min = None,
        Max = Any
    }

    public static class FlowTypeExtensions
    {
        public static bool IsValid(this FlowType type)
        {
            return type >= FlowType.Min && type <= FlowType.Max;
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
            return type == FlowType.Hot || type == FlowType.Cold || type == FlowType.Frozen;
        }

        public static string ToString(this FlowType type)
        {
            return type switch {
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
            return type switch {
                FlowType.Cold => UnityEngine.Color.blue,
                FlowType.Hot => UnityEngine.Color.red,
                FlowType.Frozen => UnityEngine.Color.cyan,
                _ => UnityEngine.Color.white
            };
        }

        public static bool Accept(this FlowType selector, FlowType candidate)
        {
            return selector != FlowType.None && candidate != FlowType.None
                && (selector == FlowType.Any || candidate == FlowType.Any || selector == candidate);
        }
    }
}
