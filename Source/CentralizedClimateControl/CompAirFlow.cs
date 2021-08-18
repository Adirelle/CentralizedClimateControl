using Verse;
using System.Text;

namespace CentralizedClimateControl
{
    public class CompAirFlow : ThingComp
    {
        public const string NotConnectedKey = "CentralizedClimateControl.AirFlowNetDisconnected";
        public const string ConnectedKey = "CentralizedClimateControl.AirFlowNetConnected";
        public const string AirTypeKey = "CentralizedClimateControl.AirType";
        public const string TotalNetworkAirKey = "CentralizedClimateControl.TotalNetworkAir";

        public virtual FlowType FlowType => global::CentralizedClimateControl.FlowType.Any;

        public int GridID = -2;

        public AirNetwork AirFlowNet;

        public virtual void ResetFlowVariables()
        {
            AirFlowNet = null;
            GridID = -1;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterPipe(this);
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDeSpawn(Map map)
        {
            CentralizedClimateControlUtility.GetNetManager(map).DeregisterPipe(this);
            ResetFlowVariables();

            base.PostDeSpawn(map);
        }

        public override string CompInspectStringExtra()
        {
            var builder = new StringBuilder();

            if (FlowType != global::CentralizedClimateControl.FlowType.Any)
            {
                builder.AppendLine(AirTypeKey.Translate(FlowType.ToKey().Translate()));
            }

            if (AirFlowNet != null)
            {
                builder.AppendLine(ConnectedKey.Translate());
                builder.AppendLine(TotalNetworkAirKey.Translate(AirFlowNet.CurrentIntakeAir));
            }
            else
            {
                builder.AppendLine(NotConnectedKey.Translate());
            }

            return builder.ToString();
        }

        public void PrintForGrid(SectionLayer layer, FlowType type)
        {
            GraphicsLoader.GetPipeOverlay(type)?.Print(layer, parent, 0);
        }
    }
}
