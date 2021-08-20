using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompBase : ThingComp
    {
        public Network Network { get; private set; } = null;

        public bool IsConnected => Network is not null;

        public virtual bool IsOperating => IsConnected;

        public virtual FlowType FlowType => FlowType.None;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
#if DEBUG
            Log.Message("registering network part");
#endif
            parent.Map.NetworkManager().RegisterPart(this);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
#if DEBUG
            Log.Message("deregistering network part");
#endif
            map.NetworkManager().DeregisterPart(this);
        }

        public void ConnectTo(Network network)
        {
            if (Network == network)
            {
                return;
            }
            Network = network;
            if (parent.Spawned)
            {
                this.CompTickRare();
            }
        }

        public void Disconnect()
        {
            if (Network is null)
            {
                return;
            }
            Network = null;
            if (parent.Spawned)
            {
                this.CompTickRare();
            }
        }

        public sealed override string CompInspectStringExtra()
        {
            StringBuilder builder = new StringBuilder();
            BuildInspectString(builder);
            return builder.ToString();
        }

        protected virtual void BuildInspectString(StringBuilder builder)
        {
            builder.AppendInNewLine(
                IsConnected
                // @TODO: translate
                ? "Connected to air network".Translate()
                // @TODO: translate
                : "Disconnected from air network".Translate()
                );
        }

        public virtual string DebugString() => $"NetworkPart(FlowType={FlowType}, IsConnected={IsConnected}, NetworkId={Network?.NetworkId})";
        /*

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var gizmo in base.CompGetGizmosExtra())
        {
            yield return gizmo;
        }
        if (NetworkPartProps.flowType.IsAny())
        {
            yield return PipeSwitchGizmo();
        }
    
    }

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

