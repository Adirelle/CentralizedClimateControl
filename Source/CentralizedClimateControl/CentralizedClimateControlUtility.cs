using Verse;

namespace CentralizedClimateControl
{
    internal static class CentralizedClimateControlUtility
    {
        public static NetworkManager NetworkManager(this Map map)
        {
            return map.GetComponent<NetworkManager>();
        }

        public static float TickDuration(this TickerType type) => type switch
        {
            TickerType.Never => float.PositiveInfinity,
            TickerType.Normal => 1.0f / 60.0f,
            TickerType.Rare => 250.0f / 60.0f,
            TickerType.Long => 2000.0f / 60.0f,
            _ => throw new System.InvalidOperationException()
        };
    }
}
