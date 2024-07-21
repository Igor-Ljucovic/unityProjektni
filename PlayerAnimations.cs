using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerSpells playerSpells;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] public  Animator playerAnimator;


    //      General:


    public string currentAnimation;


    //      HELPER VARIABLES:


    private Quaternion normalSpriteRotation;


    //      SETUP:


    private void Start()
    {
        normalSpriteRotation = playerSprite.transform.rotation;
    }


    //      CODE:


    private void Update()
    {
        if (Game.Paused()) 
            return;

        RotatePlayerSpriteCheck();

        ChangePlayerAnimationCheck();
    }


    //      METHODS:


    private void ChangePlayerAnimationCheck()
    {
        if (playerHealth.doTakingDamageAnimation)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerTakingDamageAnimation");

        else if (playerMovement.isHardFallStunned)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerHardFallStunAnimation");
        else if (playerMovement.isFastFalling)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerFastFallingAnimation");
        else if (playerMovement.isDodging)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerDodgingAnimation");
        else if (playerSpells.isCastingVengefulSpirit)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerCastingVengefulSpiritAnimation");
        else if (playerSpells.isCastingAbyssalShriek)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerCastingAbyssalShriekAnimation");
        else if (playerMovement.isShadowDashing)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerShadowDashingAnimation");
        else if (playerSpells.isDescendingDarkAscending)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerDescendingDarkAscendingAnimation");
        else if (playerSpells.doDescendingDarkDescendingAnimation)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerDescendingDarkDescendingAnimation");
        else if (playerSpells.focusHealingHistory.Contains(true))
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerFocusingAnimation");
        else if (playerNail.isParrying)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerBlockingAnimation");

        else if (playerNail.isBlocking)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerBlockingAnimation");
        else if (playerMovement.isDashing)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerDashingAnimation");
        else if (playerMovement.isRolling)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerRollingAnimation");
        else if (playerNail.IsSlashing())
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerSlashingAnimation");

        else if (playerMovement.isLookingUp)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerLookingUpAnimation");
        else if (playerMovement.isWallSliding)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerWallClingingAnimation");
        else if (playerMovement.isJumping)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerJumpingAnimation");
        else if (playerMovement.isRunning)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerRunningAnimation");
        else if (playerMovement.isFalling)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerFallingAnimation");
        else if (playerMovement.isIdling)
            Game.ChangeAnimationState(playerAnimator, currentAnimation, "PlayerIdlingAnimation");
    }

    private void RotatePlayerSpriteCheck()
    {
        //these sprites are rotated the wrong way in the spritesheet - this functions corrects them 

        if (playerNail.isUpSlashing && playerMovement.GetPlayerIsFacingRight() || playerNail.isDownSlashing && !playerMovement.GetPlayerIsFacingRight())
            playerSprite.transform.rotation = new Quaternion(normalSpriteRotation.x, normalSpriteRotation.y, +0.33f, normalSpriteRotation.w);
        else if (playerNail.isDownSlashing && playerMovement.GetPlayerIsFacingRight() || playerNail.isUpSlashing && !playerMovement.GetPlayerIsFacingRight())
            playerSprite.transform.rotation = new Quaternion(normalSpriteRotation.x, normalSpriteRotation.y, -0.33f, normalSpriteRotation.w);

        else if (playerSpells.isDescendingDarkDescending && playerSprite.transform.rotation == normalSpriteRotation && playerMovement.GetPlayerIsFacingRight())
            playerSprite.transform.rotation = new Quaternion(normalSpriteRotation.x, normalSpriteRotation.y, -0.5f, normalSpriteRotation.w);
        else if (playerSpells.isDescendingDarkDescending && playerSprite.transform.rotation == normalSpriteRotation && !playerMovement.GetPlayerIsFacingRight())
            playerSprite.transform.rotation = new Quaternion(normalSpriteRotation.x, normalSpriteRotation.y, +0.5f, normalSpriteRotation.w);

        if (playerHealth.doTakingDamageAnimation && playerSprite.transform.rotation == normalSpriteRotation)
            playerSprite.transform.rotation = Quaternion.Euler(normalSpriteRotation.x, 180f, normalSpriteRotation.z);

        if (!playerHealth.doTakingDamageAnimation && !playerSpells.isDescendingDarkDescending && !playerNail.isUpSlashing && !playerNail.isDownSlashing)
            playerSprite.transform.rotation = normalSpriteRotation;
    }
}
