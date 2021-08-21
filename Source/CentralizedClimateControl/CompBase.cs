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

        public override void CompTickRare()
        {
            base.CompTickRare();
            Network?.NotifyPartChange();
        }

        public sealed override string CompInspectStringExtra()
        {
            StringBuilder builder = new StringBuilder();
            BuildInspectString(builder);
            if (Prefs.DevMode)
            {
                builder.Append($"\n--- UNIT DEBUG:\n{DebugString().Trim()}");
                if (Network is not null)
                {
                    builder.Append($"\n--- NETWORK DEBUG:\n{Network.DebugString().Trim()}");
                }
            }
            return builder.ToString().Trim();
        }

        protected virtual void BuildInspectString(StringBuilder builder)
        {
            if (!IsConnected)
            {
                builder.AppendInNewLine("Not connected to air grid".Translate());
            }
        }

        public virtual string DebugString() =>
            string.Join("\n",
                $"FlowType={FlowType}",
                $"IsConnected={IsConnected}",
                $"Network={Network?.ToString() ?? "None"}"
            );
    }
}

