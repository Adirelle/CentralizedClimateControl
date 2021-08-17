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
            var airFlowType = (def as ThingDef)?.GetCompProperties<CompProperties_AirFlow>().flowType ?? AirFlowType.Any;

            foreach (var t in loc.GetThingList(map))
            {
                if (t is Building_AirFlowControl)
                {
                    return new AcceptanceReport("there is a climate control building there");
                }

                if (t is Building_AirPipe pipe && pipe.FlowType == airFlowType)
                {
                    return new AcceptanceReport("there is already a pipe of the same color there");
                }
            }

            return AcceptanceReport.WasAccepted;
        }
    }
}
