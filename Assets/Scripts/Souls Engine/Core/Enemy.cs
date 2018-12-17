using UnityEngine;
using SoulsEngine;

public class Enemy : Actor
{
    [SerializeField]
    private float aggroRange;
    public float AggroRange
    {
        get { return aggroRange; }
        set { aggroRange = value; }
    }

    public LayerMask aggroMask;
    Vector3 offset;

    protected override void Awake()
    {
        base.Awake();

        offset = GetComponent<SpriteRenderer>().sprite.bounds.center;
    }

    public virtual void Update()
    {
        input = Vector2.zero;

        if (!CombatController.InCombat)
        {
            Vector2 dir = (GodManager.Player.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, dir, AggroRange, aggroMask);

            Debug.DrawRay(transform.position + offset, dir * AggroRange);

            if (hit)
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    CombatController.InCombat = true;
                    GodManager.Player.CombatController.InCombat = true;
                }
            }
        }
    }

    public virtual void FixedUpdate()
    {
        Controller.Move(input);
    }
}