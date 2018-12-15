using UnityEngine;
using SoulsEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactionDistance;
    public Transform interactionTransform;
    Transform playerTransform;

    public string interactionText;

    public virtual void Start()
    {
        interactionTransform = GetComponent<Transform>();
        playerTransform = GodManager.Player.transform;
    }

    public virtual void Update ()
    {
        if(Vector2.Distance(playerTransform.position, interactionTransform.position) <= interactionDistance)
        {
            if (Input.GetAxisRaw("Interact") > 0)
            {
                Interact();
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + transform.name);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactionDistance);
    }
}
