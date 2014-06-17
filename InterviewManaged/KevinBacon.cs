using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class KevinBacon
    {
        public static void AssignBaconNumbers(Node bacon)
        {
            Queue<Node> nodesToVisit = new Queue<Node>();
            nodesToVisit.Enqueue(bacon);
            while (nodesToVisit.Count > 0)
            {
                Node current = nodesToVisit.Dequeue();

                // For each neighbor that has not yet been assigned a bacon number
                for (int i = 0; i < current.AdjacentNodes.Count; ++i)
                {
                    Node neighbor = current.AdjacentNodes[i];
                    if (neighbor.BaconNumber == 0)
                    {
                        // Assign its bacon number and queue it for a visit
                        neighbor.BaconNumber = current.BaconNumber + 1;
                        nodesToVisit.Enqueue(neighbor);
                    }
                }
            }
        }

        public struct Node
        {
            // Not good to have public setter, should probably use external data structure
            public int BaconNumber { get; set; }
            public List<Node> AdjacentNodes;
        }
    }
}
