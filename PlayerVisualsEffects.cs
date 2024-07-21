using UnityEngine;

public class PlayerVisualsEffects : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerSpells playerSpells;
    [SerializeField] private Enemy enemy;
    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private SpriteRenderer slashParticles;


    //      Color Changes:


    private Color playerFocusingColor = new Color(1f, 1f, 1f, 0.6f);
    private float playerFocusingColorTransitionTimeInSeconds = 0.15f;

    private Color playerShadowDashReadyColor = new Color(0.25f, 0.25f, 0.25f, 1f);
    private float playerShadowDashReadyColorTransitionTimeInSeconds = 0.35f;
    private float playerShadowDashReadyColorTransitionsDuration = 0.25f;   // for example, starting and ending color transitions last for 20% and 20% respectively


    //      HELPER VARIABLES:

    private Color normalColor = new Color(1f, 1f, 1f, 1f);

    [SerializeField] private float currentPlayerFocusingColorTransitionProgress = 0f;
    [SerializeField] private float currentPlayerShadowDashReadyColorTransitionProgress = 0f;

    [SerializeField] private bool startChangingShadowDashColor;


    //      CODE:


    private void Update()
    {
        if (Game.Paused())
            return;

        ChangePlayerSpriteColorCheck();
    }

    private void ChangePlayerSpriteColorCheck()
    {
        PlayerFocusingSpriteColorCheck();
        PlayerShadowDashReadySpriteColorCheck();
    }

    private void PlayerFocusingSpriteColorCheck()
    {
        if (playerSpells.isFocusHealing && currentPlayerFocusingColorTransitionProgress < 1)
        {
            currentPlayerFocusingColorTransitionProgress += Time.deltaTime / playerFocusingColorTransitionTimeInSeconds;
            playerSprite.color = Color.Lerp(normalColor, playerFocusingColor, currentPlayerFocusingColorTransitionProgress);
        }
        else if (!playerSpells.isFocusHealing && currentPlayerFocusingColorTransitionProgress > 0)
        {
            currentPlayerFocusingColorTransitionProgress -= Time.deltaTime / playerFocusingColorTransitionTimeInSeconds;
            playerSprite.color = Color.Lerp(normalColor, playerFocusingColor, currentPlayerFocusingColorTransitionProgress);
        }
    }

    private void PlayerShadowDashReadySpriteColorCheck()
    {
        if (playerMovement.currentShadowDashCooldown == 1)
            startChangingShadowDashColor = true;

        if (playerSpells.focusHealingHistory.Contains(true) || !startChangingShadowDashColor)
        {
            startChangingShadowDashColor = false;
            return;
        }

        currentPlayerShadowDashReadyColorTransitionProgress += Time.deltaTime / playerShadowDashReadyColorTransitionTimeInSeconds;

        if (currentPlayerShadowDashReadyColorTransitionProgress < playerShadowDashReadyColorTransitionsDuration)
            playerSprite.color = Color.Lerp(normalColor, playerShadowDashReadyColor,
            currentPlayerShadowDashReadyColorTransitionProgress / playerShadowDashReadyColorTransitionsDuration);

        else if (currentPlayerShadowDashReadyColorTransitionProgress > 1 - playerShadowDashReadyColorTransitionsDuration)
            playerSprite.color = Color.Lerp(playerShadowDashReadyColor, normalColor,
            (currentPlayerShadowDashReadyColorTransitionProgress - (1 - playerShadowDashReadyColorTransitionsDuration)) / playerShadowDashReadyColorTransitionsDuration);

        if (startChangingShadowDashColor && playerSprite.color == normalColor)
        {
            startChangingShadowDashColor = false;
            currentPlayerShadowDashReadyColorTransitionProgress = 0f;
        }
    }

    public void SpawnSlashParticles(Transform slash)
    {
        foreach (Collider2D collider in enemy.AllEnemyHitboxesHitWithSlash(slash))
        {
            SpriteRenderer slashParticles = Instantiate(this.slashParticles, collider.transform.position, Quaternion.identity);
            slashParticles.transform.parent = collider.transform;
            slashParticles.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(slashParticles.gameObject, playerNail.slashDuration);
        }
    }
}
