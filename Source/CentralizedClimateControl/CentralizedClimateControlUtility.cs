using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;
using System.Collections.Generic;

namespace CentralizedClimateControl
{
    public static class CentralizedClimateControlUtility
    {
        /// <summary>
        ///     Get the Network Manager of the Map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <returns>AirFlow Net Manager</returns>
        public static NetworkManager GetNetManager(Map map)
        {
            return map.GetComponent<NetworkManager>();
        }
    }
}
