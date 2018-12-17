using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace SoulsEngine.Utility.Locomotion
{
    public class RaycastController : MonoBehaviour
    {

        protected BoxCollider2D boxCollider;
        public  float skinWidth;

        protected RaycastOrigins raycastOrigins;
        protected float raySpacingX;
        protected float raySpacingY;
        public int rayCountX;
        public int rayCountY;

        protected void UpdateRaycastOrigins()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        protected void CalculateRaySpacing()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            rayCountX = Mathf.Clamp(rayCountX, 2, int.MaxValue);
            rayCountY = Mathf.Clamp(rayCountY, 2, int.MaxValue);

            raySpacingX = bounds.size.y / (rayCountX - 1);
            raySpacingY = bounds.size.x / (rayCountY - 1);

        }

        public struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }

    public class LocomotionUtility
    {

        public static void MoveSmooth(Transform body, Vector3 velocity, float time)
        {
            Timing.RunCoroutine(_MoveSmooth(body, velocity, time), body.gameObject.name);
        }

        static IEnumerator<float> _MoveSmooth(Transform body, Vector3 velocity, float time)
        {
            Vector3 distanceTotal = velocity; // 10
            Vector3 distanceCovered = new Vector3(0,0);
            float finalTime = time / Time.fixedDeltaTime; // 500 = 5 / .01 total frames
            Vector3 finalVelocity = distanceTotal / finalTime; // .1 = 10 / 500 total distance covered every frame

            while(distanceCovered.x < distanceTotal.x || distanceCovered.y < distanceTotal.y)
            {
                body.position += finalVelocity;
                yield return Timing.WaitForSeconds(Time.fixedDeltaTime);
            }

            Timing.KillCoroutines(body.gameObject.name);
        }

    }
}