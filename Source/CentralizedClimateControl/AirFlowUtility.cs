
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public static class AirFlowUtility
    {
        public static string ToStringThroughput(this float throughput)
        {
            // @TODO: translate
            return "{0}cc/s".Translate(throughput.ToStringDecimalIfSmall());
        }

        public static AirFlow Sum(this IEnumerable<AirFlow> flows)
        {
            var weightedTempSum = flows.Sum(f => f.Temperature * f.Throughput);
            var throughputSum = flows.Sum(f => f.Throughput);
            return AirFlow.Make(throughputSum, weightedTempSum / throughputSum);
        }
    }
}
