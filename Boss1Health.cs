using UnityEngine;

public class Boss1Health : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] Boss1Behavior boss1Behavior;

    [SerializeField] private Transform boss1TakingDamageHitbox;

    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyVisuals enemyVisuals;


    //      HEALTH NUMBERS:


    public int totalBoss1Health = 2000;
    public int currentBoss1Health;


    //      SETUP:


    private void Start()
    {
        currentBoss1Health = totalBoss1Health;
    }


    //      METHODS:


    //   USTVARI, "boss1TakingDamageHitbox" treba da bude NIZ, ako je vise hitbox-eva, HARDCODE
    public bool EnemyHitByPlayerAttack(Transform playerAttack) => enemy.EnemyGotHitByPlayer(playerAttack, boss1TakingDamageHitbox);
    
    public void LowerEnemyHealthAndStaggerCheck(Transform playerAttack, int playerDamage)
    {
        if (playerAttack == null) 
            return;

        if (enemy.EnemyGotHitByPlayer(playerAttack, boss1TakingDamageHitbox))
            currentBoss1Health -= playerDamage;

        Coroutine coroutine = boss1Behavior.StartCoroutine(boss1Behavior.StaggerRecentDamageTakenTimer(playerDamage));
        boss1Behavior.staggerRecentDamageTakenTimerCoroutines.Add(coroutine);
        boss1Behavior.StaggerBossCheck();

        if (boss1Behavior.isStaggered && boss1Behavior.currentBossStunDuration <= boss1Behavior.totalStaggerDuration - boss1Behavior.staggerStartingFramesThatDontEndStaggerIfAttacked)
            boss1Behavior.currentBossStunDuration = 0;
    }

    public void AddEnemySpriteToListOfEnemySpritesToChangeColorForBeingHit(Transform playerAttack)
    {
        if (enemy.EnemyGotHitByPlayer(playerAttack, boss1TakingDamageHitbox))
        {
            foreach (Transform enemySpriteRenderer in boss1TakingDamageHitbox.gameObject.transform.parent)
            {
                if (enemySpriteRenderer.name.ToLower().Contains("sprite"))
                    enemyVisuals.GetEnemyGotHitHitboxSpritesList().Add(enemySpriteRenderer.transform.GetComponent<SpriteRenderer>());
            }
        }
    }
}
