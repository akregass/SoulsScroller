using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace SoulsEngine.Utility.Locomotion
{
    public class RaycastController : MonoBehaviour
    {

        public BoxCollider2D boxCollider;
        public float skinWidth;

        public RaycastOrigins raycastOrigins;
        public float raySpacingX;
        public float raySpacingY;
        public int rayCountX;
        public int rayCountY;

        public void UpdateRaycastOrigins()
        {
            Bounds bounds = boxCollider.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        public void CalculateRaySpacing()
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

        static List<CoroutineHandle> handles = new List<CoroutineHandle>();

        public static void SmoothMovement(Vector3 transform, Vector3 velocity)
        {
            var h = Timing.RunCoroutine(Move(velocity));
            handles.Add(h);
        }

        static IEnumerator<float> Move(Vector3 velocity)
        {
            yield return 0f;
        }

    }
}