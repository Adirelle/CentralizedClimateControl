using Verse;
using System.Text;
using System.Collections.Generic;

namespace CentralizedClimateControl
{
    public class CompNetworkPart : ThingComp
    {
        public Network Network;

        public bool IsConnected => Network != null;

        public FlowType FlowType => NetworkPartProps.flowType;

        private CompProperties_NetworkPart NetworkPartProps => (CompProperties_NetworkPart) props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            CentralizedClimateControlUtility.GetNetManager(parent.Map).RegisterPart(this);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            CentralizedClimateControlUtility.GetNetManager(map).DeregisterPart(this);
        }

        public override string CompInspectStringExtra()
        {
            if (!IsConnected)
            {
                // @TODO: translate
                return "Disconnected from air network".Translate();
            }

            // @TODO: translate
            return "Connected to air network".Translate();
        }

        public bool IsVisibleOnOverlay(FlowType type)
        {
            return type.Accept(FlowType); 
        }

        public void PrintOnOverlayLayer(SectionLayer layer)
        {
            GraphicsLoader.GetPipeOverlay(NetworkPartProps.flowType)?.Print(layer, parent, 0);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach(var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }
            /*
            if (NetworkPartProps.flowType.IsAny())
            {
                yield return PipeSwitchGizmo();
            }
            */
        }

        /*
        private Command_Action PipeSwitchGizmo()
        {
            var currentFlow = NetworkPartProps.flowType;
            var next

            return new Command_Action {
                defaultLabel = FlowType.CommandLabelKey().Translate(),
                defaultDesc = "CentralizedClimateControl.Command.SwitchPipe.Desc".Translate(),
                hotKey = KeyBindingDefOf.Misc4,
                icon = ContentFinder<Texture2D>.Get(currentPriority.CommandIconName()),
                action = delegate { compAirFlowConsumer.SetPriority(currentPriority.Next()); }
            };
        }
        */

    }
}
