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

        public AirFlow CurrentProcessed { get; private set; }

        public AirFlow CurrentExhaust => CurrentIntake - CurrentProcessed + CurrentProcessed;

        private bool isDirty = true;

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
            isDirty = false;
        }

        public void Tick()
        {
            if (!isDirty)
            {
                return;
            }

            // Calculate the network capacity
            MaxExhaust = vents.Sum(vent => vent.MaxExhaust);
            MaxIntake = intakes.Sum(intake => intake.MaxIntake);
            MaxProcessing = tempControls.Sum(control => control.MaxInput);

            if (Mathf.Approximately(CurrentThroughput, 0.0f))
            {
                CurrentIntake = AirFlow.Zero;
                CurrentProcessed = AirFlow.Zero;
                return;
            }

            CurrentIntake = intakes.Select(intake => intake.Intake).Sum();
            CurrentProcessed = tempControls.Select(control => control.Output).Sum();

            isDirty = false;
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

            isDirty = true;
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

            NotifyPartChange();
        }

        public void NotifyPartChange()
        {
            isDirty = true;
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
