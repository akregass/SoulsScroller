using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Locomotion;
using SoulsEngine.Utility.Animation;

public class Controller : RaycastController
{
    Actor actor;
    SpriteRenderer sprite;

    [Header("Collision"), Space(5f)]
    public LayerMask collisionMask;
    CollisionInfo collisions;

    [Space(10f), Header("Movement values"), Space(5f), HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;
    float accelerationTimeGrounded = .1f;
    float accelerationTimeAirborne = .05f;

    #region JUMPING VARS
    [Space(10f), Header("Jumping"), Space(5f), SerializeField]
    float jumpHeight;
    [SerializeField]
    float timeToJumpApex;
    float gravity;
    float jumpVelocity;
    bool jumpGraceAble = true;
    bool jumpGraceActive = false;
    [SerializeField]
    float jumpGracePeriod;
    Timer jumpGraceTimer;
    [SerializeField]
    float jumpGraceCooldown;
    Timer jumpGraceCooldownTimer;
    bool isJumping;
    bool isFallingFromJump;
    bool isFalling = false;
    
    bool isWallSliding;
    public float wallSlideSpeed;
    public Vector2 wallJumpSpeed;
    public Vector2 wallClimbSpeed;
    public Vector2 wallJumpOffSpeed;
    bool isWallJumping;

    float wallStickTime = .25f;
    float timeToWallUnstick;

    #endregion

    #region DASHING VARS
    [Space(10f), Header("Dashing"), Space(5f), SerializeField]
    float dashDistance;
    [SerializeField]
    float dashTime;
    float dashDistanceCovered;
    [SerializeField]
    float dashCooldown;
    Timer dashCooldownTimer;
    bool canDash = true;
    bool isDashing;

    #endregion

    [Space(10f), Header("Misc"), Space(5f)]
    public int direction = 1;
    int faceDirection = 1;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        actor = GetComponent<Actor>();
        sprite = GetComponent<SpriteRenderer>();
        CalculateRaySpacing();

        #region TIMERS

        dashCooldownTimer = new Timer(dashCooldown);
        dashCooldownTimer.TimerEvent += EnableDash;

        jumpGraceTimer = new Timer(jumpGracePeriod);
        jumpGraceTimer.TimerEvent += DeactivateJumpGrace;
        jumpGraceCooldownTimer = new Timer(jumpGraceCooldown);
        jumpGraceCooldownTimer.TimerEvent += EnableJumpGrace;
        
        #endregion

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void Update()
    {
        FaceDirection(direction);

        sprite.flipX = (!isWallSliding && !isWallJumping);

        if (collisions.below)
        {
            if (isFalling)
                isFalling = false;

            if (isFallingFromJump)
                isFallingFromJump = false;

            if (!jumpGraceAble)
            {
                EnableJumpGrace();
                jumpGraceCooldownTimer.Deactivate();
            }

            if (jumpGraceActive)
            {
                DeactivateJumpGrace();
                jumpGraceTimer.Deactivate();
            }

            if (isWallSliding)
            {
                isWallSliding = false;
                timeToWallUnstick = 0;
            }
        }
    }

    public void Move(Vector2 input)
    {
        float targetVelocityX = input.x * actor.CombatController.Stats.MoveSpeed;
        int wallDirection = collisions.right ? 1 : -1;

        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if(!isWallJumping)
            velocity.x = targetVelocityX;

        if ((collisions.left || collisions.right) && !collisions.below)
        {
            isWallSliding = true;

            if (velocity.y < 0)
            {
                isJumping = false;
                isFalling = false;
                isFallingFromJump = false;

                if (velocity.y < wallSlideSpeed)
                    velocity.y = wallSlideSpeed;

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (input.x == -wallDirection)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

                if (jumpGraceActive)
                {
                    DeactivateJumpGrace();
                    jumpGraceTimer.Deactivate();
                }


            }
        }

        if (!collisions.left && !collisions.right && isWallSliding)
            isWallSliding = false;
        
        if (velocity.y < 0 && !isWallSliding)
        {
            if (isJumping)
            {
                isJumping = false;
                isFallingFromJump = true;
            }

            if (!isFallingFromJump)
                isFalling = true;
        }

        if (isDashing)
        {
            velocity.y = 0;
            velocity.x = (dashDistance / dashTime) * direction;
            dashDistanceCovered += velocity.x;

            if (Mathf.Abs(dashDistanceCovered) > dashDistance)
            {
                isDashing = false;
                dashDistanceCovered = 0;
            }
        }
        
        if (Mathf.Abs(velocity.x) > .01f)
            direction = (int)Mathf.Sign(velocity.x);

        if (velocity.y < -.1f && jumpGraceAble && !jumpGraceActive && !isFallingFromJump)
        {
            if (!isWallSliding)
            {
                ActivateJumpGrace();
                jumpGraceTimer.Activate();

                DisableJumpGrace();
                jumpGraceCooldownTimer.Activate();
            }
        }
        
        velocity.y += gravity * Time.deltaTime;
        
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0 || isWallSliding)
            HorizontalCollision(ref velocity);

        if (velocity.y != 0)
            VerticalCollision(ref velocity);

        UpdateAnimator();

        transform.Translate(velocity);

        if (isWallJumping)
            isWallJumping = false;
    }

