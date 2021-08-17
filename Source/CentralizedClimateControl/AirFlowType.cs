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
        private const string HotAirKey = "CentralizedClimateControl.HotAir";
        private const string ColdAirKey = "CentralizedClimateControl.ColdAir";
        private const string FrozenAirKey = "CentralizedClimateControl.FrozenAir";

        public static bool IsAny(this AirFlowType type)
        {
            return type == AirFlowType.Any;
        }

        public static bool Matchs(this AirFlowType type, AirFlowType other)
        {
            return type == other || other.IsAny();
        }

        public static GraphicPipe_Overlay GraphicOverlay(this AirFlowType type)
        {
            return type switch {
                AirFlowType.Hot => GraphicsLoader.GraphicHotPipeOverlay,
                AirFlowType.Cold => GraphicsLoader.GraphicColdPipeOverlay,
                AirFlowType.Frozen => GraphicsLoader.GraphicFrozenPipeOverlay,
                _ => null
            };
        }

        public static string ToKey(this AirFlowType type)
        {
            return type switch {
                AirFlowType.Cold => ColdAirKey,
                AirFlowType.Hot => HotAirKey,
                AirFlowType.Frozen => FrozenAirKey,
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
