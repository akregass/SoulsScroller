using SoulsEngine.Utility.Combat;
using SoulsEngine.Utility.Animation;

public class CombatController
{

    public Actor Actor { get; set; }
    public Stats Stats { get; set; }

    private Equipment _equipment;
    public Equipment Equipment
    {
        get { return _equipment; }
        set { _equipment = value; }
    }
    
    private int _attackChainCount;
    public int AttackChainCount
    {
        get { return _attackChainCount; }
        set
        {
            _attackChainCount = value;
            switch (_attackChainCount)
            {
                case 1:

                    //damage modifier code goes here

                    break;

                case 2:

                    //damage modifier code goes here

                    break;

                case 3:

                    //damage modifier code goes here

                    _attackChainCount = 0;
                    Actor.ActorState = ActorState.IDLE;

                    break;

                default:
                    break;
            }
        }
    }

    private bool _shieldRaised;
    public bool ShieldRaised
    {
        get { return _shieldRaised; }
        set { _shieldRaised = value; }
    }

    public CombatController(Actor a)
    {
        Actor = a;
    }

    public void Attack()
    {
        if (Actor.ActorState == ActorState.ATTACKING)
        {
            Actor.ActorState = ActorState.ATTACKING;
            AttackChainCount++;
        }
    }

    public void Parry()
    {

    }

    float CalculateDamage()
    {
        return 0f;
    }

    public void Equip(Item __item, int __slot)
    {
        switch (__slot)
        {
            case 0:
                Equipment = new Equipment(__item, Equipment.RightH, Equipment.Armor);
                break;

            case 1:
                Equipment = new Equipment(Equipment.LeftH, __item, Equipment.Armor);
                break;

            case 2:
                Equipment = new Equipment(Equipment.LeftH, Equipment.RightH, __item);
                break;
        }
    }

}
