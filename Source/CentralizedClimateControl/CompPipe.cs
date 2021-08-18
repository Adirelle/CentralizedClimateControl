using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CentralizedClimateControl
{
    public class CompPipe : ThingComp
    {
        public FlowType FlowType => PipeProps.flowType;
        public bool Hidden => PipeProps.hidden;

        private CompProperties_Pipe PipeProps => (CompProperties_Pipe) props;
    }
}
