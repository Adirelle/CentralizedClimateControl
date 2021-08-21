using Verse;

namespace CentralizedClimateControl
{
    internal static class CentralizedClimateControlUtility
    {
        public static NetworkManager NetworkManager(this Map map)
        {
            return map.GetComponent<NetworkManager>();
        }
    }
}
