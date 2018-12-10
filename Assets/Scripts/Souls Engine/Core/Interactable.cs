using UnityEngine;
using SoulsEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactionDistance;
    public Transform interactionTransform;
    private Transform playerTransform;

    public string interactionText;

    private void Start()
    {
        interactionTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("God Manager").GetComponent<GodManager>().Player.transform;
    }

    void Update ()
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
