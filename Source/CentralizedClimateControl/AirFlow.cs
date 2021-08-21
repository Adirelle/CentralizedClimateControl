using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Verse;

namespace CentralizedClimateControl
{
    public struct AirFlow
    {
        public readonly float Throughput;

        public readonly float Temperature;

        public float Energy => EnergyPerCelsius(Throughput) * Temperature;

        private AirFlow(float throughput, float temperature)
        {
            Throughput = throughput;
            Temperature = temperature;
        }

        public AirFlow Clamp(float throughput)
            => Make(Mathf.Min(Throughput, throughput), Temperature);

        public override string ToString()
            => $"{Throughput:F0}cc/s at {Temperature:F1}°C";

        public TaggedString Translate()
        // @TRANSLATE: {0} at {1}
            => this ? "CentralizedClimateControl.AirFlow".Translate(ToStringThroughput(), ToStringTemperature()) : 0.0f.ToStringThroughput();

        public string ToStringThroughput()
            => Throughput.ToStringThroughput();

        public string ToStringTemperature()
            => Temperature.ToStringTemperature();

        public static readonly AirFlow Zero = new(0.0f, 0.0f);

        public static AirFlow Make(float throughput, float temperature)
            => Mathf.Approximately(throughput, 0) ? Zero : new AirFlow(throughput, temperature);

        public static AirFlow FromEnergy(float energy, float throughput)
            => Make(throughput, energy / EnergyPerCelsius(throughput));

        public static implicit operator bool(AirFlow flow)
            => !Mathf.Approximately(flow.Throughput, 0.0f);

        public static AirFlow operator +(AirFlow a, AirFlow b)
            => a || b ? Make(
                a.Throughput + b.Throughput,
                (a.Temperature * a.Throughput + b.Temperature * b.Throughput) / (a.Throughput + b.Throughput)
            ) : Zero;

        public static AirFlow operator -(AirFlow a, AirFlow b)
            => a.Throughput > b.Throughput ? Make(a.Throughput - b.Throughput, a.Temperature) : Zero;

        public static AirFlow operator *(AirFlow a, float x)
            => Make(a.Throughput * x, a.Temperature);

        public static AirFlow operator /(AirFlow a, float x)
            => Make(a.Throughput / x, a.Temperature);

        private static float EnergyPerCelsius(float throughput)
            => throughput;
    }
}
