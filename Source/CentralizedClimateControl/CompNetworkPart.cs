using Verse;
using System.Text;

namespace CentralizedClimateControl
{
    public class CompNetworkPart : ThingComp
    {
        public AirNetwork Network;

        public bool IsConnected => Network != null;

        public FlowType FlowType => Network?.FlowType ?? NetworkPartProps.flowType;

        private CompProperties_NetworkPart NetworkPartProps => (CompProperties_NetworkPart) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterPart(this);
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override void PostDeSpawn(Map map)
        {
            CentralizedClimateControlUtility.GetNetManager(map).DeregisterPart(this);
            base.PostDeSpawn(map);
        }

        public override string CompInspectStringExtra()
        {
            if (!IsConnected)
            {
                // @TODO: translate
                return "Disconnected from air network";
            }

            // @TODO: translate
            return $"Connected to air network";
        }

        public bool IsVisibleOnOverlay(FlowType type)
        {
            return type.Accept(FlowType); 
        }

        public void PrintOnOverlayLayer(SectionLayer layer)
        {
            GraphicsLoader.GetPipeOverlay(NetworkPartProps.flowType)?.Print(layer, parent, 0);
        }
    }
}
