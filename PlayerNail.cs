using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerNail : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] internal Transform slash;

    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerSoulVessel playerSoulVessel;
    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerVisualsEffects playerVisuals;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyVisuals enemyVisuals;
    [SerializeField] private AudioManager audioManager;


    //      NAIL STATUS:


    [SerializeField] internal bool isParrying = false;
    [SerializeField] internal bool isBlocking = false;
    internal float slashCurrentHorizontalHitbox = 0f;
    internal float slashCurrentVerticalHitbox = 0f;
    internal  bool isUpSlashing = false;
    internal  bool isDownSlashing = false;
    internal  bool isSideSlashing = false;


    //      NAIL NUMBERS:


    [SerializeField] internal int currentNailDamage = 21;
    internal int normalNailDamage = 21;

    [SerializeField] private  float currentSlashHorizontalOffset = 1.625f;
    [SerializeField] internal float normalSlashHorizontalOffset = 1.625f;
    [SerializeField] private  float currentSlashVerticalOffset = 1.2f;
    [SerializeField] internal float normalSlashVerticalOffset = 1.2f;

    internal float slashHorizontalHitboxWhenSlashingHorizontally = 3f;
    internal float slashVerticalHitboxWhenSlashingHorizontally = 1.25f;
    internal float slashHorizontalHitboxWhenSlashingVertically = 1f;
    internal float slashVerticalHitboxWhenSlashingVertically = 2.75f;

    private int amountOfStartingBlockFramesThatCanParry = 15;


    //      TIMERS:


    [SerializeField] private int currentSlashCooldown = 0;
    [SerializeField] internal int totalSlashCooldown = 24;

    [SerializeField] private int currentFrameOfBlocking = 0;
    private  int minimumFramesOfBlocking = 30;

    private  int totalStunDurationAfterDoneBlocking = 6;

    public   int totalKnockbackDurationForSlashingSideways = 3;
    public   int totalKnockbackDurationForBlockingParryableAttack = 10;

    public float slashDuration = 0.15f; // ovo treba ustvari da bude ili 0, ili da ucinis transparentnim skroz taj sprite jednostavno


    //      LISTS:


    private bool[] slashInputHistory = new bool[9];
    [HideInInspector] public  bool[] blockingHistory = new bool[2];


    //      HELPER VARIABLES:


    private Transform slashSpawn;


    //      SETUP:


    private void Start()
    {
        Invoke("SetCurrentSlashOffsetsToNormalOffsets", 0.2f);
    }


    private void SetCurrentSlashOffsetsToNormalOffsets() => (currentSlashHorizontalOffset, currentSlashVerticalOffset) = (normalSlashHorizontalOffset, normalSlashVerticalOffset);


    //      CODE:


    private void Update()
    {
        FillInputHistoryLists();

        if (Game.Paused())
            return;
        
        CooldownCheck();

        playerCharms.TriggerSonicSoulCharmCheck();

        if (!Game.CurrentGameModeIsClassic())
        {
            playerCharms.bladeDanceSymphonyCurrentFrameWithoutSlashingEnemyForCriticalHit++;
            KeepBlockingCheck();
        }
        
        if (playerMovement.GetCurrentPlayerStunDuration() > 0 || playerMovement.PlayerIsBusyMoving())
            return;
        
        NailCombatCheck();
    }


    //      METHODS:


    private void NailCombatCheck()
    {
        if (!Game.CurrentGameModeIsClassic())
        {
            BlockCheck();
            ParryCheck();
        }
        
        SlashCheck();
    }

    private void KeepBlockingCheck()
    {
        if (isBlocking)
        {
            if (currentFrameOfBlocking <= minimumFramesOfBlocking - 2)
                playerMovement.SetCurrentStunDuration(2);

            else if (Input.GetKey(KeyCode.X) && Input.GetKey(KeyCode.DownArrow))
                playerMovement.SetCurrentStunDuration(totalStunDurationAfterDoneBlocking);

            currentFrameOfBlocking++;

            if (!Input.GetKey(KeyCode.X) && currentFrameOfBlocking >= minimumFramesOfBlocking)
                playerMovement.SetCurrentStunDuration(0);

            if (!playerMovement.IsGrounded())
                isBlocking = isParrying = false;
        }
        ParryCheck();
    }

    private void ParryCheck() => isParrying = isBlocking && currentFrameOfBlocking <= amountOfStartingBlockFramesThatCanParry;
    
    private void BlockCheck()
    {
        isBlocking = false;

        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow) && playerMovement.IsGrounded() && !Input.GetKeyDown(KeyCode.Z)
            && !playerMovement.PlayerIsBusyMoving())
            // "!Input.GetKeyDown(KeyCode.Z)" because block and jump can't be pressed at the same time (the player would parry while jumping)
            Block();
        
        if (!isBlocking)
            currentFrameOfBlocking = 0;
    }

    private void Block() => (playerMovement.GetPlayerRigidBody().velocity, isBlocking) = (new Vector2(0f, 0f), true);

    private void FlipSlash() => slashSpawn.transform.localScale *= -1;
    
    private void SlashCheck()
    {
        if (!Game.CurrentGameModeIsClassic())
        {
            playerCharms.TriggerCurseOfTheSoullessCharmCheck();
            playerCharms.TriggerAirborneSupremacyCharmCheck();
        }
        
        if (slashInputHistory.Contains(true) && !(Input.GetKey(KeyCode.DownArrow) && playerMovement.IsGrounded() &&
            !Game.CurrentGameModeIsClassic()) && currentSlashCooldown == 0 && !playerMovement.PlayerIsBusyMoving())
            Slash();
    }

    private void Slash()
    {
        SlashSummon(out slashSpawn);

        if (enemy.AnyEnemyGotHitByAttack(slashSpawn))
        {
            playerCharms.TriggerBladeDanceSymphonyCharmCheck();

            foreach (Collider2D enemyDealingDamageHitbox in enemy.AllEnemyHitboxesHitWithSlash(slashSpawn))
                playerSoulVessel.SetPlayerSoulAfterSlashingEnemy();

            enemyVisuals.FillListOfEnemySpritesToChangeColorForBeingHit(slashSpawn);

            enemy.LowerAllEnemyHealthCheck(slashSpawn, currentNailDamage);
            playerVisuals.SpawnSlashParticles(slashSpawn);
        }

        if ((SlashHitWall() && !isDownSlashing) || enemy.AnyEnemyGotHitByAttack(slashSpawn))
            SlashBouncePlayer();

        if (SlashHitWall() && !enemy.AnyEnemyGotHitByAttack(slashSpawn))
            audioManager.PlayAudioClip(audioManager.playerNailSoundsWithoutSlash, "SlashHitWall");

        audioManager.PlayRandomSound(audioManager.playerSlashSounds);

        currentSlashCooldown = totalSlashCooldown;

        Destroy(slashSpawn.gameObject, slashDuration);
    }

    private void SlashSummon(out Transform slashSpawn)
    {
        slashSpawn = Instantiate(slash, playerMovement.GetPlayerRigidBody().position, Quaternion.identity);

        slashSpawn.GetComponent<SpriteRenderer>().enabled = true;

        slashSpawn.transform.parent = playerMovement.GetPlayerRigidBody().transform; // HARDCODED

        ChangeSlashSideAndPositionCheck();
    }

    private void ChangeSlashSideAndPositionCheck()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !playerMovement.isWallSliding)
        {
            currentSlashHorizontalOffset = 0;
            slashSpawn.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            isUpSlashing = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !playerMovement.IsGrounded() && !playerMovement.isWallSliding)
        {
            currentSlashVerticalOffset *= -1;
            currentSlashHorizontalOffset = 0;
            slashSpawn.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            isDownSlashing = true;
        }
        else
        {
            currentSlashVerticalOffset = 0f;
            isSideSlashing = true;
            if (!playerMovement.GetPlayerIsFacingRight())
            {
                FlipSlash();
                currentSlashHorizontalOffset *= -1;
            }
        }

        slashSpawn.transform.position = new Vector2(playerMovement.GetPlayerRigidBody().position.x + currentSlashHorizontalOffset,
                                                    playerMovement.GetPlayerRigidBody().position.y + currentSlashVerticalOffset);

        ResetSlashPosition();
        Invoke("ResetSlashState", slashDuration);
    }

    private void ResetSlashState() => isSideSlashing = isUpSlashing = isDownSlashing = false;

    private bool IsSlashingVertically() => isUpSlashing || isDownSlashing;

    public bool IsSlashing() => IsSlashingVertically() || isSideSlashing;

    private void ResetSlashPosition()
    {
        if (currentSlashHorizontalOffset == 0)
            currentSlashHorizontalOffset = normalSlashHorizontalOffset;
        else if (currentSlashHorizontalOffset < 0)
            currentSlashHorizontalOffset *= -1;
        
        if (currentSlashVerticalOffset == 0)
            currentSlashVerticalOffset = normalSlashVerticalOffset;
        else if (currentSlashVerticalOffset < 0)
            currentSlashVerticalOffset *= -1;
    }

    private void SlashBouncePlayer()
    {
        if (isDownSlashing)
            playerMovement.BouncePlayerUpwards();
        else if (isUpSlashing)
            playerMovement.BouncePlayerDownwards();
        else
            playerMovement.BouncePlayerToRightSide(playerMovement.GetPlayerIsFacingRight(), totalKnockbackDurationForSlashingSideways);
    }

    private bool SlashHitWall()
    {
        Vector2 halfSize = new Vector2(slashCurrentHorizontalHitbox * 0.5f, slashCurrentVerticalHitbox * 0.5f);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(slashSpawn.transform.position, halfSize, 0f, groundLayer);

        return colliders.Length > 0;
    }

    public void SlashChangeHitboxCheck()
    {
        slashCurrentHorizontalHitbox = IsSlashingVertically() ? slashHorizontalHitboxWhenSlashingVertically : slashHorizontalHitboxWhenSlashingHorizontally;
        slashCurrentVerticalHitbox = IsSlashingVertically() ? slashVerticalHitboxWhenSlashingVertically : slashVerticalHitboxWhenSlashingHorizontally;
    }

    private void CooldownCheck()
    {
        Game.ReduceCooldownIfOnCooldownCheck(ref currentSlashCooldown);
    }

    private void FillInputHistoryLists()
    {
        slashInputHistory.UpdateArrayQueue(Input.GetKeyDown(KeyCode.X));
        blockingHistory.UpdateArrayQueue(isBlocking);
    }
}
