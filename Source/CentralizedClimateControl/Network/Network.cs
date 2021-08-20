using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public class Network
    {
        public readonly int NetworkId;
        public FlowType FlowType;

        private readonly List<CompBase> parts = new();
        private readonly List<CompPipe> pipes = new();
        private readonly List<CompVent> vents = new();
        private readonly List<CompIntake> intakes = new();
        private readonly List<CompTempControl> tempControls = new();

        public Network(int networkId, FlowType flowType)
        {
            NetworkId = networkId;
            FlowType = flowType;
        }

        public void Clear()
        {
            parts.ForEach(part => part.Disconnect());
            parts.Clear();
            pipes.Clear(); 
            vents.Clear();
            intakes.Clear();
            tempControls.Clear();
        }

        public void Tick()
        {
            // Calculate the network capacity
            var maxExhaust = vents.Sum(vent => vent.MaxExhaust);
            var maxIntake = intakes.Sum(intake => intake.MaxIntake);
            var maxProcessing = tempControls.Sum(control => control.MaxInput);

            // Calculate the current usage
            var currentThroughput = Mathf.Min(maxExhaust, maxIntake);

            if (Mathf.Approximately(currentThroughput, 0.0f))
            {
                // Shortcut if the network is not operating
                intakes.ForEach(intake => intake.NetworkLoad = 0.0f);
                tempControls.ForEach(control => control.Input = AirFlow.Zero);
                vents.ForEach(vent => vent.Exhaust = AirFlow.Zero);
                return;
            }

            // Retropropagate exhaust capacity to intake fans
            var inputLoad = currentThroughput / maxIntake;
            intakes.ForEach(intake => intake.NetworkLoad = inputLoad);

            // Gather all intake air flows
            var totalIntake = AirFlow.Collect(intakes.Select(intake => intake.Intake));

            AirFlow totalExhaust;
            if (Mathf.Approximately(maxProcessing, 0.0f))
            {
                // Shortcut if there are no operating temperature controllers
                tempControls.ForEach(control => control.Input = AirFlow.Zero);
                totalExhaust = totalIntake;
            }
            else
            {
                // Dispatch intake air flows to the temperature controllers
                tempControls.ForEach(control => control.Input = totalIntake * (control.MaxInput / maxProcessing));

                // Gather all processed air flows
                var totalProcessed = AirFlow.Collect(tempControls.Select(control => control.Output));

                // Calculate the exhausted air flow
                var partialIntake = totalIntake.Clamp(Mathf.Max(0.0f, totalIntake.Throughput - totalProcessed.Throughput));
                totalExhaust = partialIntake + totalProcessed;
            }

            // Dispatch processed air flows to the vents
            var exhaustLoad = currentThroughput / maxExhaust;
            vents.ForEach(vent => vent.Exhaust = totalExhaust * (vent.MaxExhaust / maxExhaust * exhaustLoad));
        }

        public void RegisterPart(CompBase part)
        {
            if (parts.Contains(part))
            {
                return;
            }

            parts.Add(part);
            part.ConnectTo(this);

            var pipe = part.parent.TryGetComp<CompPipe>();
            if (pipe != null)
            {
                pipes.Add(pipe);
            }

            var vent = part.parent.TryGetComp<CompVent>();
            if (vent != null)
            {
                vents.Add(vent);
            }

            var intake = part.parent.TryGetComp<CompIntake>();
            if (intake != null)
            {
                intakes.Add(intake);
            }

            var tempControl = part.parent.TryGetComp<CompTempControl>();
            if (tempControl != null)
            {
                tempControls.Add(tempControl);
            }
        }

        public void DeregisterPart(CompBase part)
        {
            if (!parts.Contains(part))
            {
                return;
            }

            parts.Remove(part);
            part.Disconnect();

            var pipe = part.parent.TryGetComp<CompPipe>();
            if (pipe != null)
            {
                pipes.Remove(pipe);
            }

            var vent = part.parent.TryGetComp<CompVent>();
            if (vent != null)
            {
                vents.Remove(vent);
            }

            var intake = part.parent.TryGetComp<CompIntake>();
            if (intake != null)
            {
                intakes.Remove(intake);
            }

            var tempControl = part.parent.TryGetComp<CompTempControl>();
            if (tempControl != null)
            {
                tempControls.Remove(tempControl);
            }
        }

        public string DebugString()
        {
            var builder = new StringBuilder($"Network(#{NetworkId}, {FlowType})");

            builder.AppendLine($"{parts.Count} parts: {pipes.Count} pipes, {vents.Count} vents, {intakes.Count} intakes, {tempControls.Count} controls");

            return builder.ToString();
        }
    }
}
