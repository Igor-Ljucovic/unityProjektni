using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private Rigidbody2D vengefulSpirit;
    [SerializeField] private Transform abyssShriek;
    [SerializeField] private Transform descendingDarkExplosion;

    [SerializeField] private PlayerSoulVessel playerSoulVessel;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyVisuals enemyVisuals;


    //      GENERAL:


    [HideInInspector] public int spellSoulCost = 33; // samo za showcase je HideInInspector
    private int focusSoulCost = 33;
    private bool playerHasEnoughSoulToCastSpell = false;

    [field: SerializeField] public bool isCastingVengefulSpirit { get; private set; } = false;
    [field: SerializeField] public bool isCastingDescendingDark { get; private set; } = false;
    [field: SerializeField] public bool isCastingAbyssalShriek { get; private set; } = false;

    [field: SerializeField] public bool[] focusHealingHistory { get; private set; } = new bool[2];


    //      FOCUS:


    private   int totalFirstFocusDelay = 23;
    private   int currentFirstFocusDelay = 23;
    private   int totalFocusFocusingDuration = 46;
    private   int currentFocusFocusingDuration = 46; // First focus is 1.14 seconds, subsequental focuses are 0.89 seconds

    private   int focusHealthHealed = 1;

    private float focusSoulDrainedPerFrame;
    private   int focusPauseDurationAfterSuccessfulHeal = 40; // 0.66 seconds of game pause, letting them think about whether they want to keep healing or not
    [field: SerializeField] public bool isFocusing { get; private set; } = false;
    [field: SerializeField] public bool isFocusHealing { get; private set; } = false;


    //     VENGEFUL SPIRIT:


    [HideInInspector] public float vengefulSpiritDelayBeforeSpawning { get; set; } = 0.17f;
    private   int vengefulSpiritPlayerStunDuration = 20;
    private float vengefulSpiritHorizontalVelocity = 25f;
    private float vengefulSpiritDuration = 0.8f;
    private float vengefulSpiritDamageCheckInterval = 0.125f;

    public int vengefulSpiritDamagePerHit { get; set; } = 10;
    private float vengefulSpiritHorizontalOffset = 0.25f;
    public float vengefulSpiritHorizontalHitbox { get; private set; } = 6f;
    public float vengefulSpiritVerticalHitbox { get; private set; } = 1.2f;

    private int currentVengefulSpiritCooldown = 0;
    private int totalVengefulSpiritCooldown = 30;

    private int totalKnockbackDurationForUsingVengefulSpirit = 3;


    //      ABYSS SHRIEK:



    public float abyssShriekDelayBeforeSpawning { get; set; } = 0.17f;
    private   int abyssShriekPlayerStunDuration = 30;
    private float abyssShriekDuration = 1f;
    private float abyssShriekDamageCheckInterval = 0.25f;


    public int abyssShriekDamagePerHit { get; set; } = 20;
    private float abyssShriekYAxisOffset = 2.5f;
    public float abyssShriekRadius { get; private set; } = 1.95f;

    private int currentAbyssShriekCooldown = 0;
    private int totalAbyssShriekCooldown = 30;


    //      DESCENDING DARK:


    private float descendingDarkAscendVerticalVelocityWhenGrounded = 17.5f;
    private float descendingDarkAscendVerticalVelocityWhenNotGrounded = 1f;
    private float descendingDarkAscendDuration = 0.15f;

    public float descendingDarkDescendDelay { get; set; } = 0.5f;
    private float descendingDarkDescendVerticalVelocity = -25f;
    public int descendingDarkDescendDamage { get; set; } = 20;
    public float descendingDarkDescendHorizontalHitbox { get; private set; } = 0.8f;
    public float descendingDarkDescendVerticalHitbox { get; private set; } = 0.8f;

    public int descendingDarkExplosionDamage { get; set; } = 30;
    private float descendingDarkExplosionVerticalOffset = 0f;
    private float descendingDarkExplosionDamageCheckInterval = 0.5f;
    public float descendingDarkExplosionHorizontalHitbox { get; private set; } = 6f;
    public float descendingDarkExplosionVerticalHitbox { get; private set; } = 1.2f;

    private float descendingDarkPlayerStunDurationAfterLanding = 0.45f;
    public float descendingDarkPlayerInvunlerabilityDurationAfterLanding { get; set; } = 1.25f;
    public int descendingDarkPlayerStunDurationAfterLandingInFrames { get; private set; } = 30;

    private  bool descendingDarkDescendHitEnemy = false;
    [field: SerializeField] public   bool isDescendingDarkAscending { get; private set; } = false;
    [field: SerializeField] public   bool isDescendingDarkDescending { get; private set; } = false;
    [field: SerializeField] public   bool descendingDarkInvulnerability { get; private set; } = false;
    [field: SerializeField] public   bool doDescendingDarkDescendingAnimation { get; private set; } = false;
    private    int currentDescendingDarkCooldown = 0;
    private   int totalDescendingDarkCooldown = 30;


    //      HELPER VARIABLES:


    public Rigidbody2D vengefulSpiritInstance { get; private set; }
    public Transform abyssShriekInstance { get; private set; }
    public Transform descendingDarkExplosionInstance { get; private set; }


    //      SETUP:


    private void Start()
    {
        // kod ispod zaokruzi broj SOUL-a potrosenih po frame-u na brojku koja ima 5 decimala i sigurno nije preko 33 po FOCUS HEAL-u
        focusSoulDrainedPerFrame = (float)Mathf.Floor((float)focusSoulCost / totalFocusFocusingDuration * 1e4f) / 1e4f;
    }


    //      CODE:


    void Update()
    {
        FillInputHistoryLists();

        SpellsSoundCheck();

        if (Game.Paused())
            return;

        CooldownCheck();

        if (playerMovement.PlayerIsBusyMoving() || playerMovement.isWallSliding)
            return;

        SpellCheck();

        playerSoulVessel.FillBigSoulVesselCheck();
    }

    private void SpellsSoundCheck()
    {
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerSpellsSounds, "DescendingDarkAscending", isDescendingDarkAscending);
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerSpellsSounds, "DescendingDarkDescending", isDescendingDarkDescending);
        audioManager.PlayAudioClipIfItsNotPlaying(audioManager.playerSpellsSounds, "FocusFocusing", isFocusing);
    }

    //      METHODS:


    private void SpellCheck()
    {
        DescendingDarkCheck();

        if (playerMovement.GetCurrentPlayerStunDuration() > 0)   // Descending Dark has to keep checking if the player touches the ground to stop the player stun
            return;

        FocusCheck();

        PlayerHasEnoughSoulToCastSpellCheck();

        AbyssShriekCheck();
        VengefulSpiritCheck();
    }

    private void PlayerHasEnoughSoulToCastSpellCheck() => playerHasEnoughSoulToCastSpell = playerSoulVessel.currentBigSoulVesselSoul >= spellSoulCost;

    private void PlayerHasEnoughSoulToCastFocusCheck() => playerHasEnoughSoulToCastSpell = playerSoulVessel.currentBigSoulVesselSoul >= focusSoulCost;

    private void PlayerCastDamagingSpellCheck()
    {
        playerSoulVessel.DrainSoulVessels(spellSoulCost);
        playerSoulVessel.currentSoulPourTimer = playerSoulVessel.totalSoulPourTimer;
        playerHasEnoughSoulToCastSpell = false;
    }

    private void FocusCheck()
    {
        PlayerHasEnoughSoulToCastFocusCheck();

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow) && playerMovement.IsGrounded() 
            && (playerHasEnoughSoulToCastSpell  || (isFocusing && playerSoulVessel.currentBigSoulVesselSoul > 0)))
        {
            isFocusing = true;
            playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, 0f);

            if (currentFirstFocusDelay > 0)
                currentFirstFocusDelay--;
            else
                StartFocusing();
        }
        else
            ResetSoulAfterFocusing();
    }

    public void ResetSoulAfterFocusing()
    {
        isFocusing = isFocusHealing = playerHasEnoughSoulToCastSpell = false;
        (currentFirstFocusDelay, currentFocusFocusingDuration) = (totalFirstFocusDelay, totalFocusFocusingDuration);
        playerSoulVessel.RoundDownAllSoulVessels();
    }

    private void StartFocusing()
    {
        if (currentFocusFocusingDuration > 0) //ovde mozda
        {
            isFocusHealing = true;
            currentFocusFocusingDuration--;
            playerSoulVessel.DrainSoulVessels(focusSoulDrainedPerFrame);

            if (playerSoulVessel.currentBigSoulVesselSoul < 0)
                playerSoulVessel.ResetBigSoulVesselSoulToZero();

            playerSoulVessel.currentSoulPourTimer = playerSoulVessel.totalSoulPourTimer;
        }
        else
            Focus();    
    }

    private void Focus()
    {
        playerHealth.HealPlayer(focusHealthHealed);

        currentFocusFocusingDuration = totalFocusFocusingDuration;
        isFocusing = false;
        playerHasEnoughSoulToCastSpell = false;
        audioManager.PlayAudioClip(audioManager.playerSpellsSounds, "FocusHeal");
        Game.PauseTheGameTemporarily(focusPauseDurationAfterSuccessfulHeal);
    }

    private void VengefulSpiritCheck()
    {
        if (playerHasEnoughSoulToCastSpell && !Input.GetKey(KeyCode.UpArrow) && Input.GetKeyUp(KeyCode.A) && !Input.GetKey(KeyCode.DownArrow)
            && !focusHealingHistory.Contains(true) && currentVengefulSpiritCooldown == 0)
            VengefulSpirit();
    }

    private void VengefulSpirit()
    {
        isCastingVengefulSpirit = true;
        playerMovement.BouncePlayerToRightSide(playerMovement.GetPlayerIsFacingRight(), totalKnockbackDurationForUsingVengefulSpirit); 
        playerMovement.GetPlayerRigidBody().velocity = new Vector2(playerMovement.GetPlayerRigidBody().velocity.x, 0f);
        playerMovement.GetPlayerRigidBody().gravityScale = 0;
        playerMovement.SetCurrentStunDuration(vengefulSpiritPlayerStunDuration);

        PlayerCastDamagingSpellCheck();

        currentVengefulSpiritCooldown = totalVengefulSpiritCooldown;

        ChangeVengefulSpiritSideCheck();

        Invoke("VengefulSpiritSummon", vengefulSpiritDelayBeforeSpawning);
    }

    private void ChangeVengefulSpiritSideCheck()
    {
        if (playerMovement.GetPlayerIsFacingRight() && vengefulSpiritHorizontalOffset < 0)
        {
            vengefulSpiritHorizontalOffset *= -1;
            vengefulSpiritHorizontalVelocity *= -1;
        }
        else if (!playerMovement.GetPlayerIsFacingRight() && vengefulSpiritHorizontalOffset > 0)
        {
            vengefulSpiritHorizontalOffset *= -1;
            vengefulSpiritHorizontalVelocity *= -1;
        }
    }

    private void VengefulSpiritSummon()
    {
        vengefulSpiritInstance = Instantiate(vengefulSpirit, playerMovement.GetPlayerRigidBody().position, Quaternion.identity);

        vengefulSpiritInstance.GetComponent<SpriteRenderer>().enabled = true;

        vengefulSpiritInstance.transform.position = new Vector2(playerMovement.GetPlayerRigidBody().position.x + vengefulSpiritHorizontalOffset,
                                                             playerMovement.GetPlayerRigidBody().position.y);

        vengefulSpiritInstance.velocity = new Vector2(vengefulSpiritHorizontalVelocity, 0f); //spriteRenderer.flipX = true;

        if (!playerMovement.GetPlayerIsFacingRight())
            vengefulSpiritInstance.GetComponent<SpriteRenderer>().flipX = true;

        for (float i = 0; i < vengefulSpiritDuration; i += vengefulSpiritDamageCheckInterval)
            Invoke("VengefulSpiritHitEnemyCheck", i);

        Invoke("StopCastingVengefulSpirit", 0.25f);

        audioManager.PlayAudioClip(audioManager.playerSpellsSounds, "VengefulSpiritCast");

        Destroy(vengefulSpiritInstance.gameObject, vengefulSpiritDuration);
    }

    private void StopCastingVengefulSpirit() => isCastingVengefulSpirit = false;
    
    private void VengefulSpiritHitEnemyCheck()
    {
        if (enemy.AnyEnemyGotHitByAttack(vengefulSpiritInstance.transform))   // can't hit the same enemy multiple times
        {
            enemy.LowerAllEnemyHealthCheck(vengefulSpiritInstance?.transform, vengefulSpiritDamagePerHit);
            enemyVisuals.FillListOfEnemySpritesToChangeColorForBeingHit(vengefulSpiritInstance?.transform);
        } 
    }

    private void AbyssShriekCheck()
    {
        if (playerHasEnoughSoulToCastSpell && Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.UpArrow) && currentAbyssShriekCooldown == 0)
        {
            playerMovement.GetPlayerRigidBody().velocity = new Vector2(playerMovement.GetPlayerRigidBody().velocity.x, 0f);
            playerMovement.GetPlayerRigidBody().gravityScale = 0;
            playerMovement.SetCurrentStunDuration(abyssShriekPlayerStunDuration);
            Invoke("AbyssShriek", abyssShriekDelayBeforeSpawning);
        }
    }

    private void AbyssShriek()
    {
        isCastingAbyssalShriek = true;

        PlayerCastDamagingSpellCheck();
        currentAbyssShriekCooldown = totalAbyssShriekCooldown;

        SummonAbyssShriek();
    }

    private void SummonAbyssShriek()
    {
        abyssShriekInstance = Instantiate(abyssShriek, playerMovement.GetPlayerRigidBody().position, Quaternion.identity);

        abyssShriekInstance.GetComponent<SpriteRenderer>().enabled = true;

        abyssShriekInstance.transform.position = new Vector2(playerMovement.GetPlayerRigidBody().position.x,
                                                          playerMovement.GetPlayerRigidBody().position.y + abyssShriekYAxisOffset);

        for (float i = 0; i < abyssShriekDuration; i += abyssShriekDamageCheckInterval)
            Invoke("AbyssShriekHitEnemyCheck", i);

        audioManager.PlayAudioClip(audioManager.playerSpellsSounds, "AbyssShriekCast");

        Invoke("StopCastingAbyssShriek", 0.4f);

        Destroy(abyssShriekInstance.gameObject, abyssShriekDuration);
    }

    private void StopCastingAbyssShriek() => isCastingAbyssalShriek = false;
    
    private void AbyssShriekHitEnemyCheck()
    {
        enemy.LowerAllEnemyHealthCheck(abyssShriekInstance, abyssShriekDamagePerHit);
        enemyVisuals.FillListOfEnemySpritesToChangeColorForBeingHit(abyssShriekInstance);
    }

    private void DescendingDarkCheck()
    {
        if (playerHasEnoughSoulToCastSpell && Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.DownArrow)
            && !isDescendingDarkDescending && currentDescendingDarkCooldown == 0 && playerMovement.GetCurrentPlayerStunDuration() == 0)
            DescendingDark();
        
        if (isDescendingDarkDescending && playerMovement.IsGrounded())
        {
            currentDescendingDarkCooldown = totalDescendingDarkCooldown;
            playerMovement.SetCurrentStunDuration(descendingDarkPlayerStunDurationAfterLandingInFrames);
            Invoke("StopDescendingDarkDescendAnimation", descendingDarkPlayerStunDurationAfterLanding);
            Invoke("PlayerBecomesVulnerableAgain", descendingDarkPlayerInvunlerabilityDurationAfterLanding);
            DescendingDarkSummonExplosion();
            audioManager.PlayAudioClip(audioManager.playerSpellsSounds, "DescendingDarkExplosion");
            isDescendingDarkDescending = false;
        }

        if (isDescendingDarkDescending && !descendingDarkDescendHitEnemy && enemy.AnyEnemyGotHitByAttack(playerMovement.GetPlayerRigidBody().transform))
        {
            descendingDarkDescendHitEnemy = true;
            enemy.LowerAllEnemyHealthCheck(playerMovement.GetPlayerRigidBody().transform, descendingDarkDescendDamage);
            enemyVisuals.FillListOfEnemySpritesToChangeColorForBeingHit(playerMovement.GetPlayerRigidBody().transform);
        }
    }

    private void StopDescendingDarkDescendAnimation() => doDescendingDarkDescendingAnimation = false;
    
    private void DescendingDark()
    {
        CancelInvoke("PlayerBecomesVulnerableAgain");
        descendingDarkInvulnerability = doDescendingDarkDescendingAnimation = isDescendingDarkAscending = true;
        descendingDarkDescendHitEnemy = false;
        playerMovement.GetPlayerRigidBody().gravityScale = 0;
        Invoke("StartDescending", descendingDarkDescendDelay);
        playerMovement.SetCurrentStunDuration(int.MaxValue);  // careful with this line of code!
        
        if (playerMovement.IsGrounded())
        {
            playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, descendingDarkAscendVerticalVelocityWhenGrounded);
            PlayerCastDamagingSpellCheck();
            Invoke("StopAscending", descendingDarkAscendDuration);
        }
        else
            playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, descendingDarkAscendVerticalVelocityWhenNotGrounded);
    }

    private void StopAscending() => playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, 0f);
    
    private void StartDescending()
    {
        playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, descendingDarkDescendVerticalVelocity);
        isDescendingDarkAscending = false;
        isDescendingDarkDescending = true;
        playerSoulVessel.DrainSoulVessels(spellSoulCost);
    }

    private void DescendingDarkSummonExplosion()
    {
        descendingDarkExplosionInstance = Instantiate(descendingDarkExplosion, playerMovement.GetPlayerRigidBody().position, Quaternion.identity);

        descendingDarkExplosionInstance.GetComponent<SpriteRenderer>().enabled = true;

        descendingDarkExplosionInstance.transform.position = new Vector2(playerMovement.GetPlayerRigidBody().position.x,
                                                                      playerMovement.GetPlayerRigidBody().position.y + descendingDarkExplosionVerticalOffset);

        Invoke("DescendingDarkExplosionHitEnemyCheck", 0f);
        Invoke("DescendingDarkExplosionHitEnemyCheck", descendingDarkExplosionDamageCheckInterval);
        Destroy(descendingDarkExplosionInstance.gameObject, descendingDarkExplosionDamageCheckInterval * 2);
    }

    private void DescendingDarkExplosionHitEnemyCheck()
    {
        enemy.LowerAllEnemyHealthCheck(descendingDarkExplosionInstance, descendingDarkExplosionDamage);
        enemyVisuals.FillListOfEnemySpritesToChangeColorForBeingHit(descendingDarkExplosionInstance);
    }

    private void PlayerBecomesVulnerableAgain() => descendingDarkInvulnerability = false;
    
    private void FillInputHistoryLists() => focusHealingHistory.UpdateArrayQueue(isFocusHealing);
    
    private void CooldownCheck()
    {
        Game.ReduceCooldownIfOnCooldownCheck(ref currentAbyssShriekCooldown);
        Game.ReduceCooldownIfOnCooldownCheck(ref currentVengefulSpiritCooldown);
        Game.ReduceCooldownIfOnCooldownCheck(ref currentDescendingDarkCooldown);
    }
}
