using System;
using UnityEngine;
using SoulsEngine.Core.Combat;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;
using SoulsEngine.Utility.Animation;

public class CombatController : MonoBehaviour
{
    public Actor Actor { get; set; }

    [SerializeField]
    private Stats stats;
    public Stats Stats
    {
        get { return stats; }
        set { stats = value; }
    }

    [SerializeField]
    private Equipment _equipment;
    public Equipment Equipment
    {
        get { return _equipment; }
        set { _equipment = value; }
    }

    [SerializeField]
    private bool inCombat;
    public bool InCombat
    {
        get { return inCombat; }
        set
        {
            inCombat = value;

            if (OnCombatStateChange != null)
                OnCombatStateChange();
        }
    }

    [SerializeField]
    bool doneAttacking;
    public bool DoneAttacking { get { return doneAttacking; } set { doneAttacking = value; } }

    [SerializeField]
    bool doneTakingHit;
    public bool DoneTakingHit { get { return doneTakingHit; } set { doneTakingHit = value; } }

    [SerializeField]
    bool dying;
    public bool Dying { get { return dying; } set { dying = value; } }

    public event Action OnCombatStateChange;
    float combatTimeout = 3.2f;
    Timer CombatTimeoutTimer;

    private void Start()
    {
        Actor = GetComponent<Actor>();

        Equip(GetComponentInChildren<Weapon>(), 0);

        stats.Health = stats.BaseHealth;

        CombatTimeoutTimer = new Timer(combatTimeout);
        CombatTimeoutTimer.TimerEvent += ExitCombat;
    }

    private void LateUpdate()
    {
        if (DoneAttacking)
            DoneAttacking = false;

        if (DoneTakingHit)
            DoneTakingHit = true;
    }

    public void Attack()
    {
        if (!EnterCombat())
            CombatTimeoutTimer.Refresh();

        Actor.AnimManager.SetState(ActorState.ATTACKING);
    }

    public void TakeDamage(float damage)
    {
        if (!EnterCombat())
            CombatTimeoutTimer.Refresh();

        Actor.AnimManager.SetState(ActorState.HIT);

        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            Actor.AnimManager.SetState(ActorState.DYING);
        }
    }

    public void DamageOnHit(Actor target)
    {
        if (!EnterCombat())
            CombatTimeoutTimer.Refresh();

        var d = CalculateDamage();
        target.CombatController.TakeDamage(d);

        Actor.AnimManager.SetState(ActorState.IDLE);
    }

    public float CalculateDamage()
    {
        return Equipment.LeftH.Damage;
    }

    public void Equip(Weapon __item, int __slot)
    {
        switch (__slot)
        {
            case 0:
                Equipment = new Equipment(__item, Equipment.RightH);
                Equipment.LeftH.OnWeaponHit += DamageOnHit;
                break;

            case 1:
                Equipment = new Equipment(Equipment.LeftH, __item);
                Equipment.RightH.OnWeaponHit += DamageOnHit;
                break;
        }
    }

    bool EnterCombat()
    {
        if (!InCombat)
        {
            InCombat = true;
            var h = CombatTimeoutTimer.Activate();
            Actor.coroutines.Add(h);

            return true;
        }

        return false;
    }

    public void ExitCombat()
    {
        if (InCombat)
            InCombat = false;
    }

}
