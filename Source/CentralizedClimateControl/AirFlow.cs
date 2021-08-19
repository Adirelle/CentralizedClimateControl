using System.Collections.Generic;
using UnityEngine;

using Verse;

namespace CentralizedClimateControl
{
    public struct AirFlow
    {
        public readonly float Throughput;

        public readonly float Temperature;

        public AirFlow(float throughput = 0.0f, float temperature = 0.0f)
        {
            Throughput = throughput;
            Temperature = temperature;
        }

        public AirFlow Clamp(float throughput)
        {
            return new AirFlow(Mathf.Min(Throughput, throughput), Temperature);
        }

        public override string ToString()
        {
            return Translate().ToString();
        }

        public TaggedString Translate()
        {
            // @TODO: translate
            return "{0} at {1}".Translate(ToStringThroughput(), ToStringTemperature());
        }

        public string ToStringThroughput()
        {
            return Throughput.ToStringThroughput();
        }

        public string ToStringTemperature()
        {
            return Temperature.ToStringTemperature();
        }
        public static readonly AirFlow Zero = new();

        public static implicit operator bool(AirFlow flow)
        {
            return flow.Throughput > 0;
        }

        public static AirFlow operator +(AirFlow a, AirFlow b)
        {
            var throughput = a.Throughput + b.Throughput;
            return new AirFlow(throughput, ((a.Temperature * a.Throughput) + (b.Temperature * b.Throughput)) / throughput);
        }

        public static AirFlow operator *(AirFlow a, float x)
        {
            return new AirFlow(a.Throughput * x, a.Temperature);
        }

        public static AirFlow operator /(AirFlow a, float x)
        {
            return new AirFlow(a.Throughput / x, a.Temperature);
        }

        public static AirFlow Collect(IEnumerable<AirFlow> flows)
        {
            var throughput = 0.0f;
            var tempSum = 0.0f;

            foreach (var flow in flows)
            {
                throughput += flow.Throughput;
                tempSum += flow.Temperature * flow.Throughput;
            }

            if (throughput > 0)
            {
                return new AirFlow(throughput, tempSum / throughput);
            }

            return Zero;
        }

    }

    public static class AirFlowExtension
    {
        public static string ToStringThroughput(this float throughput)
        {
            // @TODO: translate
            return "{0} cc/s".Translate(throughput.ToStringDecimalIfSmall());
        }
    }
}
