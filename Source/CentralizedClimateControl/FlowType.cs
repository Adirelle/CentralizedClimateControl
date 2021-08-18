namespace CentralizedClimateControl
{
    public enum FlowType
    {
        Hot = 0,
        Cold = 1,
        Frozen = 2,
        Any = 3
    }

    public static class FlowTypeExtensions
    {
        public static string ToString(this FlowType type)
        {
            return type switch {
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
            return selector == FlowType.Any || candidate == FlowType.Any || selector == candidate;
        }
    }
}
