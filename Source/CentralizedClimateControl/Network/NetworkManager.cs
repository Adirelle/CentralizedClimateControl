using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class NetworkManager : MapComponent
    {
        private int networkId;
        private readonly List<CompNetworkPart> parts = new();
        private readonly List<Network> networks = new();
        private bool isDirty = true;
        private readonly Grid[] grids;

        public NetworkManager(Map map) : base(map)
        {
            grids = new Grid[] { new Grid(map), new Grid(map), new Grid(map) };
        }

        public void RegisterPart(CompNetworkPart part)
        {
            if (!parts.Contains(part))
            {
                parts.Add(part);
                isDirty = true;
            }
        }

        public void DeregisterPart(CompNetworkPart part)
        {
            if (parts.Contains(part))
            {
                parts.Remove(part);
                isDirty = true;
            }
        }

        private Grid Grid(FlowType type)
        {
            return type switch
            {
                FlowType.Hot => grids[0],
                FlowType.Cold => grids[1],
                FlowType.Frozen => grids[2],
                _ => null
            };
        }

        private IEnumerable<Grid> Grids(FlowType type)
        {
            switch (type)
            {
                case FlowType.Any:
                    yield return grids[0];
                    yield return grids[1];
                    yield return grids[2];
                    break;
                case FlowType.None:
                    break;
                default:
                    yield return Grid(type);
                    break;
            }
        }

        public CompNetworkPart GetFirstPartAt(IntVec3 loc, FlowType select = FlowType.Any)
        {
            return Grids(select)
                .Select(grid => grid.FindAt(loc, select))
                .FirstOrDefault(part => part != null);
        }

        public IEnumerable<CompNetworkPart> GetAllPartsAt(IntVec3 loc, FlowType select = FlowType.Any)
        {
            return Grids(select)
                .Select(grid => grid.FindAt(loc, select))
                .Where(part => part != null);
        }

        public void NotifyChange()
        {
            isDirty = true;
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            if (!isDirty)
            {
                foreach (var network in networks)
                {
                    network.Tick();
                }
            }
        }

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();

            if (isDirty)
            {
                ReconstructGrids();
                ReconstructNetworks();
                isDirty = false;
            }
        }
        private void ReconstructGrids()
        {
            foreach (var grid in grids)
            {
                grid.Clear();
            }

            foreach (var part in parts)
            {
                foreach (var grid in Grids(part.FlowType))
                {
                    grid.Add(part);
                }
            }
        }

        private void ReconstructNetworks()
        {
            foreach (var network in networks)
            {
                network.Clear();
            }
            var prevNetworks = new Queue<Network>(networks);
            networks.Clear();

            foreach (var part in parts)
            {
                if (part.IsConnected || !part.FlowType.IsConcrete())
                {
                    continue;
                }

                if (!prevNetworks.TryDequeue(out var network))
                {
                    network = new Network();
                }
                network.NetworkId = ++networkId;
                network.FlowType = part.FlowType;
                networks.Add(network);

                BuildNetwork(network, part);
            }
        }

        private void BuildNetwork(Network network, CompNetworkPart source)
        {
            var type = source.FlowType;
            var grid = Grid(type);

            var workQueue = new Queue<CompNetworkPart>();
            workQueue.Enqueue(source);
            network.RegisterPart(source);

            while (workQueue.TryDequeue(out var current))
            {
                foreach (var loc in GenAdj.CellsAdjacentCardinal(current.parent))
                {
                    var newPart = grid.FindAt(loc, type);
                    if (newPart == null || newPart.IsConnected)
                    {
                        continue;
                    }
                    network.RegisterPart(newPart);
                    workQueue.Enqueue(newPart);
                }
            }
        }
    }
}
