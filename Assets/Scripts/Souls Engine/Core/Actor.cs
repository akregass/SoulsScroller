using System.Collections.Generic;
using UnityEngine;
using SoulsEngine;
using SoulsEngine.Utility.Animation;
using SoulsEngine.Utility.Combat;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Collider2D))]
public class Actor : MonoBehaviour
{
    public Vector2 input;

    private int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    protected GodManager godManager;

    [SerializeField]
    private AnimationManager _animManager;
    public AnimationManager AnimManager
    {
        get { return _animManager; }
        set { _animManager = value; }
    }

    private CombatController _combatController;
    public CombatController CombatController
    {
        get { return _combatController; }
        set { _combatController = value; }
    }
    
    [SerializeField]
    private Inventory _inventory;
    public Inventory Inventory
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    [SerializeField]
    private Stats _stats;
    public Stats Stats
    {
        get { return _stats; }
        set { _stats = value; }
    }

    [SerializeField]
    private List<StatusEffect> _currentStatusEffects = new List<StatusEffect>();
    public List<StatusEffect> CurrentStatusEffects
    {
        get { return _currentStatusEffects; }
        set { _currentStatusEffects = value; }
    }

    public ActorState ActorState { get; set; }

    public bool hasControl;

    [SerializeField]
    public bool Invincible { get; set; }

    protected Controller controller;

    #region INITIALIZATION

    protected void Awake () 
    {
        Inventory = GetComponent<Inventory>();
        Stats = _stats;
        CombatController = new CombatController(this);
        controller = GetComponent<Controller>();
        AnimManager = new AnimationManager
        {
            animator = GetComponent<Animator>(),
            animInt = new Dictionary<string, int>(),
            animFloat = new Dictionary<string, float>(),
            animBool = new Dictionary<string, bool>()
        };

        godManager = GameObject.FindGameObjectWithTag ("God Manager").GetComponent<GodManager>();
        godManager.actors.Add(this);
	}

	protected void Start()
    {
        SubscribeActor();
	}

    protected void OnEnable()
    {
        SubscribeActor();
    }

    protected void OnDisable()
    {
        UnsubscribeActor();
    }

    void SubscribeActor()
    {
        godManager.actorsActive.Add(this);
    }
    
    void UnsubscribeActor()
    {
        godManager.actorsActive.Remove(this);
    }

    #endregion

    public void SetActorState(ActorState s)
    {
        AnimManager.AnimatorState = (int)s;
        AnimManager.UpdateInt("state", AnimManager.AnimatorState);
        AnimManager.UpdateAnimator();
    }

}