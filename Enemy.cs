using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerSpells playerSpells;
    [SerializeField] private EnemyVisuals enemyVisuals;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private LayerMask dealingEnemyDamageLayer;
    [SerializeField] private LayerMask takingEnemyDamageLayer;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Boss1Health boss1Health;   // HARD CODED je to sto se dodaju svi boss-evi ovde
    [SerializeField] private Boss2Health boss2Health;


    //      GENERAL:


    public List<Transform> enemyDealingDamageToPlayerHitboxes = new List<Transform>();   // Has to be MANUALLY UPDATED for each new enemy spawned after the game started

    private int totalBossStunDurationAfterPlayerParriedIt = 150;


    //      HELPING VARIABLES:


    private Collider2D[] enemyHitboxesHitWithPlayerAttack;


    //      SETUP:


    private void Start()
    {
        FillEnemyDealingDamageToPlayerHitboxesList();
    }


    //      METHODS:


    public bool PlayerSlashTouchingEnemy(Transform slash, Transform enemyHitbox)  //ovo treba sa getter-ima i setter-ima, umesto da budu public attribute-i
    {
        playerNail.SlashChangeHitboxCheck();

        Vector2 playerAttackHalfSize = new Vector2(playerNail.slashCurrentHorizontalHitbox, playerNail.slashCurrentVerticalHitbox);
        enemyHitboxesHitWithPlayerAttack = Physics2D.OverlapBoxAll(slash.transform.position, playerAttackHalfSize, 0f, dealingEnemyDamageLayer);

        return PlayerAttackTouchingGivenEnemy(enemyHitbox);
    }

    public bool PlayerVengefulSpiritTouchingEnemy(Rigidbody2D vengefulSpirit, Transform enemyHitbox)
    {
        Vector2 playerAttackHalfSize = new Vector2(playerSpells.vengefulSpiritHorizontalHitbox, playerSpells.vengefulSpiritVerticalHitbox);
        enemyHitboxesHitWithPlayerAttack = Physics2D.OverlapBoxAll(vengefulSpirit.transform.position, playerAttackHalfSize, 0f, dealingEnemyDamageLayer);

        return PlayerAttackTouchingGivenEnemy(enemyHitbox);
    }

    public bool PlayerAbyssShriekTouchingEnemy(Transform abyssShriek, Transform enemyHitbox)
    {
        enemyHitboxesHitWithPlayerAttack = Physics2D.OverlapCircleAll(abyssShriek.transform.position, playerSpells.abyssShriekRadius, dealingEnemyDamageLayer);

        return PlayerAttackTouchingGivenEnemy(enemyHitbox);
    }

    public bool PlayerDescendingDarkDescendTouchingEnemy(Transform playerBody, Transform enemyHitbox)
    {
        Vector2 playerAttackHalfSize = new Vector2(playerSpells.descendingDarkDescendHorizontalHitbox, playerSpells.descendingDarkDescendVerticalHitbox);
        enemyHitboxesHitWithPlayerAttack = Physics2D.OverlapBoxAll(playerBody.transform.position, playerAttackHalfSize, 0f, dealingEnemyDamageLayer);

        return PlayerAttackTouchingGivenEnemy(enemyHitbox);
    }

    public bool PlayerDescendingDarkExplosionTouchingEnemy(Transform descendingDarkExplosion, Transform enemyHitbox)
    {
        Vector2 playerAttackHalfSize = new Vector2(playerSpells.descendingDarkExplosionHorizontalHitbox, playerSpells.descendingDarkExplosionVerticalHitbox);
        enemyHitboxesHitWithPlayerAttack = Physics2D.OverlapBoxAll(descendingDarkExplosion.transform.position, playerAttackHalfSize, 0f, dealingEnemyDamageLayer);

        return PlayerAttackTouchingGivenEnemy(enemyHitbox);
    }

    public bool PlayerAttackTouchingGivenEnemy(Transform enemyHitbox) => enemyHitboxesHitWithPlayerAttack.Any(hit => hit.transform == enemyHitbox);
    
    public Collider2D[] AllEnemyHitboxesHitWithSlash(Transform slash)
    {
        Vector2 playerAttackHalfSize = new Vector2(playerNail.slashCurrentHorizontalHitbox, playerNail.slashCurrentVerticalHitbox);
        Collider2D[] enemyHitboxesHit = Physics2D.OverlapBoxAll(slash.transform.position, playerAttackHalfSize, 0f, dealingEnemyDamageLayer);

        return enemyHitboxesHit;
    }

    // it needs to be a lot (4x) bigger before given in this function
    public bool BossTouchingPlayer(Vector2 bossPosition, Vector2 bossHitbox) => Physics2D.OverlapBox(bossPosition, bossHitbox, 0f, playerLayer);
    
    private void FillEnemyDealingDamageToPlayerHitboxesList()
    {
        GameObject[] objectsWithLayer = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject enemyDealingDamageHitbox in objectsWithLayer)
        {
            if (enemyDealingDamageHitbox.gameObject.layer == Mathf.Log(takingEnemyDamageLayer.value, 2))
                enemyDealingDamageToPlayerHitboxes.Add(enemyDealingDamageHitbox.transform);
        }
    }

    public Transform GetEnemyBodyThatPlayerIsTouching()
    {
        float closestEnemyDistance = int.MaxValue;
        Transform closestEnemyHitbox = null;

        foreach (Transform enemyDamagingThePlayerHitbox in enemyDealingDamageToPlayerHitboxes)
        {
            float distance = enemyDamagingThePlayerHitbox.Distance(playerMovement.GetPlayerRigidBody().transform);  

            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                closestEnemyHitbox = enemyDamagingThePlayerHitbox;
            }
        }
        return closestEnemyHitbox;
    }

    // jer ARGUMENTI: 1 - telo, 2 - precnik kruga, 3 - LAYER koji ispituje da li TELO dodiruje
    public bool PlayerTouchingEnemyBody() => Physics2D.OverlapCircle(playerMovement.GetPlayerRigidBody().position, 0.1f, takingEnemyDamageLayer);

    // HARDCODED
    public bool AnyEnemyGotHitByAttack(Transform playerAttack) => boss1Health.EnemyHitByPlayerAttack(playerAttack) || boss2Health.EnemyHitByPlayerAttack(playerAttack);

    public void KnockBackPlayerForBlockingAttackCheck(Transform bossDealingDamageHitbox, Vector2 bossParryableAttackHitboxSize, bool bossPerformingParryableAttack)
    {
        if (playerNail.isBlocking && !playerNail.isParrying && playerMovement.currentKnockbackDuration == 0 &&
            bossPerformingParryableAttack && BossTouchingPlayer(bossDealingDamageHitbox.position, bossParryableAttackHitboxSize))
        {
            playerMovement.BouncePlayerToRightSide(playerMovement.GetPlayerIsFacingRight(), playerNail.totalKnockbackDurationForBlockingParryableAttack);
            audioManager.PlayAudioClip(audioManager.playerNailSoundsWithoutSlash, "Block");
        }    
    }

    public void ParryBossCheck(ref int currentBossStunDuration, bool bossPerformingParryableAttack, Transform bossDealingDamageHitbox, Vector2 bossParryableAttackHitboxSize)
    {
        if (playerNail.isParrying && bossPerformingParryableAttack && BossTouchingPlayer(bossDealingDamageHitbox.position, bossParryableAttackHitboxSize))
        {
            currentBossStunDuration = totalBossStunDurationAfterPlayerParriedIt;
            audioManager.PlayAudioClip(audioManager.playerNailSoundsWithoutSlash, "Parry");
        }  
    }

    // HARDCODED
    public void LowerAllEnemyHealthCheck(Transform playerAttack, int damageAmount)
    {
        if (playerAttack == null) 
            return;

        boss1Health.LowerEnemyHealthAndStaggerCheck(playerAttack, damageAmount);
        boss2Health.LowerEnemyHealthAndStaggerCheck(playerAttack, damageAmount);
    }

    // HARDCODED
    public bool EnemyGotHitByPlayer(Transform playerAttack, Transform enemyTakingDamageHitboxes)
    {
        if (playerAttack == null || enemyTakingDamageHitboxes == null) 
            return false;

        return (playerAttack.gameObject.name.Contains("Slash") && PlayerSlashTouchingEnemy(playerAttack, enemyTakingDamageHitboxes)) ||
               (playerAttack.gameObject.name.Contains("VengefulSpirit") && PlayerVengefulSpiritTouchingEnemy(playerAttack.GetComponent<Rigidbody2D>(), enemyTakingDamageHitboxes)) ||
               (playerAttack.gameObject.name.Contains("AbyssShriek") && PlayerAbyssShriekTouchingEnemy(playerAttack, enemyTakingDamageHitboxes)) ||
               (playerAttack.gameObject.name.Contains("Player") && PlayerDescendingDarkDescendTouchingEnemy(playerAttack, enemyTakingDamageHitboxes)) ||
               (playerAttack.gameObject.name.Contains("DescendingDarkExplosion") && PlayerDescendingDarkExplosionTouchingEnemy(playerAttack, enemyTakingDamageHitboxes));
    }
}
