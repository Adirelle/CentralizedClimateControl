using Verse;

namespace CentralizedClimateControl
{
    public class Building_AirPipe : Building
    {
        public CompAirFlowPipe CompAirFlowPipe;

        public AirFlowType FlowType => CompAirFlowPipe?.FlowType ?? AirFlowType.Any;

        public bool Hidden => CompAirFlowPipe?.Props.hiddenPipe ?? false;

        public override Graphic Graphic => GraphicsLoader.GetPipeGraphic(FlowType, Hidden);

        /// <summary>
        ///     Building spawned on the map
        /// </summary>
        /// <param name="map">RimWorld Map</param>
        /// <param name="respawningAfterLoad">Unused flag</param>
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CompAirFlowPipe = GetComp<CompAirFlowPipe>();
        }
    }

    // Kept to avoid BC with existing saves

    public class Building_HotAirPipe : Building_AirPipe
    {
    }

    public class Building_ColdAirPipe : Building_AirPipe
    {
    }

    public class Building_FrozenAirPipe : Building_AirPipe
    {
    }
}
