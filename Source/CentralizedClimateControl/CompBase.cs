using System.Text;
using Verse;

namespace CentralizedClimateControl
{
    public abstract class CompBase : ThingComp
    {
        public Network Network;

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

        public virtual void NetworkPreTick()
        {
        }

        public virtual void NetworkPostTick()
        {
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
                builder.AppendInNewLine("CentralizedClimateControl.Disconnected".Translate());
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

