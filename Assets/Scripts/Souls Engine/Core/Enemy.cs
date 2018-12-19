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
        Vector2 dir = (GodManager.Player.transform.position - transform.position).normalized;
        
        if (!CombatController.combatInfo.InCombat)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + offset, dir, AggroRange, aggroMask);

            if (hit)
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    CombatController.EnterCombat();
                }
            }
        }
    }

    public virtual void FixedUpdate()
    {
        Controller.Move(input * Time.deltaTime);
    }
}