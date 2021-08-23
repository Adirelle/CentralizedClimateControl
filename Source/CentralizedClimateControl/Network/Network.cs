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
            parts.ForEach(part => part.Network = null);
            parts.Clear();
            vents.Clear();
            intakes.Clear();
            tempControls.Clear();
            isDirty = false;
        }

        private bool DoesTick()
        {
            return (Find.TickManager.TicksGame + NetworkId.HashOffset()) % 60 != 0;
        }

        public void NetworkTick()
        {
            if (isDirty || DoesTick())
            {
                DoNetworkTick();
                isDirty = false;
            }
        }

        private void DoNetworkTick()
        {
            // Calculate the network capacities
            vents.ForEach(vent => vent.NetworkPreTick());
            MaxExhaust = vents.Sum(vent => vent.MaxExhaust);

            intakes.ForEach(intake => intake.NetworkPreTick());
            MaxIntake = intakes.Sum(intake => intake.MaxIntake);

            tempControls.ForEach(control => control.NetworkPreTick());
            MaxProcessing = tempControls.Sum(control => control.MaxInput);

            // Update intake
            intakes.ForEach(intake => intake.NetworkPostTick());
            CurrentIntake = intakes.Select(intake => intake.Intake).Sum();

            // Update processing
            tempControls.ForEach(control => control.NetworkPostTick());
            CurrentProcessed = tempControls.Select(control => control.Output).Sum();

            // Update exhaust
            vents.ForEach(vent => vent.NetworkPostTick());
        }

        public void RegisterPart(CompBase part)
        {
            if (parts.Contains(part))
            {
                return;
            }

            parts.Add(part);
            part.Network = this;
            isDirty = true;

            if (part is CompVent vent)
            {
                vents.Add(vent);
            }

            if (part is CompIntake intake)
            {
                intakes.Add(intake);
            }

            if (part is CompTempControl tempControl)
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
            part.Network = null;
            isDirty = true;

            if (part is CompVent vent)
            {
                vents.Remove(vent);
            }

            if (part is CompIntake intake)
            {
                intakes.Remove(intake);
            }

            if (part is CompTempControl tempControl)
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
                $"#Parts (t/i/c/v)={parts.Count}/{intakes.Count}/{tempControls.Count}/{vents.Count}",
                $"Intake (cur/total)={CurrentIntake}/{MaxIntake}",
                $"Processing (cur/total)={CurrentProcessed}/{MaxProcessing}",
                $"Exhaust (cur/total)={CurrentExhaust}/{MaxExhaust}"
            );
    }
}
