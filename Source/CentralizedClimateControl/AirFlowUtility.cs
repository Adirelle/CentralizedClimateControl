
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public static class AirFlowUtility
    {
        public static string ToStringThroughput(this float throughput)
        {
            // @TRANSLATE: {0}cc/s
            return "CentralizedClimateControl.Throughput".Translate(throughput.ToStringDecimalIfSmall());
        }

        public static AirFlow Sum(this IEnumerable<AirFlow> flows)
        {
            var weightedTempSum = flows.Sum(f => f.Temperature * f.Throughput);
            var throughputSum = flows.Sum(f => f.Throughput);
            return AirFlow.Make(throughputSum, weightedTempSum / throughputSum);
        }
    }
}
