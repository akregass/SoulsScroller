using System.Collections.Generic;
using UnityEngine;
using SoulsEngine;
using SoulsEngine.Utility.Combat;
using MEC;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Collider2D))]
public class Actor : MonoBehaviour
{

    public int ID { get; set; }

    public AnimationManager AnimManager { get; set; }
    public CombatController CombatController { get; set; }

    [SerializeField]
    private Stats _stats;
    public Stats Stats
    {
        get { return _stats; }
        set { _stats = Stats; }
    }

    protected Controller Controller { get; set; }

    public bool Invincible { get; set; }
    public bool hasControl;
    public Vector2 input;


    #region INITIALIZATION

    protected void Awake()
    {
        Stats = new Stats();
        CombatController = new CombatController(this);
        Controller = GetComponent<Controller>();
    }

    protected void Start()
    {
        GodManager.RegisterActor(this);
    }

    protected void OnEnable()
    {
        GodManager.SubscribeActor(this);
    }

    protected void OnDisable()
    {
        GodManager.UnsubscribeActor(this);
    }

    protected void OnDestroy()
    {
        GodManager.UnregisterActor(this);
    }

    #endregion

    public virtual void Death()
    {
        Timing.RunCoroutine(_Death());
    }

    public IEnumerator<float> _Death()
    {
        yield return Timing.WaitForSeconds(1f);
        Destroy(gameObject);
    }

}