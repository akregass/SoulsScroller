using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SoulsEngine.Core.AI.Pathfinding
{
    [ExecuteInEditMode]
    class NavGrid : MonoBehaviour
    {
        Grid tileGrid;
        Tilemap tilemap;

        NavNode[,] grid;

        Vector3Int Offset { get; set; }

        public bool DrawGrid;

        public int Size { get { return tilemap.size.x * tilemap.size.y; } }

        private void OnEnable()
        {
            tileGrid = GetComponent<Grid>();
            tilemap = GetComponentInChildren<Tilemap>();

            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new NavNode[tilemap.size.x, tilemap.size.y];

            Offset = new Vector3Int(-tilemap.cellBounds.xMin, -tilemap.cellBounds.yMin, -tilemap.cellBounds.zMin);

            foreach(var position in tilemap.cellBounds.allPositionsWithin)
            {
                bool walkable = false;
                bool wall = false;

                Vector3Int below = position;
                Vector3Int left = position;
                Vector3Int right = position;

                below.y -= 1;
                left.x += 1;
                right.x -= 1;

                if (!tilemap.HasTile(position))
                {
                    if (tilemap.HasTile(below))
                        walkable = true;
                }
                else if (!tilemap.HasTile(left) || !tilemap.HasTile(right))
                    wall = true;

                grid[Offset.x + position.x, Offset.y + position.y] = new NavNode(position, walkable, wall);
            }
        }

        public HashSet<NavNode> FindNeighbors(NavNode node)
        {
            HashSet<NavNode> neighbors = new HashSet<NavNode>();

            for(int i = -1; i < 1; i++)
            {
                for(int j = 0; j < 1; j++)
                {
                    if(i!= 0 && j != 0)
                    {
                        int x = node.gridPos.x + i;
                        int y = node.gridPos.y + j;

                        if (x >= 0 && x < tilemap.size.x && y >= 0 && y < tilemap.size.y)
                            neighbors.Add(grid[x, y]);
                    }
                }
            }

            return neighbors;
        }

        public NavNode NodeFromWorldPos(Vector3 pos)
        {
            var p = tileGrid.WorldToCell(pos);
            return grid[p.x + Offset.x, p.y + Offset.y];
        }

        public Vector3 WorldPosFromNode(NavNode node)
        {
            return tileGrid.CellToWorld(node.gridPos) + tilemap.cellSize / 2;
        }

        public void OnDrawGizmos()
        {
            if (DrawGrid)
            {
                var c = 0;
                foreach (var node in grid)
                {
                    Gizmos.color = node.walkable ? Color.green : Color.red;
                    Gizmos.color = node.wall ? Color.blue : Gizmos.color;
                    var p = WorldPosFromNode(node);
                    Gizmos.DrawCube(p, tilemap.cellSize);
                    c++;
                }
                print(c);
            }
        }
    }
}
