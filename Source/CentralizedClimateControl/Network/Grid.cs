using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CentralizedClimateControl
{
    internal class Grid
    {
        public readonly CompNetworkPart[] Cells;

        public IEnumerable<CompNetworkPart> Parts => Cells.Where(cell => cell != null);

        private readonly CellIndices cellIndices;

        public Grid(Map map)
        {
            cellIndices = map.cellIndices;
            Cells = new CompNetworkPart[cellIndices.NumGridCells];
        }

        public void Clear()
        {
            Array.Fill(Cells, null);
        }

        public void Add(CompNetworkPart part)
        {
            foreach (var cell in part.parent.OccupiedRect())
            {
                Cells[cellIndices.CellToIndex(cell)] = part;
            }
        }
        public void Remove(CompNetworkPart part)
        {
            foreach (var cell in part.parent.OccupiedRect())
            {
                var index = cellIndices.CellToIndex(cell);
                if (Cells[index] == part)
                {
                    Cells[index] = null;
                }
            }
        }

        public CompNetworkPart GetAt(IntVec3 cell)
        {
            return Cells[cellIndices.CellToIndex(cell)];
        }

        public CompNetworkPart FindAt(IntVec3 cell, FlowType select = FlowType.Any)
        {
            var part = GetAt(cell);
            return (part != null && select.Accept(part.FlowType)) ? part : null;
        }
    }
}
