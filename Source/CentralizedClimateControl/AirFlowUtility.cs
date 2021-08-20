
using Verse;

namespace CentralizedClimateControl
{
    public static class AirFlowUtility
    {
        public static string ToStringThroughput(this float throughput)
        {
            // @TODO: translate
            return "{0} cc/s".Translate(throughput.ToStringDecimalIfSmall());
        }
    }
}
