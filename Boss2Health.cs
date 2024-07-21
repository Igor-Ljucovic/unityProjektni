using UnityEngine;

public class Boss2Health : MonoBehaviour
{
    [SerializeField] private Transform boss2TakingDamageHitbox;

    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyVisuals enemyVisuals;

    public int boss2Health = 130;


    public bool EnemyHitByPlayerAttack(Transform playerAttack) => enemy.EnemyGotHitByPlayer(playerAttack, boss2TakingDamageHitbox);
    
    public void LowerEnemyHealthAndStaggerCheck(Transform playerAttack, int playerDamage)
    {
        if (playerAttack == null) 
            return;

        if (enemy.EnemyGotHitByPlayer(playerAttack, boss2TakingDamageHitbox))
            boss2Health -= playerDamage;
    }

    public void AddEnemySpriteToListOfEnemySpritesToChangeColorForBeingHit(Transform playerAttack)
    {
        if (enemy.EnemyGotHitByPlayer(playerAttack, boss2TakingDamageHitbox))
        {
            foreach (Transform enemySpriteRenderer in boss2TakingDamageHitbox.gameObject.transform.parent)
            {
                if (enemySpriteRenderer.name.ToLower().Contains("sprite"))
                    enemyVisuals.GetEnemyGotHitHitboxSpritesList().Add(enemySpriteRenderer.transform.GetComponent<SpriteRenderer>());
            }
        }
    }
}
