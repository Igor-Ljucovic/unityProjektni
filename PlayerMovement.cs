using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerSpells playerSpells;
    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private PlayerAnimations playerSprites;
    [SerializeField] private AudioManager audioManager;


    //      GENERAL:


    [SerializeField] private bool[] groundedHistory = new bool[3];

    private bool[] dashInputHistory = new bool[9];

    [SerializeField] private bool[] dodgeInputHistory = new bool[15];

    private bool[] rollInputHistory = new bool[12];
    private int numberOfFramesOfAllowedRollInputDelay = 12;   // needs to be the same or lower than fastFallStunDuration, so that they don't overlap

    private bool[] fastFallHistory = new bool[9];

    private bool[] maxFallSpeedHistory = new bool[9];

    private bool[] jumpingHistory = new bool[5];


    //      MOVEMENT STATUS:


    [SerializeField] private int currentPlayerStunDuration = 0;

    public  bool isFastFalling = false;
    private bool isFacingRight = true;
    public  bool isDashing = false;
    private bool isWallJumping = false;
    public  bool isDodging = false;
    public  bool isRolling = false;
    public  bool isShadowDashing = false;
    public  bool isWallSliding = false;
    public  bool isRunning = false;
    public  bool isIdling = false;
    public  bool isJumping = false;
    public  bool isFalling = false;
    public  bool isHardFallStunned = false;
    public  bool isLookingUp = false;
    public  bool isLookingDown = false;


    //      HELPER-VARIABLES:


    private bool dashReady = false;
    public  bool doubleJumpReady = true;
    private bool dashedIntoWall = false;
    private bool dashedFromWall = false;
    private bool lastFrameRollingInTheAir = false;
    public  bool jumpingLastFrame = false;
    private float lastRollingFrameSpeed;


    //      MOVEMENT NUMBERS:


    private   int horizontalInput = 0;
    private float normalGravity = 3.5f;
    
    public  float speed = 7f;
    private float dashSpeed = 16.5f;
    private float maxFallSpeed = -37f;
    private float fastFallSpeed = -25f;
    private float wallSlidingSpeed = -2.25f;
    private float jumpingPower = 14.5f;

    private   int currentDodgeFrame = 0;
    private   int totalDodgeFrames = 30;
    private   int dodgeStunDuration = 30;

    public  float dashStartTime = 0f;
    public  float dashDuration = 0.25f;

    private float rollStartAndEndSpeed = 4f;
    private float rollAfterStartAndBeforeEndSpeed = 12.5f;
    private float rollMiddleSpeed = 25f;
    private   int changedRollFrameAfterFallingOffOfCliffWhenTouchingGround = 21;
    private float rollMaxSpeedInTheAir = 7.5f;

    private float knockbackSpeed = 10f;
    

    //      MOVEMENT TIMERS:
    

    public  int currentKnockbackDuration = 0;

    private int totalStunDurationAfterKnockBackToTheSide = 6;

    private int stunDurationAfterHardFall = 60;
    private int stunDurationAfterFastFall = 12;

    private int totalWallJumpCantFlip = 4;
    private int currentWallJumpCantFlip = 0;

    private int currentStunDurationAfterDashIsPerformedInFrames = 0;
    private int totalStunDurationAfterDashIsPerformedInFrames = 6;

    private int totalDashFrames = 21;

    private int currentDashCooldown = 0;
    private int totalDashCooldown = 36;

    public  int currentShadowDashCooldown = 0;
    private int totalShadowDashCooldown = 90;

    private int currentDodgeCooldown = 0;
    private int totalDodgeCooldown = 75;

    private int currentRollCooldown = 0;
    private int totalRollCooldown = 60;
    private int currentRollFrame = 0;
    private int totalRollFrames = 36;

    private int totalDashStunDurationInFrames;


    //      SETUP:


    private static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMovement>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("PlayerMovement");
                    instance = managerObject.AddComponent<PlayerMovement>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }

    void Start()
    {
        totalDashStunDurationInFrames = totalDashFrames + totalStunDurationAfterDashIsPerformedInFrames;
    }

    //float trajanje = 70;
    //bool uradi = true;

    //      CODE:


    void Update()
    {
        /*if (uradi)
        {
            Debug.Log(trajanje);
            StartCoroutine(Game.CountdownUntilZero(trajanje));
            Debug.Log(trajanje);
            uradi = false;
        }
        Debug.Log(trajanje);*/

        FillInputHistoryLists();

        if (Game.Paused() || !Game.mainGameStarted)
            return;

        MovementSoundCheck();

        CooldownCheck();

        if (isDodging)
            currentDodgeFrame++;   //dodging stuns the player, so this line of code has to be before the stun line of code instead of in "MovementCheck()"
        
        if (currentKnockbackDuration > 0)
        {
            KnockbackThePlayer();
            currentKnockbackDuration--;
            return;
        }

        if (currentPlayerStunDuration > 0)
        {
            rb.Stun();
            currentPlayerStunDuration--;
            return;
        }

        MovementCheck();
    }


    //      METHODS


    private bool JustLanded() => groundedHistory.Contains(true) && groundedHistory.Contains(false) && IsGrounded();

    public bool IsGrounded() => Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

    private void FlipCheck()
    {
        if ((isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f) && !PlayerIsBusyMoving())
            Flip();
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = rb.transform.localScale;
        localScale.x *= -1f;
        rb.transform.localScale = localScale;
    }

    private void JumpCheck()
    {
        jumpingLastFrame = rb.velocity.y > 0f;

        if (groundedHistory.Contains(true) && Input.GetKeyDown(KeyCode.Z) && !PlayerIsBusyMoving() &&
            !playerNail.blockingHistory.Contains(true) && !isJumping)
            Jump();
        
        if (Input.GetKeyUp(KeyCode.Z) && rb.velocity.y > 0f && !PlayerIsBusyMoving() && !isDodging &&
            !playerNail.blockingHistory.Contains(true))
            ExtendJump();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        audioManager.PlayAudioClip(audioManager.playerMovementSounds, "Jumping");
    }

    private void ExtendJump() => rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    
    private void FastFallCheck()
    {
        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.DownArrow) && !IsGrounded() && !IsTouchingWall() && !PlayerIsBusyMoving())
        {
            isFastFalling = true;
            FastFall();
        }
        else if (IsGrounded())
            isFastFalling = false;
    }

    private void FastFall() => rb.velocity = new Vector2(0, fastFallSpeed);
    
    private void DashCheck()
    {
        if (IsGrounded())
            dashReady = true;

        if (!IsTouchingWall() || IsGrounded())
            dashedIntoWall = false;

        if (dashInputHistory.Contains(true) && !fastFallHistory.Contains(true) && DashReady() && !PlayerIsBusyMoving()
            && !(Input.GetKey(KeyCode.DownArrow) && IsGrounded() && !Game.CurrentGameModeIsClassic())  && !playerCharms.excessivelyExtendedCharmEquipped)
            Dash();
    }

    private void Dash()
    {
        isDashing = true;
        currentStunDurationAfterDashIsPerformedInFrames = totalDashStunDurationInFrames;
        currentDashCooldown = totalDashCooldown;

        ShadowDashCheck();

        if (isDashing && !isShadowDashing)
            audioManager.PlayAudioClip(audioManager.playerMovementSounds, "Dashing");

        if (DashedFromWall())
        {
            Flip();
            dashedFromWall = true;
        }

        if (currentWallJumpCantFlip > 0)
            dashSpeed = -dashSpeed;
        
        rb.velocity = new Vector2(isFacingRight ? dashSpeed : -dashSpeed, 0);

        if (currentWallJumpCantFlip > 0)
            dashSpeed = -dashSpeed;
        
        dashReady = false;

        FlipAfterDashingFromWallCheck();
    }

    private void ShadowDashCheck()
    {
        if (currentShadowDashCooldown == 0)
        {
            isShadowDashing = true;
            audioManager.PlayAudioClip(audioManager.playerMovementSounds, "ShadowDashing");
            currentShadowDashCooldown = totalShadowDashCooldown;
        }
    }

    private void ShadowDashReadySoundCheck()
    {
        if (currentShadowDashCooldown == 1)
            audioManager.PlayAudioClip(audioManager.playerMovementSounds, "ShadowDashReady");
    }

    private void DoubleJumpCheck()
    {
        if (IsGrounded())
            doubleJumpReady = true;
        
        else if (doubleJumpReady && Input.GetKeyDown(KeyCode.Z) && !PlayerIsBusyMoving() && !groundedHistory.Contains(true))
            DoubleJump();
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower); ;
        audioManager.PlayAudioClip(audioManager.playerMovementSounds, "DoubleJumping");
        doubleJumpReady = false;
    }

    private void WallJumpCheck()
    {
        Vector2 wallRaycastDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, wallRaycastDirection, 0.25f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Z) && hit.collider != null && !PlayerIsBusyMoving() && !IsGrounded() &&
            ((dashedIntoWall && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))))
            WallJump(wallRaycastDirection);
    }

    private void WallJump(Vector2 wallRaycastDirection)
    {
        rb.velocity = new Vector2(-wallRaycastDirection.x * speed, jumpingPower);
        isWallJumping = true;
        isFacingRight = !isFacingRight;
        doubleJumpReady = true;
        audioManager.PlayAudioClip(audioManager.playerMovementSounds, "Jumping");
        Flip();
    }

    private bool IsTouchingWall()
    {
        Vector2 wallRaycastDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, wallRaycastDirection, 0.25f, groundLayer);

        return hit.collider != null;
    }

    private void WallSlide()
    {
        if (!isWallSliding)
            audioManager.PlayAudioClip(audioManager.playerMovementSounds, "WallCling");

        isWallSliding = true;
        dashReady = true;
        doubleJumpReady = true;

        if (GetPlayerFallingSpeed() <= 0f)
            rb.velocity = new Vector2(0f, wallSlidingSpeed);
    }

    private void BounceOffOfWall()
    {
        isWallJumping = false;
        currentWallJumpCantFlip++;

        if (currentWallJumpCantFlip == totalWallJumpCantFlip)
        {
            currentWallJumpCantFlip = 0;
            isFacingRight = !isFacingRight;
        }
    }

    private void SetPlayerSpeed(float speed) => rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

    public float GetPlayerFallingSpeed() => rb.velocity.y;

    private void HardFallStunCheck()
    {
        if (maxFallSpeedHistory.Contains(true) && !fastFallHistory.Contains(true) && IsGrounded())
        {
            isHardFallStunned = true;
            audioManager.PlayAudioClip(audioManager.playerMovementSounds, "HardFallLanded");
            Invoke("StopHardFallMovementStatus", 1f);
            currentPlayerStunDuration = stunDurationAfterHardFall;
        }
    }

    private void FastFallStunCheck() => currentPlayerStunDuration = fastFallHistory.Contains(true) && IsGrounded() ? stunDurationAfterFastFall : currentPlayerStunDuration;

    private void StopHardFallMovementStatus() => isHardFallStunned = false;

    public void BouncePlayerToRightSide(bool rightSide, int knockbackDuration)
    {
        Vector2 wallRaycastDirection = rightSide ? Vector2.right : Vector2.left;
       
        rb.velocity = new Vector2(-knockbackSpeed * wallRaycastDirection.x, GetPlayerFallingSpeed() > 0 ? rb.velocity.y : 0.25f * rb.velocity.y);

        currentPlayerStunDuration = totalStunDurationAfterKnockBackToTheSide;

        currentKnockbackDuration = knockbackDuration;
    }

    public void BouncePlayerUpwards()
    {
        rb.velocity = new Vector2(rb.velocity.x, 8.75f);
        dashReady = true;
        doubleJumpReady = true;
    }

    public void BouncePlayerDownwards() => rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - rb.velocity.y > -3 ? -11.5f : -7f);

    private void FlipAfterDashingFromWallCheck()
    {   // kind of a bug fix, but not really, because the character doesnt "Flip()" "if (horizontal==0)"
        if (!dashedFromWall) return;

        horizontalInput = isFacingRight ? 1 : -1;
        dashedFromWall = false;
    }

    private bool DashReady() => dashInputHistory.Contains(true) && dashReady && !PlayerIsBusyMoving() && currentDashCooldown == 0;

    private bool DashedFromWall() => IsTouchingWall() && !IsGrounded() && isWallSliding;

    void KnockbackThePlayer() => rb.velocity = (GetPlayerFallingSpeed() > 0 && !Input.GetKeyDown(KeyCode.Z)) ? new Vector2(rb.velocity.x, rb.velocity.y * 0.75f) : rb.velocity;

    private bool PlayerIsDashing()
    {
        if (isDashing)
            dashReady = false;
        
        if (currentStunDurationAfterDashIsPerformedInFrames > 0)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            currentStunDurationAfterDashIsPerformedInFrames -= 1;

            if (IsTouchingWall())
                dashedIntoWall = true;
            
            if (currentStunDurationAfterDashIsPerformedInFrames < totalStunDurationAfterDashIsPerformedInFrames)
                rb.velocity = new Vector2(0f, 0f);
            
            if (currentStunDurationAfterDashIsPerformedInFrames == 0)
            {
                isDashing = false;
                isShadowDashing = false;
            }

            return true;
        }
        else
        {
            rb.gravityScale = normalGravity;
            return false;
        }
    }

    void WallSlideCheck() => isWallSliding = IsTouchingWall() && !IsGrounded() && (dashedIntoWall || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow));

    private void WallClingCheck()
    {
        if (isWallJumping || (currentWallJumpCantFlip > 0 && currentWallJumpCantFlip < totalWallJumpCantFlip))
            BounceOffOfWall();

        else if (IsTouchingWall() && (dashedIntoWall || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            && !isRolling && !IsGrounded() && GetPlayerFallingSpeed() < 7.5f)
            WallSlide();
    }

    private void SetPlayerSpeedCheck()
    {
        if (!(isWallJumping || (currentWallJumpCantFlip > 0 && currentWallJumpCantFlip < totalWallJumpCantFlip))
        && !(IsTouchingWall() && (dashedIntoWall || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
        && GetPlayerFallingSpeed() < 0)) && !isRolling)
            SetPlayerSpeed(speed);
    }

    private void MovementCheck()
    {
        PlayerActionStatusCheck();

        if (PlayerIsDashing() || playerSpells.isFocusing || playerNail.isBlocking || playerNail.isParrying)
            return;

        if (!PlayerIsBusyMoving())
            SetHorizontalInput();
        else
            horizontalInput = 0;
            
        if (!Game.CurrentGameModeIsClassic())
        {
            FastFallCheck();  // this one has to be before double jump because they both use the jump button
            FastFallStunCheck();
            DodgeCheck();
            RollCheck();
        }

        HardFallStunCheck();
        WallSlideCheck();
        WallClingCheck();
        SetPlayerSpeedCheck();
        WallJumpCheck();
        JumpCheck();
        DashCheck();
        DoubleJumpCheck();

        FlipCheck();
    }

    private void MovementSoundCheck()
    {
        if (!playerHealth.playerAlive)
            return;

        ShadowDashReadySoundCheck();
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerMovementSounds, "WallSliding", isWallSliding);
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerMovementSounds, "Running", isRunning);
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerMovementSounds, "Falling", isFalling);
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerMovementSounds, "LandedSoft", JustLanded());
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerMovementSounds, "Rolling", isRolling);
        if (!IsGrounded())
            audioManager.StopAudioClip(audioManager.playerMovementSounds, "Rolling");
    }

    private void PlayerActionStatusCheck()
    {
        isIdling = IsGrounded() && rb.velocity.x == 0 && !isDashing && !isRolling;
        isRunning = IsGrounded() && !isDashing && !isRolling && !playerNail.isBlocking && !playerSpells.isFocusing &&
        (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && !(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow));
        isJumping = rb.velocity.y > 0 && !isFastFalling && !isDodging;
        isFalling = !IsGrounded() && rb.velocity.y < 0 && !isWallSliding;
        isLookingUp = IsGrounded() && rb.velocity.x == 0 && !isDashing && !isRolling && Input.GetKey(KeyCode.UpArrow);
        isLookingDown = IsGrounded() && rb.velocity.x == 0 && !isDashing && !isRolling && Input.GetKey(KeyCode.DownArrow);
    }

    private void SetHorizontalInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            horizontalInput = -1;
        else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = 1;
        else
            horizontalInput = 0;
    }

    private void DodgeCheck()
    {
        if (currentDodgeFrame >= totalDodgeFrames)
        {
            isDodging = false;
            currentDodgeFrame = 0;
        }

        if (dodgeInputHistory.Contains(true) && !PlayerIsBusyMoving() && currentDodgeCooldown == 0)
            Dodge();
    }

    private void Dodge()
    {
        isDodging = true;
        audioManager.PlayAudioClip(audioManager.playerMovementSounds, "Dodging");
        currentDodgeCooldown = totalDodgeCooldown;
        SetCurrentStunDuration(dodgeStunDuration);
    }

    private void RollCheck()
    {
        if (rollInputHistory.Contains(true) && !fastFallHistory.Contains(true) && IsGrounded() && currentRollCooldown == 0 &&
            !IsTouchingWall() && !PlayerIsBusyMoving() && rb.velocity.y <= 0 && !playerNail.isBlocking && !playerCharms.excessivelyExtendedCharmEquipped)
        {
            isRolling = true;
            
            currentRollCooldown = totalRollCooldown;
        }

        if (isRolling)
            RollSpeedCheck();
    }

    private void Roll() //brisi
    {
        isRolling = true;
        currentRollCooldown = totalRollCooldown;
    }

    private void RollSpeedCheck()
    {
        if (!IsGrounded())
        {
            lastFrameRollingInTheAir = true;

            if (lastRollingFrameSpeed >= rollMaxSpeedInTheAir)
                rb.velocity = new Vector2(rollMaxSpeedInTheAir, rb.velocity.y);
            else if (lastRollingFrameSpeed <= -rollMaxSpeedInTheAir)
                rb.velocity = new Vector2(-rollMaxSpeedInTheAir, rb.velocity.y);
            else
                rb.velocity = new Vector2(lastRollingFrameSpeed, rb.velocity.y);

            return;
        }

        if (lastFrameRollingInTheAir)
        {
            if (IsGrounded() && currentRollFrame < changedRollFrameAfterFallingOffOfCliffWhenTouchingGround)
                currentRollFrame = changedRollFrameAfterFallingOffOfCliffWhenTouchingGround;

            lastFrameRollingInTheAir = false;
            return;
        }
       
        if (currentRollFrame < 12 && currentRollFrame >= 0)
            rb.velocity = isFacingRight ? new Vector2(rollStartAndEndSpeed, rb.velocity.y) : new Vector2(-rollStartAndEndSpeed, rb.velocity.y);
        else if (currentRollFrame < 15 && currentRollFrame >= 12)
            rb.velocity = isFacingRight ? new Vector2(rollAfterStartAndBeforeEndSpeed, rb.velocity.y) : new Vector2(-rollAfterStartAndBeforeEndSpeed, rb.velocity.y);
        else if (currentRollFrame < 21 && currentRollFrame >= 15)
            rb.velocity = isFacingRight ? new Vector2(rollMiddleSpeed, rb.velocity.y) : new Vector2(-rollMiddleSpeed, rb.velocity.y);
        else if (currentRollFrame < 24 && currentRollFrame >= 21)
            rb.velocity = isFacingRight ? new Vector2(rollAfterStartAndBeforeEndSpeed, rb.velocity.y) : new Vector2(-rollAfterStartAndBeforeEndSpeed, rb.velocity.y);
        else if (currentRollFrame < 36 && currentRollFrame >= 24)
            rb.velocity = isFacingRight ? new Vector2(rollStartAndEndSpeed, rb.velocity.y) : new Vector2(-rollStartAndEndSpeed, rb.velocity.y);

        currentRollFrame++;

        if (currentRollFrame == totalRollFrames && IsGrounded())
        {
            isRolling = false;
            Flip();
            currentRollFrame = 0;
        }

        lastRollingFrameSpeed = rb.velocity.x;
    }

    public float GetCurrentPlayerStunDuration() => currentPlayerStunDuration;
    
    public Rigidbody2D GetPlayerRigidBody() => rb;
    
    public void SetCurrentStunDuration(int stunDuration) => currentPlayerStunDuration = stunDuration;
    
    public bool GetPlayerIsFacingRight() => isFacingRight;
    
    public bool PlayerIsBusyMoving() => isDashing || isFastFalling || isRolling;
    
    public void StopDashing()
    {
        rb.gravityScale = normalGravity;
        currentStunDurationAfterDashIsPerformedInFrames = 0;
        isDashing = false;
        isShadowDashing = false;
    }

    public void KnockbackPlayerForTouchingEnemyBody(int knockbackDuration) =>
        BouncePlayerToRightSide(rb.position.x < enemy.GetEnemyBodyThatPlayerIsTouching().position.x, knockbackDuration);
    
    private void CooldownCheck()  // ne moze drugacije, jer params-u ne smeju ref-ovi da se salju
    {
        Game.ReduceCooldownIfOnCooldownCheck(ref currentDodgeCooldown);
        Game.ReduceCooldownIfOnCooldownCheck(ref currentRollCooldown);
        Game.ReduceCooldownIfOnCooldownCheck(ref currentDashCooldown);
        Game.ReduceCooldownIfOnCooldownCheck(ref currentShadowDashCooldown);
    }

    private void FillInputHistoryLists()
    {
        dashInputHistory.UpdateArrayQueue(Input.GetKeyDown(KeyCode.C));
        maxFallSpeedHistory.UpdateArrayQueue(rb.velocity.y < maxFallSpeed);
        dodgeInputHistory.UpdateArrayQueue(Input.GetKeyDown(KeyCode.V));
        fastFallHistory.UpdateArrayQueue(isFastFalling);
        rollInputHistory.UpdateArrayQueue(Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.DownArrow));
        groundedHistory.UpdateArrayQueue(IsGrounded());
        jumpingHistory.UpdateArrayQueue(isJumping);
    }
}