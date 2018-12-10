using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{
    public float xMargin;
    public float yMargin;
    public float xSmooth;
    public float ySmooth;
    public Vector2 defaultMinPos;
    public Vector2 defaultMaxPos;
    public Vector2 minPos;
    public Vector2 maxPos;
    
    public CameraLockRegion currentRegion;
    public Transform followObject;


    public List<CameraLockRegion> cameraLockRegions;

    private void Update()
    {

        currentRegion = null;
        CameraLockRegion c;
        Rect r;
        for (int i = 0; i < cameraLockRegions.Count; i++)
        {
            c = cameraLockRegions[i];
            if (cameraLockRegions[i] == null)
            {
                cameraLockRegions[i] = gameObject.AddComponent<CameraLockRegion>();
            }
            else if(!c.disabled)
            {
                if (currentRegion != c)
                {
                    r = new Rect(c.regionPos, c.regionSize);

                    if (r.Contains(followObject.transform.position))
                    {
                        currentRegion = c;
                        break;
                    }
                }
            }
        }

        minPos = (currentRegion == null) ? defaultMinPos : currentRegion.minPos;
        maxPos = (currentRegion == null) ? defaultMaxPos : currentRegion.maxPos;
    }

    void FixedUpdate()
    {
        TrackObject();
    }

    bool CheckFollowMarginX() { return Mathf.Abs(transform.position.x - followObject.position.x) > xMargin; }

    bool CheckFollowMarginY() { return Mathf.Abs(transform.position.y - followObject.position.y) > yMargin; }

    bool CheckBoundsX(ref float target)
    {
        if((target - minPos.x) < -.25f)
        {
            target = Mathf.Lerp(target, minPos.x, xSmooth * Time.deltaTime);
            return true;
        }
        else if ((target - maxPos.x) > .25f)
        {
            target = Mathf.Lerp(target, maxPos.x, xSmooth * Time.deltaTime);
            return true;
        }

        return false;
    }

    bool CheckBoundsY(ref float target)
    {
        if ((target - minPos.y) < -1f)
        {
            target = Mathf.Lerp(target, minPos.y, ySmooth * Time.deltaTime);
            return true;
        }
        else if ((target - maxPos.y) > 1f)
        {
            target = Mathf.Lerp(target, maxPos.y, ySmooth * Time.deltaTime);
            return true;
        }

        return false;
    }

    void TrackObject()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (!CheckBoundsX(ref targetX))
        {
            if (CheckFollowMarginX())
            {
                targetX = Mathf.Lerp(transform.position.x, Mathf.Clamp(followObject.position.x, minPos.x, maxPos.x), xSmooth * Time.deltaTime);
                //targetX = Mathf.Clamp(targetX, minPos.x, maxPos.x);
            }
        }

        if(!CheckBoundsY(ref targetY))
        {
            if (CheckFollowMarginY())
            {
                targetY = Mathf.Lerp(transform.position.y, Mathf.Clamp(followObject.position.y, minPos.y, maxPos.y), ySmooth * Time.deltaTime);
                //targetY = Mathf.Clamp(targetY, minPos.y, maxPos.y);
            }
        }
        
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}