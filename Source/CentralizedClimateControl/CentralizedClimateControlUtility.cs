using RimWorld;
using UnityEngine;
using Verse;

namespace CentralizedClimateControl
{
    public static class CentralizedClimateControlUtility
    {
        /// <summary>
        ///     Get the Network Manager of the Map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <returns>AirFlow Net Manager</returns>
        public static AirFlowNetManager GetNetManager(Map map)
        {
            return map.GetComponent<AirFlowNetManager>();
        }

        /// <summary>
        ///     Gizmo for Changing Pipes
        /// </summary>
        /// <param name="compAirFlowConsumer">Component Asking for Gizmo</param>
        /// <returns>Action Button Gizmo</returns>
        public static Command_Action GetPipeSwitchToggle(CompAirFlowConsumer compAirFlowConsumer)
        {
            var currentPriority = compAirFlowConsumer.AirTypePriority;

            return new Command_Action {
                defaultLabel = currentPriority.CommandLabelKey().Translate(),
                defaultDesc = "CentralizedClimateControl.Command.SwitchPipe.Desc".Translate(),
                hotKey = KeyBindingDefOf.Misc4,
                icon = ContentFinder<Texture2D>.Get(currentPriority.CommandIconName()),
                action = delegate { compAirFlowConsumer.SetPriority(currentPriority.Next()); }
            };
        }
    }
}
