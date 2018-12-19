using System;
using UnityEngine;
using SoulsEngine.Core.AI.Pathfinding;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;
using SoulsEngine.Utility.Animation;

namespace SoulsEngine.Core.Combat
{

    public class CombatController : MonoBehaviour
    {
        public Actor Actor { get; set; }

        public NavBrain NavBrain { get; set; }

        [SerializeField]
        private Stats stats;
        public Stats Stats
        {
            get { return stats; }
            set { stats = value; }
        }

        [SerializeField]
        private Equipment equipment;
        public Equipment Equipment
        {
            get { return equipment; }
            set { equipment = value; }
        }

        [SerializeField]
        private bool inRange;
        public bool InRange
        {
            get { return inRange; }
            set { inRange = value; }
        }
        
        float combatTimeout = 3.2f;
        Timer CombatTimeoutTimer;

        Vector3 playerPosition;

        [SerializeField]
        public CombatInfo combatInfo;

        private void Start()
        {
            Actor = GetComponent<Actor>();

            combatInfo = new CombatInfo();

            Equip(GetComponentInChildren<Weapon>(), 0);

            stats.Health = stats.BaseHealth;

            CombatTimeoutTimer = new Timer(combatTimeout);
            CombatTimeoutTimer.TimerEvent += ExitCombat;

            if (Actor.tag != "Player")
            {
                NavBrain = GetComponent<NavBrain>();
            }
        }

        private void Update()
        {
            if (Actor.tag != "Player")
            {
                if (combatInfo.InCombat)
                {
                    playerPosition = GodManager.Player.transform.position;

                    InRange = Vector2.Distance(new Vector2(playerPosition.x, playerPosition.y), new Vector2(transform.position.x, transform.position.y)) < Equipment.LeftH.Range;

                    if (!InRange && !combatInfo.Attacking)
                    {
                        playerPosition = GodManager.Player.transform.position;
                        NavBrain.UpdateTarget(playerPosition);
                        NavBrain.MoveToTarget();
                    }

                    if(inRange && !combatInfo.Attacking)
                    {
                        FacePlayer();
                        Attack();
                    }
                }
            }

            if (combatInfo.Attacking)
                Actor.Controller.velocity.x = 0;
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

            stats.Health -= damage;
            if (stats.Health <= 0f)
                Actor.Death(3.5f);
            else
                Actor.AnimManager.SetState(ActorState.HIT);

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

        public bool EnterCombat()
        {
            if (!combatInfo.InCombat)
            {
                combatInfo.InCombat = true;
                var h = CombatTimeoutTimer.Activate();
                Actor.coroutines.Add(h);

                return true;
            }

            return false;
        }

        public void ExitCombat()
        {
            if (combatInfo.InCombat)
                combatInfo.InCombat = false;
        }

        public void FacePlayer()
        {
            int dir = Mathf.RoundToInt(Mathf.Sign(playerPosition.x - transform.position.x));

            if (Actor.Controller.direction != dir)
                Actor.Controller.FaceDirection(dir);
        }

        [Serializable]
        public struct CombatInfo
        {
            public event Action OnCombatInfoChange;

            [SerializeField]
            bool _inCombat;
            public bool InCombat
            {
                get { return _inCombat; }
                set
                {
                    _inCombat = value;

                    if (OnCombatInfoChange != null)
                        OnCombatInfoChange();
                }
            }

            [SerializeField]
            bool _attacking;
            public bool Attacking
            {
                get { return _attacking; }
                set
                {
                    _attacking = value;

                    if (OnCombatInfoChange != null)
                        OnCombatInfoChange();
                }
            }

            [SerializeField]
            bool _takingHit;
            public bool TakingHit
            {
                get { return _takingHit; }
                set
                {
                    _takingHit = value;

                    if (OnCombatInfoChange != null)
                        OnCombatInfoChange();
                }
            }

            [SerializeField]
            bool _dying;
            public bool Dying
            {
                get { return _dying; }
                set
                {
                    _dying = value;
                }
            }

        }

    }


}