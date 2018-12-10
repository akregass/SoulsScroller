using UnityEngine;

namespace SoulsEngine.Utility
{
    [ExecuteInEditMode]
    public class CameraLockRegion : MonoBehaviour
    {
        public Vector2 minPos;
        public Vector2 maxPos;
        public Vector2 regionPos;
        public Vector2 regionSize;
        
        public bool showTriggerBounds;
        public bool showRegion;
        public bool disabled;

        Vector2 regionBoundTopLeft;
        Vector2 regionBoundsTopRight;
        Vector2 regionBoundsBottomLeft;
        Vector2 regionBoundsBottomRight;

        private void Update()
        {
            if (showTriggerBounds)
            {
                Debug.DrawLine(new Vector2(regionPos.x, regionPos.y), new Vector2(regionPos.x + regionSize.x, regionPos.y), Color.magenta);
                Debug.DrawLine(new Vector2(regionPos.x + regionSize.x, regionPos.y), new Vector2(regionPos.x + regionSize.x, regionPos.y + regionSize.y), Color.magenta);
                Debug.DrawLine(new Vector2(regionPos.x + regionSize.x, regionPos.y + regionSize.y), new Vector2(regionPos.x, regionPos.y + regionSize.y), Color.magenta);
                Debug.DrawLine(new Vector2(regionPos.x, regionPos.y + regionSize.y), new Vector2(regionPos.x, regionPos.y), Color.magenta);
            }

            regionBoundsBottomLeft = new Vector2(minPos.x - CameraUtility.ScreenWidthInUnits / 2, minPos.y - CameraUtility.ScreenHeightInUnits / 2);
            regionBoundsBottomRight = new Vector2(maxPos.x + CameraUtility.ScreenWidthInUnits / 2, minPos.y - CameraUtility.ScreenHeightInUnits / 2);
            regionBoundsTopRight = new Vector2(maxPos.x + CameraUtility.ScreenWidthInUnits / 2, maxPos.y + CameraUtility.ScreenHeightInUnits / 2);
            regionBoundTopLeft = new Vector2(minPos.x - CameraUtility.ScreenWidthInUnits / 2, maxPos.y + CameraUtility.ScreenHeightInUnits / 2);

            if (showRegion)
            {
                Debug.DrawLine(regionBoundsBottomLeft, regionBoundsBottomRight, Color.green);
                Debug.DrawLine(regionBoundsBottomRight, regionBoundsTopRight, Color.green);
                Debug.DrawLine(regionBoundsTopRight, regionBoundTopLeft, Color.green);
                Debug.DrawLine(regionBoundTopLeft, regionBoundsBottomLeft, Color.green);
            }
        }
    }

    public class CameraUtility
    {
        public static float ScreenHeightInUnits
        {
            get
            {
                return Camera.main.orthographicSize * 2;
            }
        }
        
        public static float ScreenWidthInUnits
        {
            get
            {
                return ScreenHeightInUnits * Camera.main.aspect;
            }
        }
    }
}