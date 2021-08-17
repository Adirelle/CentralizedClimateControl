using UnityEngine;

namespace CentralizedClimateControl
{
    public enum AirFlowType
    {
        Hot = 0,
        Cold = 1,
        Frozen = 2,
        Any = 3
    }

    public static class AirFlowTypeExtensions
    {
        public static bool IsAny(this AirFlowType type)
        {
            return type == AirFlowType.Any;
        }

        public static bool Matchs(this AirFlowType a, AirFlowType b)
        {
            return a == b || a == AirFlowType.Any || b == AirFlowType.Any;
        }

        public static string ToKey(this AirFlowType type)
        {
            return type switch {
                AirFlowType.Cold => "CentralizedClimateControl.ColdAir",
                AirFlowType.Hot => "CentralizedClimateControl.HotAir",
                AirFlowType.Frozen => "CentralizedClimateControl.FrozenAir",
                _ => "Unknown"
            };
        }

        public static UnityEngine.Color Color(this AirFlowType type)
        {
            return type switch {
                AirFlowType.Cold => UnityEngine.Color.blue,
                AirFlowType.Hot => UnityEngine.Color.red,
                AirFlowType.Frozen => UnityEngine.Color.cyan,
                _ => UnityEngine.Color.white
            };
        }
    }
}
