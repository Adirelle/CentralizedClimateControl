using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralizedClimateControl
{
    static class NetworkPool
    {
        private static Queue<Network> pool = new Queue<Network>();

        private static int lastId;

        public static Network Acquire(FlowType flowType)
        {
            if (pool.TryDequeue(out var network))
            {
                network.FlowType = flowType;
                return network;
            }

            return new Network(lastId++, flowType);
        }

        public static void Release(Network network)
        {
            network.Clear();
            pool.Enqueue(network);
        }
    }
}