    public void Jump(Vector2 input)
    {
        int wallDirection = collisions.right ? 1 : -1;
        
        if (isWallSliding)
        {
            if(input.x != 0)
            {
                velocity.y = input.x == wallDirection ? wallClimbSpeed.y : wallJumpSpeed.y;
                velocity.x = input.x == wallDirection ? (wallClimbSpeed.x * -wallDirection) : (wallJumpSpeed.x * -wallDirection);
                isWallJumping = true;
            }
            else
            {
                velocity.x = wallJumpOffSpeed.x * -wallDirection;
                velocity.y = wallJumpOffSpeed.y;
                isWallJumping = true;
            }
        }
        else if ((collisions.below || jumpGraceActive) && !isDashing)
        {
            velocity.y = jumpVelocity;
            isJumping = true;

            if (jumpGraceActive)
            {
                DeactivateJumpGrace();
                jumpGraceTimer.Deactivate();
            }
        }
    }

    public void EnableJumpGrace() { jumpGraceAble = true; }

    public void DisableJumpGrace() { jumpGraceAble = false; }

    public void ActivateJumpGrace () { jumpGraceActive = true; }

    public void DeactivateJumpGrace() { jumpGraceActive = false; }

    public void Dash()
    {
        if (canDash)
        {
            isDashing = true;
            DisableDash();
            dashCooldownTimer.Activate();
        }
    }

    public void EnableDash() { canDash = true; }

    public void DisableDash() { canDash = false; }

    void HorizontalCollision(ref Vector3 velocity)
    {
        int directionX = direction;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(Mathf.Abs(velocity.x) < skinWidth)
            rayLength = skinWidth * 8f;

        for (int i = 0; i < rayCountX; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raySpacingX * i);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = (directionX == -1);
                collisions.right = (directionX == 1);
            }
        }
    }

    void VerticalCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < rayCountY; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raySpacingY * i);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = (directionY == -1);
                collisions.above = (directionY == 1);
            }
        }
    }

    void UpdateAnimator()
    {
        actor.AnimManager.SetState(ActorState.IDLE);

        if (Mathf.Abs(velocity.x) > .05)
            actor.AnimManager.SetState(ActorState.RUNNING);

        if (!collisions.below)
            actor.AnimManager.SetState(ActorState.JUMPING);
    }

    public void FaceDirection(int dir)
    {
        if (Mathf.Sign(transform.localScale.x) != dir)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
    
    [System.Serializable]
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

}
