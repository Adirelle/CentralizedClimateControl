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

        public float MaxIntake { get; private set; }
        public float MaxExhaust { get; private set; }
        public float MaxProcessing { get; private set; }

        public float CurrentThroughput => Mathf.Min(MaxIntake, MaxExhaust);

        public AirFlow CurrentIntake { get; private set; }

        public AirFlow CurrentExhaust { get; private set; }

        public AirFlow CurrentProcessed { get; private set; }

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
            MaxExhaust = vents.Sum(vent => vent.MaxExhaust);
            MaxIntake = intakes.Sum(intake => intake.MaxIntake);
            MaxProcessing = tempControls.Sum(control => control.MaxInput);

            if (Mathf.Approximately(CurrentThroughput, 0.0f))
            {
                // Shortcut if the network is not operating
                intakes.ForEach(intake => intake.NetworkLoad = 0.0f);
                tempControls.ForEach(control => control.Input = AirFlow.Zero);
                vents.ForEach(vent => vent.Exhaust = AirFlow.Zero);
                return;
            }

            // Retropropagate exhaust capacity to intake fans
            var inputLoad = CurrentThroughput / MaxIntake;
            intakes.ForEach(intake => intake.NetworkLoad = inputLoad);

            // Gather all intake air flows
            CurrentIntake = AirFlow.Collect(intakes.Select(intake => intake.Intake));

            if (Mathf.Approximately(MaxProcessing, 0.0f))
            {
                // Shortcut if there are no operating temperature controllers
                tempControls.ForEach(control => control.Input = AirFlow.Zero);
                CurrentProcessed = AirFlow.Zero;
                CurrentExhaust = CurrentIntake;
            }
            else
            {
                // Dispatch intake air flows to the temperature controllers
                tempControls.ForEach(control => control.Input = CurrentIntake * (control.MaxInput / MaxProcessing));

                // Gather all processed air flows
                CurrentProcessed = AirFlow.Collect(tempControls.Select(control => control.Output));

                // Calculate the exhausted air flow
                var partialIntake = CurrentIntake.Clamp(Mathf.Max(0.0f, CurrentIntake.Throughput - CurrentProcessed.Throughput));
                CurrentExhaust = partialIntake + CurrentProcessed;
            }

            // Dispatch processed air flows to the vents
            var exhaustLoad = CurrentThroughput / MaxExhaust;
            vents.ForEach(vent => vent.Exhaust = CurrentExhaust * (vent.MaxExhaust / MaxExhaust * exhaustLoad));
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


        public override string ToString()
        {
            return $"Network(#{NetworkId}, {FlowType})";
        }

        public string DebugString() =>
            string.Join("\n",
                $"NetworkId={NetworkId}",
                $"FlowType={FlowType}",
                $"#Parts (t/p/i/c/v)={parts.Count}/{pipes.Count}/{intakes.Count}/{tempControls.Count}/{vents.Count}",
                $"Intake (cur/total)={CurrentIntake}/{MaxIntake}",
                $"Processing (cur/total)={CurrentProcessed}/{MaxProcessing}",
                $"Exhaust (cur/total)={CurrentExhaust}/{MaxExhaust}"
            );
    }
}
