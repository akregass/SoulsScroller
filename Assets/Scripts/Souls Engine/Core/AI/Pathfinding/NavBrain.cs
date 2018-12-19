using System.Collections;
using UnityEngine;

namespace SoulsEngine.Core.AI.Pathfinding
{
    public class NavBrain : MonoBehaviour
    {
        Actor actor;
        Controller controller;

        Vector3[] path;
        int targetIndex;

        Vector3 targetPosition;


        private void Start()
        {
            actor = GetComponent<Actor>();
            controller = GetComponent<Controller>();

            targetPosition = GodManager.Player.transform.position;
            NavigationManager.RequestPath(transform.position, targetPosition, OnPathReceived);
        }

        public void OnPathReceived(Vector3[] _path, bool _success)
        {
            path = _path;
            if (_success)
            {
                path = _path;
                targetIndex = 0;

                //move code here
                print("success");
            }
        }

        public void UpdateTarget(Vector3 target)
        {
            targetPosition = target;
            NavigationManager.RequestPath(transform.position, targetPosition, OnPathReceived);
        }

        public void MoveToTarget()
        {
            Vector2 dir = (GodManager.Player.transform.position - transform.position).normalized;
            actor.input.x = Mathf.Sign(dir.x);
        }

        public void OnDrawGizmos()
        {
            if(path != null)
            {
                Debug.Log("not null");
                for(int i = 0; i < path.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                        Gizmos.DrawLine(transform.position, path[i]);
                    else
                        Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
