using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private LayerMask dealingEnemyDamageLayer;
    [SerializeField] private LayerMask takingEnemyDamageLayer;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Boss1Health boss1Health;   // HARD CODED je to sto se dodaju svi boss-evi ovde
    [SerializeField] private Boss2Health boss2Health;


    //      GENERAL:


    private float enemyHitNormalColorTransitionTimeInSeconds = 0.3f;
    private Color enemyHitColor = new Color(0.8f, 0.4f, 0.4f, 0.4f);

    private List<SpriteRenderer> enemyGotHitHitboxSpritesList = new List<SpriteRenderer>();


    //      HELPER VARIABLES:


    private Color normalColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private float currentEnemyHitColorTransitionProgress = 0f;   // 0 - it has just started, 0.5 - halfway through the transition, 1 - transition over

    
    //      CODE:


    private void Update()
    {
        ChangeEnemySpritesColorForBeingHitCheck();
    }


    //      METHODS:
    

    public void FillListOfEnemySpritesToChangeColorForBeingHit(Transform playerAttack)
    {
        currentEnemyHitColorTransitionProgress = 0f;   // makes the sprites change color to the starting color if they are hit while they are transitioning their color
        boss1Health.AddEnemySpriteToListOfEnemySpritesToChangeColorForBeingHit(playerAttack);
        boss2Health.AddEnemySpriteToListOfEnemySpritesToChangeColorForBeingHit(playerAttack);
    }

    public List<SpriteRenderer> GetEnemyGotHitHitboxSpritesList() => enemyGotHitHitboxSpritesList;
    
    private void ChangeEnemySpritesColorForBeingHitCheck()
    {
        if (currentEnemyHitColorTransitionProgress > 1)
            enemyGotHitHitboxSpritesList.Clear();
        else if (currentEnemyHitColorTransitionProgress < 1)
            currentEnemyHitColorTransitionProgress += Time.deltaTime / enemyHitNormalColorTransitionTimeInSeconds;
        else if (currentEnemyHitColorTransitionProgress > 0)
            currentEnemyHitColorTransitionProgress -= Time.deltaTime / enemyHitNormalColorTransitionTimeInSeconds;

        // small problem - if multiple enemies get hit, and after that only one enemy keeps getting hit, all of them get their sprite color changed

        foreach (SpriteRenderer enemySpriteRenderer in enemyGotHitHitboxSpritesList)
            enemySpriteRenderer.color = Color.Lerp(enemyHitColor, normalColor, currentEnemyHitColorTransitionProgress);
    }
}
