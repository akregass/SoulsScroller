using System;
using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility;
using MEC;

namespace SoulsEngine.Core.AI.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {

        NavGrid grid;
        NavigationManager navManager;

        private void Awake()
        {
            grid = GetComponent<NavGrid>();
            navManager = GetComponent<NavigationManager>();
        }

        public CoroutineHandle StartPathfinder(Vector3 start, Vector3 goal)
        {
            var h = Timing.RunCoroutine(FindPath(start, goal));
            return h;
        }

        IEnumerator<float> FindPath(Vector3 start, Vector3 target)
        {
            Vector3[] waypoints = new Vector3[0];
            bool success = false;

            NavNode startNode = grid.NodeFromWorldPos(start);
            NavNode targetNode = grid.NodeFromWorldPos(target);

            if(startNode.walkable && targetNode.walkable)
            {

                Heap<NavNode> openSet = new Heap<NavNode>(grid.Size);
                HashSet<NavNode> closedSet = new HashSet<NavNode>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    NavNode currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if(currentNode == targetNode)
                    {
                        success = true;
                        break;
                    }

                    foreach(NavNode neighbor in grid.FindNeighbors(currentNode))
                    {
                        if(neighbor.walkable && !closedSet.Contains(neighbor))
                        {
                            int costToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                            if(costToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                            {
                                neighbor.gCost = costToNeighbor;
                                neighbor.hCost = GetDistance(neighbor, targetNode);
                                neighbor.parent = currentNode;

                                if (!openSet.Contains(neighbor))
                                    openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }

            yield return Timing.WaitForOneFrame;

            if (success)
                waypoints = RetracePath(startNode, targetNode);

            navManager.OnRequestComplete(waypoints, success);

        }

        Vector3[] RetracePath(NavNode startNode, NavNode targetNode)
        {
            List<NavNode> path = new List<NavNode>();
            NavNode currentNode = targetNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            return waypoints;
        }

        Vector3[] SimplifyPath(List<NavNode> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 direction = Vector2.zero;

            for(int i = 0; i < path.Count; i++)
            {
                Vector2 dir = new Vector2(path[i - 1].gridPos.x - path[i].gridPos.x, path[i - 1].gridPos.y - path[i].gridPos.y);

                if(dir != direction)
                    waypoints.Add(grid.WorldPosFromNode(path[i]));

                direction = dir;
            }

            Vector3[] final = new Vector3[waypoints.Count];

            for (int i = 0; i < waypoints.Count; i++)
                final[i] = waypoints[i];

            return final;
        }

        int GetDistance(NavNode nodeA, NavNode nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.gridPos.x - nodeB.gridPos.x);
            int distanceY = Mathf.Abs(nodeA.gridPos.y - nodeB.gridPos.y);

            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);

            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

    }
}
