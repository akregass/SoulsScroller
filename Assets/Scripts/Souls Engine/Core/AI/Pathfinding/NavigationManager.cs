using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsEngine.Core.AI.Pathfinding
{
    class NavigationManager : MonoBehaviour
    {

        static NavigationManager instance;

        Pathfinder Pathfinder;

        Queue<Request> requests;
        Request currentRequest;
        bool isProcessingRequest;

        private void Awake()
        {
            instance = this;

            requests = new Queue<Request>();

            Pathfinder = GetComponent<Pathfinder>();
        }

        public static void RequestPath(Vector3 startPos, Vector3 targetPos, Action<Vector3[], bool> callback)
        {
            Request newRequest = new Request(startPos, targetPos, callback);
            instance.requests.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        void TryProcessNext()
        {
            if(!isProcessingRequest && requests.Count > 0)
            {
                currentRequest = requests.Dequeue();
                isProcessingRequest = true;
                Pathfinder.StartPathfinder(currentRequest.StartPos, currentRequest.TargetPos);
            }
        }

        public void OnRequestComplete(Vector3[] path, bool success)
        {
            currentRequest.Callback(path, success);
            isProcessingRequest = false;
            TryProcessNext();
        }

        struct Request
        {
            public Vector3 StartPos { get; set; }
            public Vector3 TargetPos { get; set; }
            public Action<Vector3[], bool> Callback { get; set; }

            public Request(Vector3 startPos, Vector3 targetPos, Action<Vector3[], bool> callback)
            {
                StartPos = startPos;
                TargetPos = targetPos;
                Callback = callback;
            }
        }
    }
}
