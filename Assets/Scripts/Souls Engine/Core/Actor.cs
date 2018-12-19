using System.Collections.Generic;
using UnityEngine;
using SoulsEngine;
using SoulsEngine.Core.Combat;
using MEC;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Collider2D))]
public class Actor : MonoBehaviour
{

    public int ID { get; set; }

    public AnimationManager AnimManager { get; set; }
    public CombatController CombatController { get; set; }

    public Controller Controller { get; set; }

    public bool Invincible { get; set; }
    public bool hasControl;
    public Vector2 input;

    public List<CoroutineHandle> coroutines;


    #region INITIALIZATION

    protected virtual void Awake()
    {
        Controller = GetComponent<Controller>();
        CombatController = GetComponent<CombatController>();
        AnimManager = GetComponent<AnimationManager>();

        coroutines = new List<CoroutineHandle>();
    }

    protected virtual void Start()
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
        GodManager.UnsubscribeActor(this);
        GodManager.UnregisterActor(this);
    }

    #endregion

    public virtual void Death(float delay)
    {
        AnimManager.SetState(SoulsEngine.Utility.Animation.ActorState.DYING);

        foreach (CoroutineHandle handle in coroutines)
        {
            Timing.KillCoroutines(handle);
        }

        Destroy(gameObject, delay);
    }

}