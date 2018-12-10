using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility.Locomotion;
using SoulsEngine.Utility;
using MEC;

public class Controller : RaycastController
{
    Actor actor;

    [Header("Collision"), Space(5f)]
    public LayerMask collisionMask;
    [SerializeField]
    public CollisionInfo collisions;

    [Space(10f), Header("Movement values"), Space(5f)]
    public Vector3 velocity;
    Vector3 velocityLastFrame;
    float velocityXSmoothing;
    float accelerationTimeGrounded = .1f;
    float accelerationTimeAirborne = .05f;
    
    [Space(10f), Header("Jumping"), Space(5f), SerializeField]
    float jumpHeight;
    [SerializeField]
    float timeToJumpApex;
    float gravity;
    float jumpVelocity;
    bool jumpGraceAble = true;
    public bool jumpGraceActive = false;
    [SerializeField]
    float jumpGracePeriod;
    Timer jumpGraceTimer;
    [SerializeField]
    float jumpGraceCooldown;
    Timer jumpGraceCooldownTimer;
    public bool isJumping;
    public bool isFallingFromJump;
    public bool isFalling;
    
    public bool isWallSliding;
    public float wallSlideSpeed;
    public Vector2 wallJumpSpeed;
    public Vector2 wallClimbSpeed;
    public Vector2 wallJumpOffSpeed;

    float wallStickTime = .25f;
    public float timeToWallUnstick;

    [Space(10f), Header("Dashing"), Space(5f), SerializeField]
    float dashDistance;
    [SerializeField]
    float dashTime;
    float dashDistanceCovered;
    [SerializeField]
    float dashCooldown;
    Timer dashCooldownTimer;
    bool canDash = true;
    public bool isDashing;

    [Space(10f), Header("Misc"), Space(5f)]
    public int direction = 1;

    public float LocomotionModifier { get; set; }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        actor = GetComponent<Actor>();
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
        if (Mathf.Sign(gameObject.transform.localScale.x) != direction)
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        
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

    public void Move(Vector3 input)
    {
        float targetVelocityX = input.x * actor.Stats.MoveSpeed;
        int wallDirection = collisions.right ? 1 : -1;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if ((collisions.left || collisions.right) && !collisions.below)
        {
            isWallSliding = true;
            if (velocity.y < 0)
            {

                //isWallSliding = true;
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

        if(Mathf.Abs(velocity.x) > .01f)
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
        
        transform.Translate(velocity);
    }

    public void Jump(Vector2 input)
    {
        int wallDirection = collisions.right ? 1 : -1;
        
        if (isWallSliding)
        {
            if(input.x != 0)
            {
                velocity.x = input.x == wallDirection ? (wallClimbSpeed.x * -wallDirection) : (wallJumpSpeed.x * -wallDirection);
                velocity.y = input.x == wallDirection ? wallClimbSpeed.y : wallJumpSpeed.y;
            }
            else
            {
                velocity.x = wallJumpOffSpeed.x * -wallDirection;
                velocity.y = wallJumpOffSpeed.y;
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

    IEnumerator<float> SmoothMove()
    {
        yield return 0f;
    }
    
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
