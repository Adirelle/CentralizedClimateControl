using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class PlaceWorker_AirPipe : PlaceWorker
    {
        /// <summary>
        ///     Place Worker for Air Pipes. Checks if Air Pipes are in a Suitable Location or not.
        ///     Checks:
        ///     - Current Cell shouldn't have an Air Flow Building (Since they already have a Pipe)
        /// </summary>
        /// <param name="def">The Def Being Built</param>
        /// <param name="loc">Target Location</param>
        /// <param name="rot">Rotation of the Object to be Placed</param>
        /// <param name="map"></param>
        /// <param name="thingToIgnore">Unused field</param>
        /// <param name="thing"></param>
        /// <returns>Boolean/Acceptance Report if we can place the object of not.</returns>
        public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 loc, Rot4 rot, Map map,
            Thing thingToIgnore = null, Thing thing = null)
        {
            var airFlowNuilding = loc.GetFirstThing<Building_AirFlowControl>(map);
            if (airFlowNuilding != null)
            {
                return "cannot build under climate control building";
            }

            var flowType = def.frameDef.GetCompProperties<CompProperties_AirFlow>()?.flowType ?? AirFlowType.Any;
            if (loc.GetThingList(map).OfType<Building_AirPipe>().Where(pipe => pipe.CompAirFlowPipe.FlowType.Matchs(flowType)).Any())
            {
                return "there is a pipe of this type already";
            }

            return true;
        }
    }
}
