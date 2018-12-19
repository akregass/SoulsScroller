using UnityEngine;
using SoulsEngine.Utility;

namespace SoulsEngine.Core.AI.Pathfinding
{
    public class NavNode : IHeapItem<NavNode>
    {
        public Vector3Int gridPos;
        public bool walkable;
        public bool wall;

        public int gCost;
        public int hCost;
        public NavNode parent;
        
        public int Index { get; set; }

        public int fCost { get { return gCost + hCost; } }
        
        public NavNode(Vector3Int _gridPos, bool _walkable, bool _wall)
        {
            gridPos = _gridPos;
            walkable = _walkable;
            wall = _wall;
        }

        public int CompareTo(NavNode node)
        {
            var v = fCost.CompareTo(node.fCost);
            if (v == 0)
                v = gCost.CompareTo(node.gCost);

            return -v;
        }

    }
}
