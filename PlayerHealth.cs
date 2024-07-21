using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private Enemy               enemy;
    [SerializeField] private PlayerMovement      playerMovement;
    [SerializeField] private PlayerSpells        playerSpells;
    [SerializeField] private PlayerNail          playerNail;
    [SerializeField] private PlayerCharms        playerCharms;
    [SerializeField] private PlayerHealthSprites playerHealthSprite;
    [SerializeField] private AudioManager        audioManager;

    [SerializeField] private SpriteRenderer      playerSprite;
    [SerializeField] private Sprite              playerTakingDamageSprite;


    //      GENERAL:


    public    int maxPlayerHealth = 9;
    public    int currentPlayerHealth = 9;
    public   bool playerAlive = true;
    public   bool playerInvunlerable = false;
    public   bool doTakingDamageAnimation = false;
    
    public    int totalPlayerInvulnerabilityAfterTakingDamage = 135;
    private   int currentPlayerInvulnerabilityAfterTakingDamage = 0;

    public    int damageTakenForTouchingEnemy = 1;

    private   int totalPauseDurationAfterTouchingEnemy = 30;
    private   int currentPauseDurationAfterTouchingEnemy = 0;

    private   int playerStunDurationForTouchingEnemy = 9;

    private   int totalKnockbackDurationForTouchingEnemy = 3;

    private float slowlyTurnAllSoundsBackAfterTakingDamageDelay = 0.35f;
    private float slowlyTurnAllSoundsBackAfterTakingDamageTransitionDuration = 1f;


    //      SETUP:


    private void Start()
    {
        playerCharms.TriggerUnimpairedCataclysmCharm();
    }


    //      CODE:


    void Update()
    {
        if (Game.Paused())
            return;

        if (!playerAlive)
        {
            Game.PauseTheGameTemporarily(int.MaxValue);
            playerMovement.SetCurrentStunDuration(int.MaxValue);   // ovoliko frame-ova se stun-uje i pauzira kada nije ziv igrac
            playerMovement.GetPlayerRigidBody().velocity = new Vector2(0f, 0f);
            return;
        }
        
        if (currentPauseDurationAfterTouchingEnemy > 0)
        {
            Game.PauseTheGameTemporarily(currentPauseDurationAfterTouchingEnemy);
            currentPauseDurationAfterTouchingEnemy = 0;
        }

        Game.ReduceCooldownIfOnCooldownCheck(ref currentPlayerInvulnerabilityAfterTakingDamage);

        PlayerInvulnerableCheck();

        if (enemy.PlayerTouchingEnemyBody() && currentPlayerInvulnerabilityAfterTakingDamage == 0 && playerAlive && !playerInvunlerable) {

            DamagePlayer();
            PlayerAliveCheck();
            playerMovement.KnockbackPlayerForTouchingEnemyBody(totalKnockbackDurationForTouchingEnemy);
            StunPlayerForTouchingEnemyBody();
            currentPlayerInvulnerabilityAfterTakingDamage = totalPlayerInvulnerabilityAfterTakingDamage;
        }
    }


    //      METHODS:


    private void DamagePlayer()
    {
        doTakingDamageAnimation = true;

        currentPlayerHealth -= damageTakenForTouchingEnemy;
        playerCharms.TriggerFuryOfTheFallenCharmCheck();
        playerCharms.TriggerAbyssalDesperationCharmCheck();
        playerCharms.TriggerUnimpairedCataclysmCharm();
        playerHealthSprite.ChangeHealthSpritesCheck();

        playerSpells.ResetSoulAfterFocusing();

        DamagePlayerAudioCheck();

        Invoke("StopTakingDamageAnimation", 0.2f);
    }

    private void DamagePlayerAudioCheck()
    {
        audioManager.ChangeAllMusicVolume(0f);
        audioManager.StopAllSounds();
        audioManager.ChangeAudioClipVolume(audioManager.playerHealthSounds, "TakingDamage", 1f);
        audioManager.ChangeAudioClipVolume(audioManager.playerHealthSounds, "TakingDoubleDamage", 1f);

        if (damageTakenForTouchingEnemy == 1)
            audioManager.PlayAudioClip(audioManager.playerHealthSounds, "TakingDamage");
        else
            audioManager.PlayAudioClip(audioManager.playerHealthSounds, "TakingDoubleDamage");

        audioManager.SlowlyTurnAllAudioClipsVolumeBackAfterDelay(slowlyTurnAllSoundsBackAfterTakingDamageDelay,
                                                                 slowlyTurnAllSoundsBackAfterTakingDamageTransitionDuration, false, true);
    }

    public void HealPlayer(int healthHealed)
    {
        if (currentPlayerHealth < maxPlayerHealth && playerAlive)
        {
            currentPlayerHealth += healthHealed;
            playerCharms.TriggerFuryOfTheFallenCharmCheck();
            playerCharms.TriggerUnimpairedCataclysmCharm();
        }
        playerHealthSprite.ChangeHealthSpritesCheck();
    }

    private void StopTakingDamageAnimation() => doTakingDamageAnimation = false;

    private void PlayerInvulnerableCheck() => playerInvunlerable = playerMovement.isRolling ||  playerMovement.isDodging ||
        currentPlayerInvulnerabilityAfterTakingDamage > 0 || playerSpells.descendingDarkInvulnerability || playerMovement.isShadowDashing;

    private void StunPlayerForTouchingEnemyBody()
    {
        playerMovement.StopDashing();
        currentPauseDurationAfterTouchingEnemy = totalPauseDurationAfterTouchingEnemy;
        playerMovement.SetCurrentStunDuration(playerStunDurationForTouchingEnemy);
        playerSprite.sprite = playerTakingDamageSprite;
    }

    private void PlayerAliveCheck()
    {
        if (currentPlayerHealth > 0)
            return;

        currentPlayerHealth = 0;
        playerAlive = false;
        audioManager.PlayAudioClip(audioManager.playerHealthSounds, "Death");
    }
}