using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    public class NetworkManager : MapComponent
    {
        private const FlowType dirtyCell = (FlowType) 0xff;

        private readonly List<CompBase> parts = new();
        private readonly List<Network> networks = new();
        private bool isDirty = true;

        private readonly FlowType[] typeCacheGrid;

        public NetworkManager(Map map) : base(map)
        {
            typeCacheGrid = new FlowType[map.cellIndices.NumGridCells];
            Array.Fill(typeCacheGrid, dirtyCell);
        }

        public override void MapGenerated()
        {
            base.MapGenerated();
            isDirty = true;
        }

        public override void MapRemoved()
        {
            base.MapRemoved();
            parts.Clear();
            ClearNetworks();
        }

        public void RegisterPart(CompBase part)
        {
            if (!parts.Contains(part))
            {
                parts.Add(part);
                NotifyChange(part);
            }
        }

        public void DeregisterPart(CompBase part)
        {
            if (parts.Contains(part))
            {
                parts.Remove(part);
                NotifyChange(part);
            }
        }

        public IEnumerable<CompBase> GetAllPartsAt(IntVec3 loc)
        {
            return map.thingGrid
                .ThingsListAtFast(loc)
                .OfType<ThingWithComps>()
                .SelectMany(t => t.AllComps)
                .OfType<CompBase>();
        }

        public IEnumerable<CompBase> GetPartsAt(IntVec3 loc, FlowType selector)
        {
            return selector switch
            {
                FlowType.None => Enumerable.Empty<CompBase>(),
                FlowType.Any => GetAllPartsAt(loc),
                _ => GetAllPartsAt(loc).Where(c => selector.Accept(c.FlowType))
            };
        }

        public bool HasPartAt(IntVec3 loc, FlowType filter = FlowType.Any)
        {
            return filter.Accept(GetCachedType(loc));
        }

        private FlowType GetCachedType(IntVec3 loc)
        {
            var index = map.cellIndices.CellToIndex(loc);
            var cellType = typeCacheGrid[index];
            if (cellType is dirtyCell)
            {
                cellType = FlowType.None;
                foreach (var part in GetAllPartsAt(loc))
                {
                    cellType |= part.FlowType;
                }
                typeCacheGrid[index] = cellType;
            }
            return cellType;
        }

        public void NotifyChange(CompBase part = null)
        {
            if (part is not null)
            {
                foreach (var loc in part.parent.OccupiedRect())
                {
                    typeCacheGrid[map.cellIndices.CellToIndex(loc)] = dirtyCell;
                }
            }
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
#if DEBUG
                Log.Message($"--- NetworkManager reconstructing networks on map #{map.uniqueID} ---");
#endif
                ReconstructNetworks();
                isDirty = false;
#if DEBUG
                Log.Message("--- NetworkManager reconstruction done ---");
#endif
            }
        }

        private void ReconstructNetworks()
        {
            ClearNetworks();

            var partList = parts.ListFullCopy();
            var queue = new Queue<CompBase>();

            while (partList.Count > 0)
            {
                var part = partList.OfType<CompBase>().FirstOrFallback(part => part.FlowType != FlowType.Any);
                if (part is null)
                {
                    break;
                }

                var flowType = part.FlowType;
                var network = NetworkPool.Acquire(flowType);
                networks.Add(network);

                queue.Enqueue(part);
                while (queue.TryDequeue(out var current))
                {
                    partList.Remove(current);
                    network.RegisterPart(current);

                    foreach (var loc in current.parent.OccupiedRect().AdjacentCellsCardinal)
                    {
                        if (HasPartAt(loc, flowType))
                        {
                            foreach (var candidate in GetPartsAt(loc, flowType))
                            {
                                if (!candidate.IsConnected)
                                {
                                    queue.Enqueue(candidate);
                                }
                            }
                        }
                    }
                }

            }
        }

        private void ClearNetworks()
        {
            networks.ForEach(NetworkPool.Release);
            networks.Clear();
        }
    }
}
