using UnityEngine;
using UnityEngine.UI;

public class PlayerSoulVesselAnimations : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] PlayerSoulVessel playerSoulVessel;
    [SerializeField] private Canvas canvasItem;


    //      GENERAL:

    [SerializeField] private Image bigSoulVessel;
    [SerializeField] private Image smallSoulVessel1;
    [SerializeField] private Image smallSoulVessel2;
    [SerializeField] private Image smallSoulVessel3;

    private SpriteRenderer bigSoulVesselSprite;
    private SpriteRenderer smallSoulVessel1Sprite;
    private SpriteRenderer smallSoulVessel2Sprite;
    private SpriteRenderer smallSoulVessel3Sprite;

    Animator bigSoulVesselAnimator;
    Animator smallSoulVessel1Animator;
    Animator smallSoulVessel2Animator;
    Animator smallSoulVessel3Animator;

    string currentAnimationState;


    //      ANIMATIONS:


    const string PLAYER_BIG_SOUL_VESSEL_EMPTY_ANIMATION = "PlayerBigSoulVesselEmptyAnimation";
    const string PLAYER_BIG_SOUL_VESSEL_1_TO_32_ANIMATION = "PlayerBigSoulVessel1To32Animation";
    const string PLAYER_BIG_SOUL_VESSEL_33_TO_65_ANIMATION = "PlayerBigSoulVessel33To65Animation";
    const string PLAYER_BIG_SOUL_VESSEL_66_TO_98_ANIMATION = "PlayerBigSoulVessel66To98Animation";
    const string PLAYER_BIG_SOUL_VESSEL_FULL_ANIMATION = "PlayerBigSoulVesselFullAnimation";

    const string PLAYER_SMALL_SOUL_VESSEL_EMPTY_ANIMATION = "PlayerSmallSoulVesselEmptyAnimation";
    const string PLAYER_SMALL_SOUL_VESSEL_1_TO_10_ANIMATION = "PlayerSmallSoulVessel1To10Animation";
    const string PLAYER_SMALL_SOUL_VESSEL_11_TO_21_ANIMATION = "PlayerSmallSoulVessel11To21Animation";
    const string PLAYER_SMALL_SOUL_VESSEL_22_TO_32_ANIMATION = "PlayerSmallSoulVessel22To32Animation";
    const string PLAYER_SMALL_SOUL_VESSEL_FULL_ANIMATION = "PlayerSmallSoulVesselFullAnimation";


    //      HELPER VARIABLES:


    private int gameFrameCounter = 0;


    //      SETUP:


    private void Start()
    {
        (bigSoulVesselAnimator, smallSoulVessel1Animator, smallSoulVessel2Animator, smallSoulVessel3Animator) =
        (bigSoulVessel.GetComponent<Animator>(), smallSoulVessel1.GetComponent<Animator>(),
         smallSoulVessel2.GetComponent<Animator>(), smallSoulVessel3.GetComponent<Animator>());

        (bigSoulVesselSprite, smallSoulVessel1Sprite, smallSoulVessel2Sprite, smallSoulVessel3Sprite) =
        (bigSoulVessel.GetComponent<SpriteRenderer>(), smallSoulVessel1.GetComponent<SpriteRenderer>(),
         smallSoulVessel2.GetComponent<SpriteRenderer>(), smallSoulVessel3.GetComponent<SpriteRenderer>());
    }


    //      CODE:


    private void Update() //ovde izbegni update isto
    {
        if (!canvasItem.isActiveAndEnabled)
            return;
        
       ChangeSoulVesselsAnimationCheck();
    }


    //      METHODS:


    public void ChangeSoulVesselsAnimationCheck()
    {
        ChangeBigSoulVesselAnimationCheck();
        ChangeSmallSoulVesselsAnimationCheck();

        ChangeImageToSprite();
    }

    private void ChangeBigSoulVesselAnimationCheck()
    {
        if (playerSoulVessel.currentBigSoulVesselSoul == 0)
            Game.ChangeAnimationState(bigSoulVesselAnimator, currentAnimationState, PLAYER_BIG_SOUL_VESSEL_EMPTY_ANIMATION);
        else if (playerSoulVessel.currentBigSoulVesselSoul >= 1 && playerSoulVessel.currentBigSoulVesselSoul <= 32)
            Game.ChangeAnimationState(bigSoulVesselAnimator, currentAnimationState, PLAYER_BIG_SOUL_VESSEL_1_TO_32_ANIMATION);
        else if (playerSoulVessel.currentBigSoulVesselSoul >= 33 && playerSoulVessel.currentBigSoulVesselSoul <= 65)
            Game.ChangeAnimationState(bigSoulVesselAnimator, currentAnimationState, PLAYER_BIG_SOUL_VESSEL_33_TO_65_ANIMATION);
        else if (playerSoulVessel.currentBigSoulVesselSoul >= 66 && playerSoulVessel.currentBigSoulVesselSoul <= 98)
            Game.ChangeAnimationState(bigSoulVesselAnimator, currentAnimationState, PLAYER_BIG_SOUL_VESSEL_66_TO_98_ANIMATION);
        else if (playerSoulVessel.currentBigSoulVesselSoul == playerSoulVessel.maximumBigSoulVesselSoul)
            Game.ChangeAnimationState(bigSoulVesselAnimator, currentAnimationState, PLAYER_BIG_SOUL_VESSEL_FULL_ANIMATION);
    }

    private void ChangeSmallSoulVesselsAnimationCheck() //for unknown reasons, these functions cannot be called at the same frame
    {
        if (gameFrameCounter % 3 == 0)
            ChangeSmallSoulVessel1AnimationCheck();
        else if (gameFrameCounter % 3 == 1)
            ChangeSmallSoulVessel2AnimationCheck();
        else if (gameFrameCounter % 3 == 2)
        {
            ChangeSmallSoulVessel3AnimationCheck();
            gameFrameCounter = -1;
        }
        gameFrameCounter++;
    }

    private void ChangeSmallSoulVessel1AnimationCheck()
    {
        if (playerSoulVessel.currentSmallSoulVessel1Soul == 0)
            Game.ChangeAnimationState(smallSoulVessel1Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_EMPTY_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel1Soul >= 1 && playerSoulVessel.currentSmallSoulVessel1Soul <= 10)
            Game.ChangeAnimationState(smallSoulVessel1Animator, currentAnimationState,PLAYER_SMALL_SOUL_VESSEL_1_TO_10_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel1Soul >= 11 && playerSoulVessel.currentSmallSoulVessel1Soul <= 21)
            Game.ChangeAnimationState(smallSoulVessel1Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_11_TO_21_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel1Soul >= 22 && playerSoulVessel.currentSmallSoulVessel1Soul <= 32)
            Game.ChangeAnimationState(smallSoulVessel1Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_22_TO_32_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel1Soul == playerSoulVessel.maximumSmallSoulVesselSoul)
            Game.ChangeAnimationState(smallSoulVessel1Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_FULL_ANIMATION);
    }

    private void ChangeSmallSoulVessel2AnimationCheck()
    {
        if (playerSoulVessel.currentSmallSoulVessel2Soul == 0)
            Game.ChangeAnimationState(smallSoulVessel2Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_EMPTY_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel2Soul >= 1 && playerSoulVessel.currentSmallSoulVessel2Soul <= 10)
            Game.ChangeAnimationState(smallSoulVessel2Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_1_TO_10_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel2Soul >= 11 && playerSoulVessel.currentSmallSoulVessel2Soul <= 21)
            Game.ChangeAnimationState(smallSoulVessel2Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_11_TO_21_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel2Soul >= 22 && playerSoulVessel.currentSmallSoulVessel2Soul <= 32)
            Game.ChangeAnimationState(smallSoulVessel2Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_22_TO_32_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel2Soul == playerSoulVessel.maximumSmallSoulVesselSoul)
            Game.ChangeAnimationState(smallSoulVessel2Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_FULL_ANIMATION);
    }

    private void ChangeSmallSoulVessel3AnimationCheck()
    {
        if (playerSoulVessel.currentSmallSoulVessel3Soul == 0)
            Game.ChangeAnimationState(smallSoulVessel3Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_EMPTY_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel3Soul >= 1 && playerSoulVessel.currentSmallSoulVessel3Soul <= 10)
            Game.ChangeAnimationState(smallSoulVessel3Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_1_TO_10_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel3Soul >= 11 && playerSoulVessel.currentSmallSoulVessel3Soul <= 21)
            Game.ChangeAnimationState(smallSoulVessel3Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_11_TO_21_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel3Soul >= 22 && playerSoulVessel.currentSmallSoulVessel3Soul <= 32)
            Game.ChangeAnimationState(smallSoulVessel3Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_22_TO_32_ANIMATION);
        else if (playerSoulVessel.currentSmallSoulVessel3Soul == playerSoulVessel.maximumSmallSoulVesselSoul)
            Game.ChangeAnimationState(smallSoulVessel3Animator, currentAnimationState, PLAYER_SMALL_SOUL_VESSEL_FULL_ANIMATION);
    }

    //fixes the problem of Animations changing the "sprite" in the "SpriteRenderer" instead of "image source" in the "Image"
    private void ChangeImageToSprite() =>         
        (bigSoulVessel.sprite, smallSoulVessel1.sprite, smallSoulVessel2.sprite, smallSoulVessel3.sprite) =
        (bigSoulVesselSprite.sprite, smallSoulVessel1Sprite.sprite, smallSoulVessel2Sprite.sprite, smallSoulVessel3Sprite.sprite);
}
